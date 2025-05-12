using UnityEngine;

namespace ProjectTile
{
    public interface IProjectileMove
    {
        public ProjectileBase Projectile { get; set; }
        public void Move(float deltaTime);
    }
    
    public class NonTargetMove : IProjectileMove
    {
        public NonTargetMove(ProjectileBase projectile)
        {
            Projectile = projectile;
        }
        
        public ProjectileBase Projectile { get; set; }
        public void Move(float deltaTime)
        {
            Projectile.transform.position += deltaTime * Projectile.ownerStatus.speed.Value * Projectile.transform.forward;
        }
    }

    public class TargetMove : IProjectileMove
    {
        public TargetMove(ProjectileBase projectile)
        {
            Projectile = projectile;
        }
        
        private Vector3 direction;
        
        public ProjectileBase Projectile { get; set; }
        public void Move(float deltaTime)
        {
            if (Projectile.targetStatus != null && !Projectile.targetStatus.hp.IsMin)
            {
                direction = (Projectile.targetTransform.position - Projectile.transform.position).normalized;
            }
            else
            {
                Projectile.targetStatus = null;
                Projectile.targetTransform = null;
            }

            Projectile.transform.LookAt(Projectile.transform.position + direction);
            Projectile.transform.position += deltaTime * Projectile.ownerStatus.speed.Value * direction;
        }

        public void SetDirection(Vector3 dir)
        {
            direction = dir.normalized;
        }
    }

    public class TargetAroundMove : IProjectileMove
    {
        public TargetAroundMove(ProjectileBase projectile)
        {
            Projectile = projectile;
        }
        
        public ProjectileBase Projectile { get; set; }
        public float radius;
        public Vector3 rotateAxis;
        public float rotateSpeed = 1f;

        private float angle = 0;
        public void Move(float deltaTime)
        {
            angle += rotateSpeed * deltaTime;
            Vector3 offset = Quaternion.AngleAxis(Mathf.Rad2Deg * angle, rotateAxis) * (rotateAxis * radius);
            Projectile.transform.position = Projectile.targetTransform.position + offset;
        }
    }
}
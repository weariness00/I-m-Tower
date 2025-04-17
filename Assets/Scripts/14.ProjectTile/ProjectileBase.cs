using System;
using Game.Status;
using UnityEngine;
using UnityEngine.Pool;

namespace ProjectTile
{
    public class ProjectileBase : MonoBehaviour
    {
        [HideInInspector] public GameObject ownerObject;
        [HideInInspector] public StatusBase ownerStatus;
        
        [HideInInspector] public Transform targetTransform;
        [HideInInspector] public StatusBase targetStatus; 
        
        [HideInInspector] public new Collider collider;

        public ObjectPool<ProjectileBase> pool;

        private Vector3 direction;

        public virtual void Awake()
        {
            collider = GetComponent<Collider>();
        }

        public virtual void Update()
        {
            if (targetStatus != null && !targetStatus.Hp.IsMin)
            {
                direction = (targetTransform.position - transform.position).normalized;
            }
            else
            {
                targetStatus = null;
                targetTransform = null;
            }

            transform.LookAt(transform.position + direction);
            transform.position += Time.deltaTime * ownerStatus.Speed * direction;
        }

        public virtual void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out StatusBase otherStatus) &&
                !otherStatus.Hp.IsMin)
            {
                otherStatus.Damaged(ownerStatus.Damage);
                pool.Release(this);
            }
        }
    }
}
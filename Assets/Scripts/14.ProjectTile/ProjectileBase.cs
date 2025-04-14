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
        [HideInInspector] public new Collider collider;

        public ObjectPool<ProjectileBase> pool;

        private Vector3 targetPosition;

        public virtual void Awake()
        {
            collider = GetComponent<Collider>();
        }

        public virtual void Update()
        {
            if (targetTransform != null)
            {
                targetPosition = targetTransform.position;
            }

            transform.LookAt(targetPosition);
            transform.position += Time.deltaTime * ownerStatus.Speed * transform.forward;
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
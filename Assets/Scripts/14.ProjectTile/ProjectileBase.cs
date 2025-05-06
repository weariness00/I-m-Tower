using System;
using Status;
using UnityEngine;
using UnityEngine.Pool;
using Util;

namespace ProjectTile
{
    public class ProjectileBase : MonoBehaviour, IPoolOnOff
    {
        [HideInInspector] public GameObject ownerObject;
        [HideInInspector] public StatusBase ownerStatus;
        
        [HideInInspector] public Transform targetTransform;
        [HideInInspector] public StatusBase targetStatus; 
        
        [HideInInspector] public new Collider collider;

        public ObjectPool<ProjectileBase> pool;
        public IProjectileMove Move;

        public virtual void Awake()
        {
            collider = GetComponent<Collider>();
        }

        public virtual void Start()
        {
            
        }

        public virtual void Update()
        {
            Move?.Move(Time.deltaTime);
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

        public virtual void PoolOn()
        {
            
        }

        public virtual void PoolOff()
        {
        }
    }
}
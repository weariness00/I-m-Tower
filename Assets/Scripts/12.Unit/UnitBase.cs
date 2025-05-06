using System;
using Status;
using UnityEngine;

namespace Unit
{
    public class UnitBase : MonoBehaviour
    {
        [HideInInspector] public new Collider collider;
        [NonSerialized] public StatusBase status;

        public int id;
        
        public virtual void Awake()
        {
            collider = GetComponent<Collider>();
            status = GetComponent<StatusBase>();
        }

        public virtual void InitStatus()
        {
        }
    }
}


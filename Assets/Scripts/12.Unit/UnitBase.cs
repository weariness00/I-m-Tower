using System;
using Game.Status;
using UnityEngine;

namespace Unit
{
    public class UnitBase : MonoBehaviour
    {
        [NonSerialized] public StatusBase status;
        
        public virtual void Awake()
        {
            status = GetComponent<StatusBase>();
        }
    }
}


using System;
using UnityEngine;
using UnityEngine.AI;

namespace Unit.Monster
{
    public class MonsterControl : UnitBase
    {
        [HideInInspector]public NavMeshAgent navMeshAgent;

        public override void Awake()
        {
            base.Awake();
            navMeshAgent = GetComponent<NavMeshAgent>();
        }
    }
}


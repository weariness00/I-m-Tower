using System;
using UnityEngine;
using UnityEngine.AI;

namespace Unit.Monster
{
    public class MonsterControl : UnitBase
    {
        [HideInInspector]public NavMeshAgent navMeshAgent;
        [NonSerialized] public new MonsterStatus status;

        public override void Awake()
        {
            base.Awake();
            navMeshAgent = GetComponent<NavMeshAgent>();
            status = base.status as MonsterStatus;
        }
    }
}


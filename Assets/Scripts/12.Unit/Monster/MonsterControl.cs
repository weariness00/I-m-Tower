using System;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;

namespace Unit.Monster
{
    public class MonsterControl : UnitBase
    {
        [HideInInspector] public BehaviorGraphAgent behaviorGraphAgent;
        [HideInInspector] public NavMeshAgent navMeshAgent;
        [NonSerialized] public new MonsterStatus status;

        public override void Awake()
        {
            base.Awake();
            behaviorGraphAgent = GetComponent<BehaviorGraphAgent>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            status = base.status as MonsterStatus;
        }

        public override void InitStatus()
        {
            base.InitStatus();
            status.value.Copy(UnitSOManager.GetMonsterSO(id).data);
        }
    }
}


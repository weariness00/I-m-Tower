using System;
using Looting;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;
using Util.AStar;

namespace Unit.Monster
{
    public class MonsterControl : UnitBase
    {
        [HideInInspector] public BehaviorGraphAgent behaviorGraphAgent;
        [HideInInspector] public NavMeshAgent navMeshAgent;
        [HideInInspector] public AStarAgent aStarAgent;
        [NonSerialized] public new MonsterStatus status;

        public override void Awake()
        {
            base.Awake();
            behaviorGraphAgent = GetComponent<BehaviorGraphAgent>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            aStarAgent = GetComponent<AStarAgent>();
            status = base.status as MonsterStatus;
            
            status.onDieEvent.AddListener(OnLooting);
        }

        public void Update()
        {
            aStarAgent.Look();
            aStarAgent.Move(status.Speed * Time.deltaTime);
        }

        public override void InitStatus()
        {
            base.InitStatus();
            status.value.Copy(UnitSOManager.GetMonsterSO(id).data);
        }

        protected virtual void OnLooting()
        {
            LootingTableHandleSO.GetLootingTableSO(0).OnLooting(transform);
        }
    }
}


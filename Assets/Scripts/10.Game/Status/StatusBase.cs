using System;
using System.Collections;
using System.Threading;
using Manager;
using UnityEngine;
using UnityEngine.Events;
using Util;

namespace Game.Status
{
    public class StatusBase : MonoBehaviour
    {
        [InspectorName("기본 Data")] public StatusData value = new();

        public UnityEvent onDieEvent = new ();
        public Action<int> onDamagedEvent;
        
        public CancellationTokenSource dieCancelToken = new CancellationTokenSource();
        private int stunRefCount = 0; // 현재 스턴중인 횟수
        public bool isStun;
            
        public MinMaxValue<int> Hp => value.hp;
        [HideInInspector] public float speedMultiple = 1f;
        public float Speed => value.speed * speedMultiple;

        public virtual int Damage => Mathf.FloorToInt(value.damage * (value.criticalChange.IsProbability() ? 1f : value.criticalMultiple));
        public float moreDamageMultiple = 1f;
        public float DamageMultiple
        {
            get => value.damageMultiple;
            set => this.value.damageMultiple = value;
        }
        public float AttackRange => value.attackRange;
        public MinMaxValue<float> AttackTimer => value.attackTimer;
        public float AttackSpeed => 1f / value.attackTimer.Max; 
        
        public int Defense => Mathf.FloorToInt(value.defense * DefenseMultiple);
        public float DefenseMultiple = 1f;
        
        public virtual void OnDrawGizmos()
        {
            if (AttackRange > 0)
            {
                Gizmos.DrawWireSphere(transform.position, AttackRange);
            }
        }

        public virtual void Damaged(int atk, int defencePenetrationValue = 0)
        {
            var realDamage =  Mathf.FloorToInt(atk * moreDamageMultiple) - (Defense - defencePenetrationValue);
            Hp.Current -= realDamage;
            onDamagedEvent?.Invoke(realDamage);
            
            DebugManager.Log($"{name}이 {realDamage}만큼 피해를 입었습니다.");

            if (Hp.IsMin)
            {
                onDieEvent.Invoke();
                dieCancelToken.Cancel();
                dieCancelToken.Dispose();
                dieCancelToken = new();
            }
        }

        // 스턴은 누적되지 않는다.
        public void Stun(float duration) => StartCoroutine(StunCoroutine(duration));
        private IEnumerator StunCoroutine(float duration)
        {
            DebugManager.Log($"{name}이 스턴에 걸렸습니다.");
            
            ++stunRefCount;
            isStun = true;
            yield return new WaitForSeconds(duration);
            --stunRefCount;
            if (stunRefCount <= 0)
            {
                isStun = false;
                DebugManager.Log($"{name}이 스턴에서 해제되었습니다.");
            }
        }
    }
}


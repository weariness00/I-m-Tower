using System;
using UnityEngine;
using UnityEngine.Events;
using Util;

namespace Game.Status
{
    public class StatusBase : MonoBehaviour
    {
        [InspectorName("기본 Data")] public StatusData value = new();

        public UnityEvent<int> onDieEvent = new ();
            
        public MinMaxValue<int> Hp => value.hp;
        public float Speed => value.speed;

        public virtual int Damage => (int)value.damage;
        public float DamageMultiple
        {
            get => value.damageMultiple;
            set => this.value.damageMultiple = value;
        }
        public float AttackRange => value.attackRange;
        public MinMaxValue<float> AttackTimer => value.attackTimer;
        public float AttackSpeed => 1f / value.attackTimer.Max; 
        public virtual void OnDrawGizmos()
        {
            if (AttackRange > 0)
            {
                Gizmos.DrawWireSphere(transform.position, AttackRange);
            }
        }

        public virtual void Damaged(int atk)
        {
            Hp.Current -= atk;

            if (Hp.IsMin)
            {
                onDieEvent.Invoke(atk);
            }
        }
    }
}


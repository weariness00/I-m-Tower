using System;
using UnityEngine;
using UnityEngine.Events;
using Util;

namespace Game.Status
{
    [Serializable]
    public class StatusData
    {
        public MinMaxValue<int> hp = new(10,0,10);
        public float speed = 1;

        public float damage = 1;
        public float attackRange = 1;
        public MinMaxValue<float> attackTimer = new(0, 0, 1, false, true);

        public void Copy(StatusData otherData)
        {
            hp.Min = otherData.hp.Min;
            hp.Max = otherData.hp.Max;
            hp.Current = otherData.hp.Current;

            speed = otherData.speed;
        
            damage = otherData.damage;
            attackRange = otherData.attackRange;
            attackTimer.Max = otherData.attackTimer.Max;
        }
    }

    public class StatusBase : MonoBehaviour
    {
        [SerializeField] private StatusData value = new();

        public UnityEvent<int> onDieEvent = new ();
            
        public MinMaxValue<int> Hp => value.hp;
        public float Speed => value.speed;

        public int Damage => (int)value.damage;
        public float AttackRange => value.attackRange;
        public MinMaxValue<float> AttackTimer => value.attackTimer;

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


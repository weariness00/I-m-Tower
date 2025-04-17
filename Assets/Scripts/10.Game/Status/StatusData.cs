using System;
using UnityEngine;
using Util;

namespace Game.Status
{
    [Serializable]
    public class StatusData
    {
        public MinMaxValue<int> hp = new(10,0,10);
        public float speed = 1;

        public float damage = 1;
        [Tooltip("공격력 배율")]public float damageMultiple = 1f;
        public float attackRange = 1;
        public MinMaxValue<float> attackTimer = new(0, 0, 1, false, false);

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
}
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
        [Tooltip("공격력 배율")] public float damageMultiple = 1f;
        [Tooltip("치명타 확률")] public float criticalChange = 0.1f;
        [Tooltip("치명타 배율")] public float criticalMultiple = 2f;
        public float attackRange = 1;
        public MinMaxValue<float> attackTimer = new(0, 0, 1, false, false);
        [Tooltip("방어력")]public float defense = 0;
        
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
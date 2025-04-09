using System;
using UnityEngine;
using Util;

namespace Game.Status
{
    [Serializable]
    public class StatusData
    {
        public MinMaxValue<int> hp = new(10,0,10);
        public float damage = 1;
        public float speed = 1;
    }

    public class StatusBase : MonoBehaviour
    {
        [SerializeField] private StatusData value = new();

        public MinMaxValue<int> Hp => value.hp;
        public float Damage => value.damage;
        public float Speed => value.speed;
    }
}


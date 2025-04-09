using UnityEngine;
using Util;

namespace Game.Status
{
    public class StatusBase : MonoBehaviour
    {
        public MinMaxValue<int> hp = new(10,0,10);
        public float damage = 1;
        public float speed = 1;
    }
}


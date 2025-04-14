using Game.Status;
using UnityEngine;
using UnityEngine.Events;
using Util;

namespace Tower
{
    public class TowerStatus : StatusBase
    {
        [Header("Tower Status 관련")]
        public int level;
        public MinMaxValue<float> experience = new(0,0,1,false,true);

        public UnityEvent onLevelUpEvent;

        public void AddEXP(float exp)
        {
            experience.Current += exp;
            if (experience.IsMax)
            {
                experience.Current -= experience.Max;
                experience.Max *= 1.2f;

                ++level;
                onLevelUpEvent.Invoke();
            }
        }
    }
}
using System;
using UnityEngine;
using Util.Condition;

namespace Rebirth
{
    // 환생 시스템
    public class RebirthSystem : MonoBehaviour
    {
        public Action onSuccessRebirthEvent;
        public ConditionUtil rebirthCondition; // 환생 조건
        public int point; // 환생 포인트

        public void OnRebirth()
        {
            if (rebirthCondition.IsCondition)
            {
                point += 100;
                onSuccessRebirthEvent?.Invoke();
            }
        }
    }
}
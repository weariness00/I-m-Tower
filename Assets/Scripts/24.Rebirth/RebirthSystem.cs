using System;
using UnityEngine;

namespace Rebirth
{
    // 환생 시스템
    public class RebirthSystem : MonoBehaviour
    {
        public event Action onRebirthEvent; // 환생에 성공했을 경우 호출되는 이벤트 
        public delegate bool RebirthCondition(); 
        public event RebirthCondition onRebirthConditionEvent; // 환생 조건들 모든 조건을 만족해야 환생 가능 
        
        // 환생 조건 확인
        public bool CheckRebirthCondition()
        {
            if (onRebirthConditionEvent != null)
            {
                foreach (var del in onRebirthConditionEvent.GetInvocationList())
                {
                    if (del is RebirthCondition condition && !condition())
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        
        // 환생
        public void OnRebirth()
        {
            if(CheckRebirthCondition() == false) return;
            onRebirthEvent?.Invoke();
        }
    }
}
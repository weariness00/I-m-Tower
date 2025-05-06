using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class GamePlayStateManager
    {
        // 시간 단위로 리셋되는 통계값
        private float elapsedTime = 0f;
        
        // 초당 통계값
        private float damageThisSecond = 0;
        private int killsThisSecond = 0;
        private float expThisSecond = 0;

        // 전체 누적값
        public int TotalDamage { get; private set; }
        public int TotalKills { get; private set; }
        public float TotalExp { get; private set; }

        // 초당 수치 (최근 1초)
        public float Dps => damageThisSecond;
        public int KillsPerSecond => killsThisSecond;
        public float ExpPerSecond => expThisSecond;
        
        private void Update()
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= 1f)
            {
                ResetSecondStats();
                elapsedTime = 0f;
            }
        }
        
        private void ResetSecondStats()
        {
            damageThisSecond = 0;
            killsThisSecond = 0;
            expThisSecond = 0;
        }
        
        public void AddDamage(int amount)
        {
            damageThisSecond += amount;
            TotalDamage += amount;
        }

        public void AddKill()
        {
            killsThisSecond++;
            TotalKills++;
        }

        public void AddExp(float amount)
        {
            expThisSecond += amount;
            TotalExp += amount;
        }
    }
}
using System;
using Game;
using Game.Age;
using Leveling.Stage;
using Manager;
using UnityEngine;

namespace Leveling
{
    public class LevelingControl : MonoBehaviour
    {
        public int currentStageLevel;
        public StageData currentStageData;

        public Action onStageClear;

        private int aliveMonsterCount = 0; // 임시

        public void StageStart()
        {
            DebugManager.ToDo("몬스터 다 처치 어떻게 처리할지 생각하기");
            aliveMonsterCount = 1;
            foreach (var monsterSpawner in GameManager.Instance.ageMap.CurrentMapData.monsterSpawnerArray)
            {
                monsterSpawner.Play();
            }
        }

        public void StageClear()
        {
            foreach (var monsterSpawner in GameManager.Instance.ageMap.CurrentMapData.monsterSpawnerArray)
            {
                monsterSpawner.Stop();
                monsterSpawner.Clear();
                monsterSpawner.Init();
            }
            
            if (currentStageLevel == 100 && GameManager.Instance.currentAge != AgeType.Endless)
            {
                currentStageLevel = 1;
                GameManager.Instance.currentAge += 1;
                GameManager.Instance.ageMap.ChangeMap(GameManager.Instance.currentAge);
                
                DebugManager.ToDo("몬스터 다 처치한 후 클리어 하는거 이게 최선인지 다시 생각하기");
                foreach (var monsterSpawner in GameManager.Instance.ageMap.CurrentMapData.monsterSpawnerArray)
                {
                    monsterSpawner.onReleaseSuccess += monster =>
                    {
                        if (--aliveMonsterCount == 0)
                        {
                            StageClear();
                            StageStart();
                        }
                    };
                }
            }
            else
                currentStageLevel++;

            currentStageData = LevelingDataSO.Instance.GetStageDataSO(GameManager.Instance.currentAge).GetStage(currentStageLevel);

            onStageClear?.Invoke();
        }
    }
}
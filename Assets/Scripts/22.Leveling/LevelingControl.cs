using System;
using Leveling.Age;
using Leveling.Stage;
using Manager;
using UnityEngine;

namespace Leveling
{
    public class LevelingControl : MonoBehaviour
    {
        [Header("시대")]
        public AgeMap currentMap;
        public Action onStageClear;
        
        [Header("Stage 관련")]
        public StageData currentStageData;
        public int currentStageLevel;
        private int aliveMonsterCount = 0; // 임시

        public void Start()
        {
            DebugManager.ToDo("일단 임시로 무조건 스테이지 시작되게함");
            currentMap.ChangeMap(currentMap.currentAge);
            StageStart();
        }

        public void StageStart()
        {
            DebugManager.ToDo("몬스터 다 처치 어떻게 처리할지 생각하기");
            aliveMonsterCount = 1;
            foreach (var monsterSpawner in currentMap.CurrentMapData.monsterSpawnerArray)
            {
                monsterSpawner.Play();
            }
        }

        public void StageClear()
        {
            foreach (var monsterSpawner in currentMap.CurrentMapData.monsterSpawnerArray)
            {
                monsterSpawner.Stop();
                monsterSpawner.Clear();
                monsterSpawner.Init();
            }
            
            if (currentStageLevel == 100 && currentMap.currentAge != AgeType.Endless)
            {
                currentStageLevel = 1;
                currentMap.ChangeMap(currentMap.currentAge + 1);
                
                DebugManager.ToDo("몬스터 다 처치한 후 클리어 하는거 이게 최선인지 다시 생각하기");
                foreach (var monsterSpawner in currentMap.CurrentMapData.monsterSpawnerArray)
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

            currentStageData = LevelingDataSO.Instance.GetStageDataSO(currentMap.currentAge).GetStage(currentStageLevel);

            onStageClear?.Invoke();
        }
    }
}
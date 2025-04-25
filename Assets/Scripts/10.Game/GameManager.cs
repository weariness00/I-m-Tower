using System;
using Game.Age;
using Leveling;
using Tower;
using UnityEngine;
using Util;

namespace Game
{
    public class GameManager : Singleton<GameManager>
    {
        public AgeType currentAge;
        
        public TowerControl tower;
        public LevelingControl levelingControl;
        public AgeMap ageMap;
        
        protected override void Initialize()
        {
            base.Initialize();
            IsDontDestroy = false;
        }

        public void Start()
        {
            tower.status.AddEXP(tower.status.experience.Max);
            
            levelingControl.StageStart();
        }
    }
}


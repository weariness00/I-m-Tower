using System;
using Tower;
using UnityEngine;
using Util;

namespace Game
{
    public class GameManager : Singleton<GameManager>
    {
        public TowerControl tower;
        
        protected override void Initialize()
        {
            base.Initialize();
            IsDontDestroy = false;
        }

        public void Start()
        {
            tower.status.AddEXP(tower.status.experience.Max);
        }
    }
}


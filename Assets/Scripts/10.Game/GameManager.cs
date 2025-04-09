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
    }
}


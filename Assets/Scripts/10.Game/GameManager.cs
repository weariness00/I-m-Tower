using Util;

namespace Game
{
    public partial class GameManager : Singleton<GameManager>
    {
        public GamePlayStateManager playState = new();
        
        protected override void Initialize()
        {
            base.Initialize();
            IsDontDestroy = false;
        }
    }
}


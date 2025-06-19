using Util;

namespace Status
{
    public interface IDebuffController
    {
        public IDebuffData Data { get; }
        
        void Enter(StatusBase targetStatus);
        void Execute(StatusBase targetStatus);
        void End(StatusBase targetStatus);
    }

    public interface IDebuffData
    {
        
    }

    public interface IBleedingController : IDebuffController
    {
        public IBleedingData BleedingData { get; }
    }
    
    public interface IBleedingData : IDebuffData
    {
        public static float Tick => 0.5f;
        
        MinMaxValue<int> BleedingStack { get; set; }
        float BleedingChance { get; set; }
        float BleedingDamage { get; }
        float BleedingDuration { get; set; }
    }
}
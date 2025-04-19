namespace Looting
{
    public interface ILootingItem
    {
        public int ID { get; set; }
    }
    
    public interface ILootingSuccess
    {
        public void OnLootingSuccess();
    }
}
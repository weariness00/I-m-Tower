using UnityEngine;

namespace Leveling
{
    [CreateAssetMenu(fileName = "Leveling Data", menuName = "Game/Leveling/Level", order = 0)]
    public class LevelDataSO : ScriptableObject
    {
        [SerializeField] private LevelData[] levelDataArray;

        public void Init()
        {
            
        }
    }
}
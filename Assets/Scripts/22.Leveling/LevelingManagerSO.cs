using UnityEngine;

namespace Leveling
{
    [CreateAssetMenu(fileName = "Level Manager", menuName = "Game/Leveling/Level Manager", order = 0)]
    public class LevelingManagerSO : ScriptableObject
    {
        [SerializeField] private LevelDataSO[] levelSOArray;
    }
}
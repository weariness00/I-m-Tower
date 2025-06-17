using UnityEngine;

namespace Game
{
    public class MenuUIData : MonoBehaviour
    {
        [Tooltip("Menu와 연동할 모든 Canvas")] public GameObject[] menuObjectArray;

        public RectTransform skillLogRectTransform;
        public void Awake()
        {
            foreach (var menuObj in menuObjectArray)
            {
                if(menuObj.activeSelf)
                    menuObj.SetActive(false);
            }
        }
    }
}
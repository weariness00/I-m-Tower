using UnityEngine;

namespace Game
{
    public class MenuUI : MonoBehaviour
    {
        [Tooltip("Menu와 연동할 모든 Canvas")] public GameObject[] menuObjectArray;
        
        public void OnOff(GameObject obj)
        {
            var active = obj.activeSelf;

            // Menu와 연결된 Canvas 전부 비활성화
            foreach (var o in menuObjectArray)
                o.SetActive(false);
            
            // 선택된 메뉴 Canvas Active 설정
            obj.SetActive(!active);
        }
    }
}
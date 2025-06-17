using System;
using UnityEngine;
using Util;

namespace Game
{
    public class MenuUI : MonoBehaviour
    {
        public MenuUIData menuUIData;

        public void OnOff(GameObject obj)
        {
            // 이미 켜져 있으면 반환
            if (obj.activeSelf)
            {
                obj.SetActive(false);
                menuUIData.skillLogRectTransform.SetBottomHeightPositionToSource(menuUIData.skillLogRectTransform.GetComponentInParent<Canvas>().gameObject);
                return;
            }

            // Menu와 연결된 Canvas 전부 비활성화
            foreach (var o in menuUIData.menuObjectArray)
                o.SetActive(false);
            
            // 선택된 메뉴 Canvas Active 설정
            obj.SetActive(true);
            menuUIData.skillLogRectTransform.SetTopHeightPositionToSource(obj);
        }
    }
}
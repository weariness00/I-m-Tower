using System;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tower
{
    public partial class TowerPointView : MonoBehaviour
    {
        [NonSerialized] public TowerPointData pointData;
        [Tooltip("Tech Point 레벨 업 버튼")] public Button techPointLevelUpButton;
        [Tooltip("Bio Point 레벨 업 버튼")] public Button bioPointLevelUpButton;
        public UIData techUI;
        public UIData bioUI;

        public void Awake()
        {
            techUI.toggle.onValueChanged.AddListener(techUI.OnValueChangedToggle);
            bioUI.toggle.onValueChanged.AddListener(bioUI.OnValueChangedToggle);
        }

        public void SetPointData(TowerPointData data)
        {
            pointData = data;
            pointData.techPointReactive.Subscribe(value => techUI.pointText.text = value.ToString());
            pointData.bioPointReactive.Subscribe(value => bioUI.pointText.text = value.ToString());
        }
    }
    
    public partial class TowerPointView
    {
        [Serializable]
        public class UIData
        {
            public Canvas canvas;
            public Toggle toggle;
            
            public TMP_Text pointText;
            public Button skillDrawButton;

            public void OnValueChangedToggle(bool value) => canvas.gameObject.SetActive(value);
        }
    }
}
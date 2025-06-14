using System;
using R3;
using TMPro;
using UnityEngine;

namespace Tower
{
    public class TowerPointView : MonoBehaviour
    {
        public TowerPointData pointData;
        public TMP_Text techPointText;
        public TMP_Text bioPointText;

        public void Start()
        {
            SetPointData(pointData);
        }

        public void SetPointData(TowerPointData data)
        {
            pointData = data;
            pointData.techPointReactive.Subscribe(value => techPointText.text = value.ToString());
            pointData.bioPointReactive.Subscribe(value => bioPointText.text = value.ToString());
        }
        
    }
}
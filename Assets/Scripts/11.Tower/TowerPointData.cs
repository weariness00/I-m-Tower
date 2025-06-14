using System;
using UnityEngine;

namespace Tower
{
    public class TowerPointData : MonoBehaviour
    {
        [Tooltip("기술 포인트")][SerializeField] private int techPoint = 0;
        [Tooltip("생물 포인트")][SerializeField] private int bioPoint = 0;

        public void AddTechPoint(int value) => techPoint += value;
        
        public void UseTechPoint(int useValue, Action useCompleteAction)
        {
            if(techPoint > useValue) return;
            
            techPoint -= useValue;
            useCompleteAction?.Invoke();
        }
        
        public void AddBioPoint(int value) => bioPoint += value;
        public void UseBioPoint(int useValue, Action useCompleteAction)
        {
            if(bioPoint > useValue) return;
            
            bioPoint -= useValue;
            useCompleteAction?.Invoke();
        }
    }
}
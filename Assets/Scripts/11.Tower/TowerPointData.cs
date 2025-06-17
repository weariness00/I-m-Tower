using System;
using R3;
using UnityEngine;

namespace Tower
{
    public class TowerPointData : MonoBehaviour
    {
        public enum PointType
        {
            [InspectorName("기술")] Tech,
            [InspectorName("생명")] Bio
        }
        
        [Tooltip("기술 포인트")][SerializeField] private int techPoint = 0;
        [Tooltip("생물 포인트")][SerializeField] private int bioPoint = 0;
        
        [NonSerialized] public ReactiveProperty<int> techPointReactive = new ReactiveProperty<int>(0);
        [NonSerialized] public ReactiveProperty<int> bioPointReactive = new(0);

        public void Awake()
        {
            techPointReactive.Value = techPoint;
            bioPointReactive.Value = bioPoint;
        }
        
        public void AddPoint(PointType pointType, int value)
        {
            if (pointType == PointType.Tech)
            {
                techPoint += value;
                techPointReactive.Value = techPoint;
            }
            else if (pointType == PointType.Bio)
            {
                bioPoint += value;
                bioPointReactive.Value = bioPoint;
            }
        }

        public bool UsePoint(PointType type, int useValue)
        {
            switch (type)
            {
                case PointType.Tech:
                    if (techPoint < useValue) return false;
                    techPoint -= useValue;
                    techPointReactive.Value = techPoint;
                    return true;
                case PointType.Bio:
                    if(bioPoint < useValue) return false;
                    bioPoint -= useValue;
                    bioPointReactive.Value = bioPoint;
                    return true;
                default:
                    return false;
            }
        }
    }
}
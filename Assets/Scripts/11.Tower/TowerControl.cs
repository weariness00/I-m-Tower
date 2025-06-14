using System;
using System.Collections.Generic;
using Skill;
using Status;
using UnityEditor;
using UnityEngine;
using Util;

namespace Tower
{
    [RequireComponent(typeof(TowerPointData))]
    public class TowerControl : Singleton<TowerControl>
    {
        public TowerStatus status;
        public TowerPointData pointData;
        public SkillManager skillManager;

        protected override void Initialize()
        {
            base.Initialize();
            IsDontDestroy = false;
        }

        public void Reset()
        {
            status = GetComponent<TowerStatus>();
            pointData = GetComponent<TowerPointData>();
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public override void Awake()
        {
            base.Awake();
            status.onLevelUpEvent.AddListener(() => skillManager.AddRandomSkill());
        }

        public void Start()
        {
            status.AddEXP(status.experience.Max);
        }
    }
}
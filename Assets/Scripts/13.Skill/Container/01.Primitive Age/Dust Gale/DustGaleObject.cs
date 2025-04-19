using System;
using System.Collections.Generic;
using Unit.Monster;
using UnityEngine;
using Util;

namespace Skill
{
    public class DustGaleObject : MonoBehaviour
    {
        private new SphereCollider collider;
        [HideInInspector] public SkillDustGale skill;

        private float moveSpeedDown = 0;
        private float moreDamageMultiple = 0;
        private HashSet<MonsterControl> insideMonster = new();

        public void Awake()
        {
            collider = GetComponent<SphereCollider>();
        }

        public void Start()
        {
            moveSpeedDown = skill.status.moveSpeedDown;
            moreDamageMultiple = skill.status.dustMoreDamageMultiple;
            collider.radius = skill.status.dustRadius;
        }

        public void OnDestroy()
        {
            foreach (var monster in insideMonster)
            {
                monster.status.speedMultiple += moveSpeedDown;
                monster.status.moreDamageMultiple -= moreDamageMultiple;
            }
            insideMonster.Clear();
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out MonsterControl monster) && !insideMonster.Contains(monster))
            {
                insideMonster.Add(monster);
                monster.status.speedMultiple -= moveSpeedDown;
                monster.status.moreDamageMultiple += moreDamageMultiple;
                
                if (skill.status.stunProbability.IsProbability())
                    monster.status.Stun(skill.status.stunDuration);
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out MonsterControl monster))
            {
                insideMonster.Remove(monster);
                monster.status.speedMultiple += moveSpeedDown;
                monster.status.moreDamageMultiple -= moreDamageMultiple;
                if (skill.status.stunProbability.IsProbability())
                    monster.status.Stun(skill.status.stunDuration);
            }
        }
    }
}
using System.Collections;
using ProjectTile;
using UnityEngine;

namespace Skill
{
    public class TelekineticSlamDebris : ProjectileBase
    {
        // 애니메이션이 끝난 후
        // TargetAroundMove를 TargetMove로 변경
        // Target이 없으면 자동으로 -Vector3.up 방향으로 가게끔 하기

        private TargetMove targetMove;
        private TargetAroundMove targetAroundMove;

        public override void Awake()
        {
            base.Awake();
            targetMove = new(this);
            targetAroundMove =new TargetAroundMove(this)
            {
                rotateAxis = Vector3.up,
                rotateSpeed = 0,
                radius = 5,
            };

            Move = targetAroundMove;
        }

        public IEnumerator ChangedMoveCoroutine()
        {
            yield return new WaitForSeconds(1f);
            Move = targetMove;
            if(targetStatus.Hp.IsMin) targetMove.SetDirection(-Vector3.up);
        }
    }
}


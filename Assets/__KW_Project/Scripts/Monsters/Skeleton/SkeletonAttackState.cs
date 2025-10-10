using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace KW
{
    public class SkeletonAttackState : MonsterBaseState<SkeletonController>
    {
        public override void EnterState(SkeletonController controller)
        {
            Debug.Log("½ºÄÌ·¹Åæ »óÅÂ ÁøÀÔ : Attack");
        }

        public override void ExitState(SkeletonController controller)
        {

        }

        public override void UpdateState(SkeletonController controller)
        {

        }
    }
}

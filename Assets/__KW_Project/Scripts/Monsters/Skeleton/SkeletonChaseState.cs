using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace KW
{
    public class SkeletonChaseState : MonsterBaseState<SkeletonMovement>
    {
        public override void EnterState(SkeletonMovement controller)
        {
            Debug.Log("½ºÄÌ·¹Åæ Chasing »óÅÂ ÁøÀÔ");
        }

        public override void ExitState(SkeletonMovement controller)
        {
        }

        public override void UpdateState(SkeletonMovement controller)
        {
        }
    }
}

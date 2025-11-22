using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KW
{
    public class SkeletonDeathState : MonsterBaseState<SkeletonController>
    {
        public override void EnterState(SkeletonController controller)
        {
            Debug.Log("스켈레톤 상태 진입 : Dead");
        }

        public override void UpdateState(SkeletonController controller)
        {
        }

        public override void ExitState(SkeletonController controller)
        {
        }

       
    }
}

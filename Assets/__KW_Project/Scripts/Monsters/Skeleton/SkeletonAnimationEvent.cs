using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KW
{
    public class SkeletonAnimationEvent : MonoBehaviour
    {
        public SkeletonController skeletonController;

        private void Awake()
        {
            skeletonController = GetComponentInParent<SkeletonController>();
        }

        public void PerformAttackLunge()
        {
            skeletonController.PerformAttackLunge();
        }

        public void EnableHitBox()
        {
            if(skeletonController.hitBox != null)
            {
                skeletonController.hitBox.SetActive(true);
            }
        }
        public void DisableHitBox()
        {
            if (skeletonController.hitBox != null)
            {
                skeletonController.hitBox.SetActive(false);
            }
        }
    }
}

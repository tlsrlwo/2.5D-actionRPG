using UnityEngine;

namespace KW
{
    public class SkeletonPersistence : MonoBehaviour
    {
        // 고유 식별자
        public string skeletonID;
  
        private void Start()
        {
            // 게임 시작 시 확인
            if (SaveManager.Instance != null)
            {
                // SaveManager 에 자신의 ID 가 존재하는 지 확인
                if (SaveManager.Instance.deadMonsterIDs.Contains(skeletonID))
                {
                    // 있다면 사라짐
                    gameObject.SetActive(false);
                    return;
                }
            }
        }

        //몬스터가 죽을 때 호출
        public void RecordDeath()
        {
            if (SaveManager.Instance != null)
            {
                // 현재 SaveManager의 리스트에 저장되어있지 않다면
                if (!SaveManager.Instance.deadMonsterIDs.Contains(skeletonID))
                {
                    // 본인의 ID 를 추가
                    SaveManager.Instance.deadMonsterIDs.Add(skeletonID);
                    Debug.Log($"[SkeletonPersistence] 사망 기록됨 : {skeletonID}");
                }
            }
        }
        
      /*   // ID 자동 생성용 (에디터 편의 기능)
        [ContextMenu("Generate Unique ID")]
        private void GenerateID()
        {
            skeletonID = System.Guid.NewGuid().ToString();
        } */
    }
}

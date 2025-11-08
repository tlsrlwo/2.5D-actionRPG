using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace KW
{
    public class TooltipManager : MonoBehaviour
    {
        [Header("UI 요소 (인스펙터 연결)")]       
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private RectTransform backgroundRect;

        [Header("설정")]
        // 슬롯 모서리와의 X축 여백
        [SerializeField] private float xPadding = 10f;
        // 슬롯 모서리와의 Y축 여백
        [SerializeField] private float yPadding = 0f;

        // 툴팁 자기 자신의 RectTransform
        private RectTransform tooltipRect;

        private void Awake()
        {
            // 이 게임오브젝트의 RectTransform 컴포넌트를 가져오기           
            TryGetComponent<RectTransform>(out tooltipRect);

            // 툴팁 UI의 기준점(Pivot)을 '왼쪽 상단(0, 1)'으로 고정  
            if (tooltipRect != null)
            {
                tooltipRect.pivot = new Vector2(0, 1);
            }
            else
            {
                Debug.LogWarning("툴팁을 위한 슬롯의 Rect 를 불러오지 못함");
            }

            // 툴팁이 마우스 이벤트를 '삼키지' 않도록 Raycast Target 끄기
            // (이건 나중에 문제가 생기면 추가해도 됩니다)

            gameObject.SetActive(false);                                        // 시작할 땐 툴팁을 숨김
        }
        
        // InventorySlotUI가 호출할 '공개' 함수
        public void ShowTooltip(Item itemToShow, RectTransform slotRect)
        {
            // 1. 텍스트 채우기
            // titleText의 text를 itemToShow.itemName으로 설정
            if (itemToShow != null)
            {
                titleText.text = itemToShow.itemName;
            }

            // 해당 아이템의 description 내용이 없으면
            if (string.IsNullOrEmpty(itemToShow.description))
            {
                descriptionText.gameObject.SetActive(false);
            }
            else
            {
                descriptionText.gameObject.SetActive(true);
                descriptionText.text = itemToShow.description;
            }


            // 2. 툴팁 배경 크기 자동 조절 (backgroundRect 에 VerticalLayoutGroup, Content Size Fitter 부착)
            LayoutRebuilder.ForceRebuildLayoutImmediate(backgroundRect);

            // 3. 슬롯의 '오른쪽 상단' 모서리 월드 좌표 가져오기
            Vector3[] slotCorners = new Vector3[4];                             // 슬롯의 모서리 좌표를 가져오는 배열 [모서리는 4개니까]
            slotRect.GetWorldCorners(slotCorners);                              // 슬롯의 모서리 좌표값을 slotCorner라는 배열에 저장
            Vector3 targetPos = slotCorners[2];                                 // 슬롯의 오른쪽 상단의 좌표값을 가져옴
                        
            targetPos.x += xPadding;                                            // 툴팁 위치를 (슬롯 오른쪽 상단 + 여백)으로 설정
            targetPos.y += yPadding;
            tooltipRect.position = targetPos;

            // 5. [고급] 화면 이탈 방지 로직 (일단 건너뛰기)

            // 6. 툴팁 켜기
            gameObject.SetActive(true);            
        }

        // InventorySlotUI가 호출
        public void HideTooltip()
        {
            // 툴팁 끄기
            gameObject.SetActive(false);
        }
    }
}
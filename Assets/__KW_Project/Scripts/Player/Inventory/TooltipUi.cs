using UnityEngine;
using UnityEngine.EventSystems; 

namespace KW
{    
    public class TooltipUi : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        [Header("변수")]
        [SerializeField] private Item _currentItem;                          // 현재 표시할 아이템
        [SerializeField] private TooltipManager _tooltipManager;
        [SerializeField] private RectTransform _rectTransform;               

        // --- 초기화 ---
        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        // InventoryUI가 이 슬롯을 생성할 때 호출해 줄 함수
        public void Initialize(TooltipManager manager)
        {
            _tooltipManager = manager; 
        }

        // InventoryUI의 UpdateUI가 호출할, 아이템 정보 업데이트 함수
        public void SetItem(Item item)
        {
            _currentItem = item;
        }

        // --- 이벤트 함수 (자동 호출) ---

        // 마우스가 슬롯에 '들어왔을 때' 자동 호출
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_tooltipManager != null && _currentItem != null)
            {
                _tooltipManager.ShowTooltip(_currentItem, _rectTransform);
            }
        }

        // 마우스가 슬롯에서 '나갔을 때' 자동 호출
        public void OnPointerExit(PointerEventData eventData)
        {          
            if(_tooltipManager != null)
            {
                _tooltipManager.HideTooltip();
            }
        }

        // 마우스로 슬롯을 '클릭(Down)'했을 때 자동 호출
        public void OnPointerDown(PointerEventData eventData)
        {
            // 드래그를 시작할 때 툴팁 숨기기
            if (_tooltipManager != null)
            {
                _tooltipManager.HideTooltip();
            }
        }
    }
}
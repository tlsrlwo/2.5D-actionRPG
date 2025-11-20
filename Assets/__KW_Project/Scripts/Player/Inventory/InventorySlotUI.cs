using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

namespace KW
{
    public class InventorySlotUI : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {       
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI stackText;

        private InventoryUI _inventoryUI;

        private Item _currentItem;


        public void Initialize(InventoryUI uiManager)
        {
            this._inventoryUI = uiManager;
        }

        // Slot 에 데이터 채우기
        public void SetSlotData(InventorySlot slotData)
        {
            _currentItem = slotData.item;

            iconImage.sprite = slotData.item.itemSprite;                // Item의 sprite
            iconImage.enabled = true;

            // 겹칠 수 있는 아이템일 경우에만 수량 텍스트를 표시
            if (slotData.stack > 1)
            {
                stackText.text = slotData.stack.ToString();
                stackText.enabled = true;
            }
            else
            {
                stackText.enabled = false;
            }
        }

        // Slot에 데이터 비우기 (InventoryUI 에서 호출)
        public void ClearSlot()
        {
            _currentItem = null;

            iconImage.sprite = null;
            iconImage.enabled = false;
            stackText.enabled = false;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_currentItem == null) return;

            // 인벤토리UI 에 알림
            if (_inventoryUI != null)
            {
                _inventoryUI.BeginDrag(_currentItem);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_currentItem == null) return;

            // 매니저에게 현재 마우스 위치를 전달해서 '유령 아이콘'이 따라오게 함
            if (_inventoryUI != null)
            {
                _inventoryUI.OnDrag(eventData.position);
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            // 매니저에게 드래그 끝났음을 알림 (유령 아이콘 끄기)
            if (_inventoryUI != null)
            {
                _inventoryUI.EndDrag();
            }
        }
    }
}

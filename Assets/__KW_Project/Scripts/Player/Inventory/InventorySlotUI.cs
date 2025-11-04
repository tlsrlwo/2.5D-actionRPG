using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace KW
{
    public class InventorySlotUI : MonoBehaviour
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI stackText;

        // Slot 에 데이터 채우기
        public void SetSlotData(InventorySlot slotData)
        {
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
            iconImage.sprite = null;
            iconImage.enabled = false;
            stackText.enabled = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KW
{
    public class NotificationManager : MonoBehaviour
    {
        [Header("참조")]
        [SerializeField] private GameObject itemLootPopupPrefab;                // 팝업 창 프리팹

        [SerializeField] private GameObject itemLootLinePrefab;                 // 아이템 1줄 프리팹

        [SerializeField] private Transform popupHolder;                         // 팝업이 생성될 위치

        public void ShowLootPopup(List<ChestSlot> items)
        {
            Time.timeScale = 0.00001f;

            // 팝업 창 생성
            GameObject popupObj = Instantiate(itemLootPopupPrefab, popupHolder);

            if (popupObj != null) Debug.Log("팝업 생성됨");
            else Debug.LogWarning("팝업 생성 안됨");

            // 팝업 창 안에서 아이템 1줄이 생성될 위치 찾기
            Transform lineHolder = popupObj.transform.Find("LineHolder");

            Button confirmBtn = popupObj.transform.Find("ConfirmBtn").GetComponent<Button>();

            foreach (ChestSlot slot in items)
            {
                // 아이템 1 줄 프리팹을 LineHolder의 자식으로 생성함
                GameObject lineObj = Instantiate(itemLootLinePrefab, lineHolder);

                Image itemIcon = lineObj.transform.Find("ItemSprite").GetComponent<Image>();
                TextMeshProUGUI text = lineObj.transform.Find("ItemName").GetComponent<TextMeshProUGUI>();

                if (itemIcon != null)
                    itemIcon.sprite = slot.item.itemSprite;                     // Item 의 itemSprite

                if (text != null)
                    text.text = $"{slot.item.itemName}이(가) x{slot.quantity}개 추가됐다.";
            }

            if (confirmBtn != null)
            {
                confirmBtn.onClick.AddListener(() =>
                {
                    Time.timeScale = 1f;
                    Destroy(popupObj);
                });
            }
        }
    }
}

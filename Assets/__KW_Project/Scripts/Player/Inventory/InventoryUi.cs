using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KW
{
    public class InventoryUI : MonoBehaviour
    {
        [Header("참조")]
        [SerializeField] private Inventory playerInventory;
        [SerializeField] private GameObject player;
        [SerializeField] private GameObject slotPrefab;
        [SerializeField] private Transform slotParent;           // 슬롯prefab이 생성될 부모

        private List<InventorySlotUI> uiSlots = new List<InventorySlotUI>();        // slot Ui 를 관리할 리스트

        private void Start()
        {
            playerInventory = player.GetComponent<Inventory>();

            if (playerInventory == null || slotPrefab == null || slotParent == null)
            {
                Debug.LogError("InventoryUI : 필요한 참조가 설정되지 않았습니다");
                return;
            }

            // 슬롯 UI 들을 미리 생성
            InitializeSlots();

            // Inventory의 OnInventoryChanged 를 구독
            playerInventory.OnInventoryChanged += UpdateUI;

            // UI 최초 업데이트
            UpdateUI();
        }

        private void OnDestroy()
        {
            playerInventory.OnInventoryChanged -= UpdateUI;
        }

        // 인벤토리에 maxSlots 만큼 미리 slots 생성
        private void InitializeSlots()
        {
            // 기존에 생성된 게 있다면 삭제 (초기화)
            foreach (Transform child in slotParent)
            {
                Destroy(child.gameObject);
            }
            uiSlots.Clear();

            // maxSlot 만큼 slot 을 생성해줌
            for (int i = 0; i < playerInventory.maxSlots; i++)
            {
                // slotPrefab 을 slotParent 의 자식으로 생성
                GameObject newSlot = Instantiate(slotPrefab, slotParent);

                // 생성된 slot 에 InventorySlotUI 를 가져와서 List에 추가
                InventorySlotUI slotUi = newSlot.GetComponent<InventorySlotUI>();
                uiSlots.Add(slotUi);
            }
        }

        // 이벤트에서 구독 할 함수
        private void UpdateUI()
        {
            Debug.Log("InventoryUI : UI 업데이트");

            // 현재 생성된 uiSlots 의 개수 만큼 반복
            for (int i = 0; i < uiSlots.Count; i++)
            {
                // 실제 데이터에도 i번째 데이터가 있는지 확인
                if (i < playerInventory.slots.Count)
                {
                    // 데이터가 있으면
                    uiSlots[i].SetSlotData(playerInventory.slots[i]);
                }
                else
                {
                    // 데이터가 없으면 (빈 슬롯이면)
                    uiSlots[i].ClearSlot();
                }
            }
        }

    }
}

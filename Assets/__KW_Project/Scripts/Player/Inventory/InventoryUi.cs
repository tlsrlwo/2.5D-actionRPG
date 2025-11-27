using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KW
{
    public class InventoryUI : MonoBehaviour, IDropHandler
    {
        [Header("참조")]
        [SerializeField] private Inventory playerInventory;
        public Inventory PlayerInventory => playerInventory;

        [SerializeField] private GameObject player;
        [SerializeField] private GameObject slotPrefab;
        [SerializeField] private Transform slotParent;                              // 슬롯prefab이 생성될 부모
        [SerializeField] private TooltipManager tooltipManager;

        [Header("드래그 앤 드롭")]
        [SerializeField] private Image dragIconImage;
                
        public Item currentDragItem { get; private set; }                           // 현재 드래그 중인 아이템
        public QuickSlot_Ui dragSourceSlot {get; private set; }

        private List<InventorySlotUI> uiSlots = new List<InventorySlotUI>();        // slot Ui 를 관리할 리스트

        private void Start()
        {
            playerInventory = player.GetComponent<Inventory>();

            if (playerInventory == null || slotPrefab == null || slotParent == null || tooltipManager == null)
            {
                Debug.LogError("InventoryUI : 필요한 참조가 설정되지 않았습니다");
                return;
            }

            if (dragIconImage != null) dragIconImage.gameObject.SetActive(false);

            // 슬롯 UI 들을 미리 생성
            InitializeSlots();

            // Inventory의 OnInventoryChanged 를 구독
            playerInventory.OnInventoryChanged += UpdateUI;

            // UI 최초 업데이트
            UpdateUI();
        }

        private void OnDestroy()
        {
            if (playerInventory != null)
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

                // 생성된 slot 에서 TooltipUi 를 가져옴
                TooltipUi tooltipUi = newSlot.GetComponent<TooltipUi>();

                // 가져온 tooltipUi 에 tooltipManager 참조를 전달해줌
                if(tooltipUi != null)
                {
                    tooltipUi.Initialize(tooltipManager);
                }

                if(slotUi != null)
                {
                    slotUi.Initialize(this);
                }

                uiSlots.Add(slotUi);
            }
        }
        // InventorySlotUi 에서 참조됨 -----------------------
        public void BeginDrag(Item item, QuickSlot_Ui sourceSlot = null)
        {
            if (item == null) return;

            currentDragItem = item;
            dragSourceSlot = sourceSlot;            // 출처 저장

            // Ghost 세팅
            if (dragIconImage != null)
            {
                dragIconImage.sprite = currentDragItem.itemSprite;
                dragIconImage.color = new Color(1, 1, 1, 0.8f);
                dragIconImage.gameObject.SetActive(true);
            }
        }

        public void OnDrag(Vector2 mousePos)
        {
            if (currentDragItem != null)
            {
                // 아이콘이 마우스를 따라다니게
                dragIconImage.transform.position = mousePos;
            }
        }

        public void EndDrag()
        {
            currentDragItem = null;
            dragSourceSlot = null;

            if (dragIconImage != null) dragIconImage.gameObject.SetActive(false);
        }

        
        public void OnDrop(PointerEventData eventData)
        {
            // 드래그 중인 아이템이 있고, 출처가 퀵슬롯
            if(currentDragItem != null  && dragSourceSlot != null)
            {
                // 인벤토리에 아이템 추가
                playerInventory.AddItem(currentDragItem);

                // 퀵슬롯에서는 아이템 제거
                dragSourceSlot.ClearSlot();

                EndDrag();
            }
        }
        //------------------------------------------------

        // 이벤트에서 구독 할 함수
        private void UpdateUI()
        {
            // Debug.Log("InventoryUI : UI 업데이트");

            // 현재 생성된 uiSlots 의 개수 만큼 반복
            for (int i = 0; i < uiSlots.Count; i++)
            {
                // 현재 슬롯[i] 에서 tooltipUi 컴포넌트를 가져옴
                TooltipUi tooltipUi = uiSlots[i].GetComponent<TooltipUi>();

                // 실제 데이터에도 i번째 데이터가 있는지 확인
                if (i < playerInventory.slots.Count)
                {
                    // 데이터가 있으면
                    uiSlots[i].SetSlotData(playerInventory.slots[i]);

                    // tooltipUi 컴포넌트가 존재한다면
                    if (tooltipUi != null)
                    {
                        // tooltipUi 에게 현재 아이템의 정보를 전달
                        tooltipUi.SetItem(playerInventory.slots[i].item);
                    }
                }
                else
                {
                    // 데이터가 없으면 (빈 슬롯이면)
                    uiSlots[i].ClearSlot();

                    if(tooltipUi!= null)
                    {
                        tooltipUi.SetItem(null);
                    }
                }
            }
        }
    }
}

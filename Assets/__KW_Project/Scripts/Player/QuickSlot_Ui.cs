using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KW
{
    public class QuickSlot_Ui : MonoBehaviour, IDropHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private ItemType _acceptedItemType;

        [SerializeField] private Image _itemSprite;

        [SerializeField] private PlayerHealth _playerHealth;

        private InventoryUI _inventoryUI;

        [SerializeField] private Item _equippedItem;

        public Item equippedItem => _equippedItem;

        public void OnEnable()
        {
            if (_inventoryUI == null)
            {
                _inventoryUI = GetComponentInParent<InventoryUI>();

                if (_inventoryUI == null)
                {
                    _inventoryUI = FindObjectOfType<InventoryUI>();
                }
            }
            if (_playerHealth == null)
            {
                GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
                if (playerObj != null)
                {
                    _playerHealth = playerObj.GetComponent<PlayerHealth>();
                }
            }
        }
        private void Awake()
        {
            // _itemSprite = GetComponentInChildren<Image>(); 직접 할당해줌
            ClearSlot();
        }

        // 아이템 장착 시도
        public bool EquipItem(Item newItem)
        {
            if (newItem == null) return false;

            if (newItem.itemType != _acceptedItemType)
            {
                Debug.Log($"장착 실패 : 이 슬롯은 {_acceptedItemType} 만 착용 가능합니다!");

                // 추후에 장착 실패 UI 만들기

                return false;
            }

            _equippedItem = newItem;

            // 아이템 UI 갱신
            _itemSprite.sprite = newItem.itemSprite;
            _itemSprite.enabled = true;

            // 무기 수치 저장 로직
            if (_acceptedItemType == ItemType.Weapon && newItem is Weapon weapondata)
            {
                if (_playerHealth != null)
                {
                    _playerHealth.SetEquippedWeapon(weapondata);
                }
            }

            // 방어구 수치 저장 로직

            return true;
        }

        public void ClearSlot()
        {
            if (_acceptedItemType == ItemType.Weapon && _equippedItem != null)
            {
                if (_playerHealth != null)
                {
                    _playerHealth.SetEquippedWeapon(null);
                }
            }

            // 데이터 비우기
            _equippedItem = null;

            // Ui 비우기
            _itemSprite.sprite = null;
            _itemSprite.enabled = false;
        }

        // 마우스 Drag 관련-----------------------------------------------------------------------      
        public void OnBeginDrag(PointerEventData eventData)
        {
            if(_equippedItem == null) return;
            if(_inventoryUI == null) return;

            // InventoryUI 에 아이템의 정보와, 자기 자신에서 Drag 를 시작했다고 알림
            _inventoryUI.BeginDrag(_equippedItem, this);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if(_equippedItem == null) return;
            if(_inventoryUI != null) _inventoryUI.OnDrag(eventData.position);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if(_equippedItem == null) return;
            if(_inventoryUI != null) _inventoryUI.EndDrag();
        }

          public void OnDrop(PointerEventData eventData)
        {
            if (_inventoryUI == null) return;

            Item droppedItem = _inventoryUI.currentDragItem;
            QuickSlot_Ui sourceSlot = _inventoryUI.dragSourceSlot;

            // 유효한 아이템이고, 인벤토리(퀵슬롯 x) 에서 온 경우
            if (droppedItem != null && sourceSlot == null)
            {               
                // 타입 체크 (EquipItem 함수 안에서 체크하지만 미리 해도 됨)
                if(droppedItem.itemType != _acceptedItemType) return;
               
                // [교체 로직] 만약 이미 장착중인 아이템이 있다면?
                if(equippedItem != null)
                {
                    // 기존 아이템을 인벤토리로 돌려보냄 (Swap)
                    _inventoryUI.PlayerInventory.AddItem(_equippedItem);
                }

                // 새 아이템 장착
                if (EquipItem(droppedItem))
                {
                    //[핵심] 장착 성공 시 인벤토리에서 해당 아이템 제거
                    _inventoryUI.PlayerInventory.RemoveItem(droppedItem);
                }
            }
        }
        //--------------------------------------------------------------------------------
    }
}

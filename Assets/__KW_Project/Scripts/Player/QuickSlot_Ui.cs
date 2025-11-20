using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KW
{
    public class QuickSlot_Ui : MonoBehaviour, IDropHandler
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

        public void OnDrop(PointerEventData eventData)
        {
            if (_inventoryUI == null) return;

            Item droppedItem = _inventoryUI.currentDragItem;

            if (droppedItem != null)
            {
                if (EquipItem(droppedItem))
                {
                    Debug.Log("퀵슬롯 장착 성공");
                }
            }
        }
    }
}

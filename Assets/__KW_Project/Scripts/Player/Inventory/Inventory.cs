using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;                                                           // 이벤트 Action 을 위해 필요함


namespace KW
{    
    public class Inventory : MonoBehaviour
    {
        public event Action OnInventoryChanged;
        public event Action<Item, int> OnItemAdded;                     // 팝업 Ui를 위함

        public List<InventorySlot> slots = new List<InventorySlot>();

        public int maxSlots = 50;                                       // 가방 최대 칸 수

        // 아이템이 한개일때만 AddItem(itemToAdd, amountToAdd) 호출
        public bool AddItem(Item itemToAdd)
        {
            return AddItem(itemToAdd, 1);
        }

        public bool AddItem(Item itemToAdd, int amountToAdd)
        {
            int totalAddedItem = 0;                                     // 추가 완료된 아이템의 갯수 (Ui 창에서 숫자를 위함)
            int amountRemaining = amountToAdd;                          // 추가해야될 아이템 수 = 추가할 아이템 수

            // 포션 처럼 1개 이상 겹칠 수 있는 지 아이템인 경우
            if (itemToAdd.maxStack > 1)
            {
                foreach (InventorySlot slot in slots)                   // 각 인벤토리 칸에서 같은 아이템을 찾음
                {
                    if (slot.item == itemToAdd && slot.stack < slot.item.maxStack)  // maxStack 보다 적게 있을 시
                    {
                        int spaceAvailable = slot.item.maxStack - slot.stack;
                        int amountToFill = Mathf.Min(amountRemaining, spaceAvailable);

                        slot.stack += amountToFill;
                        amountRemaining -= amountToFill;
                        totalAddedItem += amountToFill;

                        if (amountRemaining <= 0) break;                // 추가할 아이템 수 가 다 채워지면 멈춤
                    }
                }

                // 추가해야될 아이템이 남아있을 경우 (1개 이상)
                while (amountRemaining > 0)
                {
                    // 인벤토리 칸이 남아있을 경우
                    if (slots.Count < maxSlots)
                    {
                        int amountForNewSlot = Mathf.Min(amountRemaining, itemToAdd.maxStack);

                        InventorySlot newSlot = new InventorySlot(itemToAdd, amountForNewSlot);
                        slots.Add(newSlot);
                        amountRemaining -= amountForNewSlot;
                        totalAddedItem += amountForNewSlot;
                    }
                    // 인벤토리 꽉 참 ( 추후에 인벤토리에 자리가 없음 팝업Ui 로직 추가 )
                    else
                    {
                        break;
                    }
                }

            }

            else        // 스택 불가능한 아이템 (무기, 방어구)
            {
                // 겹칠 수 없는 아이템은 요청한 수량(amountToAdd)만큼 1개씩 추가
                for (int i = 0; i < amountToAdd; i++)
                {
                    // 겹칠 수 없는 아이템인 경우
                    if (slots.Count < maxSlots)                         // 인벤토리의 여유 공간 확인
                    {
                        InventorySlot newSlot = new InventorySlot(itemToAdd, 1);
                        slots.Add(newSlot);
                        amountRemaining--;
                        totalAddedItem++;
                    }
                    else
                    {
                        break;                                          // 인벤토리 꽉참
                    }
                }
            }
            if(totalAddedItem > 0)
            {
                Debug.Log($"{itemToAdd} 가 {totalAddedItem}개 추가됨");

                OnItemAdded?.Invoke(itemToAdd, totalAddedItem);         // 팝업 Ui 에서 사용

                OnInventoryChanged?.Invoke();                           // 인벤토리 Ui 에서 사용
            }
          
            if(amountRemaining <= 0)
            {
                return true;                                            // 위의 로직이 다 성공됨
            }
            else
            {
                Debug.LogWarning($"인벤토리 공간 부족. {itemToAdd} {amountRemaining} 개를 추가하지 못함.");
                return false;
            }
        }
    }
}



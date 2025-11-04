using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;               // 이벤트 Action 을 위해 필요함

namespace KW
{
    public class Inventory : MonoBehaviour
    {
        public event Action OnInventoryChanged; 

        public List<InventorySlot> slots = new List<InventorySlot>();
        public int maxSlots = 50;               // 가방 최대 칸 수

        public bool AddItem(Item itemToAdd)
        {            
            // 포션 처럼 1개 이상 겹칠 수 있는 지 아이템인 경우
            if (itemToAdd.maxStack > 1)
            {
                foreach (InventorySlot slot in slots)     // 각 인벤토리 칸에서 같은 아이템을 찾음
                {
                    if (slot.item == itemToAdd && slot.stack < slot.item.maxStack)  // maxStack 보다 적게 있을 시
                    {
                        slot.stack++;
                        Debug.Log($"{itemToAdd.itemName} 스택 추가. 현재 : {slot.stack} 개");

                        OnInventoryChanged?.Invoke();           // 이벤트 호출

                        return true;
                    }
                }
            }

            // 겹칠 수 없는 아이템인 경우
            if(slots.Count < maxSlots) // 인벤토리의 여유 공간 확인
            {
                InventorySlot newSlot = new InventorySlot(itemToAdd, 1);
                slots.Add(newSlot);

                Debug.Log($"{itemToAdd.itemName} 1개 추가");

                OnInventoryChanged?.Invoke();

                return true;
            }

            // 인벤토리가 꽉 차서 더 안 들어갈 때
            Debug.Log("인벤토리에 여유 공간 없음");
            return false;
        }
    }
}

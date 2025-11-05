using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KW
{
    public class Chest : MonoBehaviour, IInteractable
    {
        [Header("파밍")]
        public List<ChestSlot> itemsInChest = new List<ChestSlot>();

        private bool isOpen = false;                                                    // 상자가 열려있는지

        public void Interact(GameObject player)
        {
            if (isOpen) return;

            Debug.Log("상자를 엽니다");

            // 플레이어에서 <Inventory>() 찾기
            Inventory playerInventory = player.GetComponent<Inventory>();

            if(playerInventory != null)
            {               
                List<ChestSlot> itemSuccessfullyAdded = new List<ChestSlot>();          // 성공적으로 가져간 아이템을 담는 임시 리스트

                foreach (ChestSlot chestSlot in itemsInChest)
                {
                    bool success = playerInventory.AddItem(chestSlot.item, chestSlot.quantity);

                    if (success)
                    {
                        // 아이템 추가 성공
                        itemSuccessfullyAdded.Add(chestSlot);
                    }
                    else
                    {
                        // 인벤토리가 꽉 찼을 경우
                        Debug.LogWarning($"{chestSlot.item.itemName}을(를) 추가하지 못했습니다. (공간 부족)");
                    }
                }

                // 성공적으로 추가된 아이템들만 상자 리스트에서 제거함
                foreach (ChestSlot addedSlot in itemSuccessfullyAdded)
                {
                    itemsInChest.Remove(addedSlot);
                }

                if (itemsInChest.Count == 0)
                {
                    isOpen = true;
                    // TODO: 상자 열리는 애니메이션/사운드 재생
                }
            }
        }
    }
}

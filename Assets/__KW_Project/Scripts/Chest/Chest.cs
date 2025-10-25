using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KW
{
    public class Chest : MonoBehaviour, IInteractable
    {
        [Header("파밍")]
        public List<Item> itemsInChest = new List<Item>();

        private bool isOpen = false;                    // 상자가 열려있는지

        public void Interact(GameObject player)
        {
            if (isOpen) return;

            Debug.Log("상자를 엽니다");

            // 플레이어에서 <Inventory>() 찾기
            Inventory playerInventory = player.GetComponent<Inventory>();

            if(playerInventory != null)
            {
                foreach(Item item in itemsInChest)
                {
                    // 아이템을 추가
                    playerInventory.AddItem(item);
                }
                itemsInChest.Clear();       // 상자 비우기
            }
        }
    }
}

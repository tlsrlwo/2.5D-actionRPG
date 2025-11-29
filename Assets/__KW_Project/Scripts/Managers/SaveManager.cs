using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Unity.VisualScripting;



#if UNITY_EDITOR
using UnityEditor;
#endif

namespace KW
{
    public class SaveManager : MonoBehaviour
    {
        public static SaveManager Instance { get; private set; }

        [Header("참조")]
        public PlayerMovement player;
        public PlayerHealth playerHealth;
        public Inventory inventory;
        public QuestManager questManager;

        // 퀵슬롯 UI 들의 참조
        public QuickSlot_Ui weaponSlot;
        public QuickSlot_Ui armourSlot;
        public QuickSlot_Ui potionSlot;

        private string savePath;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }                

            savePath = Application.persistentDataPath + "/saveGame.json";                   // 저장될 주소 
        }

        public void SaveGame()
        {
            SaveData data = new SaveData();

            // 플레이어 정보 저장
            data.playerPos = player.transform.position;
            data.currentHp = playerHealth.currentHp;

            // 인벤토리 저장
            foreach (var slot in inventory.slots)
            {
                if (slot.item != null)
                {
                    // 아이템의 정보를 불러옴
                    ItemSaveData itemData = new ItemSaveData
                    {
                        itemId = slot.item.itemId,
                        amount = slot.stack,
                    };
                    // SaveData.cs 의 inventoryItems List 에 추가
                    data.inventoryItems.Add(itemData);
                }
            }

            // 퀵슬롯 저장
            SaveQuickSlot(weaponSlot, data.quickSlotItems);
            SaveQuickSlot(armourSlot, data.quickSlotItems);
            // SaveQuickSlot(potionSlot, data.quickSlotItems);

            // 퀘스트 저장
            data.completedQuestNames = new List<string>(questManager.completedQuests);
            foreach (var q in questManager.activeQuests)
            {
                QuestSaveData qData = new QuestSaveData
                {
                    questName = q.data.questTitle,                  // 고유한 식별자여야 함, 그러려면 QuestSO에 퀘스트Id 를 추가하는게 좋지 않나?
                    currentCount = q.currentCount
                };
                data.activeQuests.Add(qData);
            }

            // 파일 쓰기
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(savePath, json);
            Debug.Log("게임저장됨 : " + savePath);
        }
        
        private void SaveQuickSlot(QuickSlot_Ui slot, List<ItemSaveData> list)
        {
            if (slot.equippedItem != null)
            {
                list.Add(new ItemSaveData { itemId = slot.equippedItem.itemId, amount = 1 });
            }
            else
            {
                list.Add(new ItemSaveData { itemId = "", amount = 0 });
            }
        }
        

        public void LoadGame()
        {
            if (!File.Exists(savePath))
            {
                Debug.LogError("저장된 파일이 없습니다");
                return;
            }

            string json = File.ReadAllText(savePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            // 플레이어 복구
            // CharacterController 가 있으면 transform.position 직접 수정이 안 먹힐 수 있어서 꺼야함
            player.cController.enabled = false;
            player.transform.position = data.playerPos;
            player.cController.enabled = true;

            // 체력 복구 
            playerHealth.SetHealth(data.currentHp);;

            // 인벤토리 복구
            inventory.slots.Clear();

            foreach (var itemData in data.inventoryItems)
            {
                Item item = ItemDataBase.Instance.GetItemId(itemData.itemId);
                if (item != null)
                {
                    inventory.AddItem(item, itemData.amount);
                }
            }

            // 퀵슬롯 복구
            LoadQuickSlot(weaponSlot, data.quickSlotItems[0]);
            LoadQuickSlot(armourSlot, data.quickSlotItems[1]);
            // LoadQuickSlot(potionSlot, data.quickSlotItems[2]);

            questManager.activeQuests.Clear();
            questManager.completedQuests = data.completedQuestNames;

            foreach (var qData in data.activeQuests)
            {
                QuestSO so = Resources.Load<QuestSO>("Quests/" + qData.questName);          // Resources 안에 Quests 안에 questName 을 기준으로 찾음

                if (so != null)
                {
                    Quest restoredQuest = new Quest(so);
                    restoredQuest.currentCount = qData.currentCount;

                    questManager.activeQuests.Add(restoredQuest);
                }
            }

            // UI 갱신
            if (inventory != null)
            {
                inventory.ForceUpdateUI();
            }
            
            if (questManager != null)
            {
                questManager.ForceUpdateUI();
            }

            Debug.Log("Game Data Loaded");
        }
        public void DisplayBtn()
        {
            Debug.Log("Display Button is Pressed");
        }
        public void SoundBtn()
        {
            Debug.Log("Sound Button is Pressed");
        }

        // 퀵슬롯 아이템 로드하기
        private void LoadQuickSlot(QuickSlot_Ui slot, ItemSaveData data)
        {
            slot.ClearSlot();

            if (!string.IsNullOrEmpty(data.itemId))
            {
                Item item = ItemDataBase.Instance.GetItemId(data.itemId);
                if (item != null) slot.EquipItem(item);
            }
        }
        
        public void ExitBtn()
        {
            Debug.Log("Exit Button is Pressed");

            // --- 게임 종료 로직 ---

            // 1. 실제 빌드된 게임(PC, Mac 등)에서 종료할 때 사용
            Application.Quit();

            // 2. 유니티 에디터에서 플레이 모드를 중지할 때 사용
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#endif
        }
    }
}

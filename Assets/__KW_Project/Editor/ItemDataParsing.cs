using UnityEngine;
using UnityEditor;                  // 에디터 스크립트 필수
using System.IO;                    // CSV 파일(File) 읽기 필수
using System.Collections.Generic;   // 딕셔너리(Dictionary) 사용 필수
using System;
using Codice.Client.BaseCommands.Merge.Xml;                       // Enum, TryParse 등 예외 처리 필수

namespace KW
{
    public class ItemDataParsing 
    {
        // 데이터 csv 파일 경로 
        private const string CSV_PATH = "Assets/__KW_Project/Data/ItemData.csv";

        // 파싱 버튼 생성
        [MenuItem("DuskBorn/Parse Item Data")]
        public static void ParseItemData()
        {
            Dictionary<string, Item> itemDataBase = LoadAllItemSO();

            // 오류 방지
            if(itemDataBase.Count == 0)
            {
                Debug.LogWarning("[ItemDataParsing] 파싱할 아이템 Scriptable Object 에셋을 프로젝트에서 찾을 수 없음");

                return;
            }
            if (!File.Exists(CSV_PATH))
            {
                Debug.LogError($"[ItemDataParsing] CSV 파일을 찾을 수 없음 {CSV_PATH}");
                return;
            }

            // CSV 파일의 모든 줄 읽어오기
            string[] allLines = File.ReadAllLines(CSV_PATH, System.Text.Encoding.UTF8);
            int updateCount = 0;

            Debug.Log($"[ItemDataParsing] CSV파일 읽기 완료. {allLines.Length - 1}개의 데이터 파싱을 시작합니다");

            // 데이터 파싱
            for (int i = 1; i < allLines.Length; i++)
            {
                string line = allLines[i];

                if (string.IsNullOrEmpty(line)) continue;       // 비어있는 줄 건너뛰기

                string[] columns = line.Split(',');             // 쉼표(,) 로 데이터 분리

                // A열 (ItemId)
                string itemId = columns[0].Trim();
                if(string.IsNullOrEmpty(itemId))
                {
                    Debug.LogWarning($"[ItemDataParsing] {i + 1}번째 줄의 ItemID 가 비어있음.");
                    continue;
                }

                // ItemId 로 딕셔너리에서 맞는 Scriptable Object 찾기
                if (itemDataBase.TryGetValue(itemId, out Item targetItem))
                {
                    try
                    {   
                        // 열(Column) 데이터 주입
                        // [B열] Category (Weapon, Equipment)
                        string category = columns[1].Trim();

                        // [C열] Type
                        string type = columns[2].Trim();

                        // [D열] Name
                        targetItem.itemName = columns[3].Trim();

                        // [E열] Grade
                        string grade = columns[4].Trim();

                        // 아이템 타입에 따라 세부 정보 주입
                        if (category == "Weapon" && targetItem is Weapon)
                        {
                            Weapon weapon = (Weapon)targetItem;                 // targetItem 에서 Weapon스크립트를 가져옴

                            // Type -> weaponType (Enum)
                            weapon.weaponType = ParseEnum<WeaponType>(type);

                            // Grade -> weaponGrade (Enum)
                            weapon.weaponGrade = ParseEnum<WeaponGrade>(grade);

                            // Damage -> damage (int)
                            weapon.damage = TryParseInt(columns[5]);
                        }
                        else if (category == "Equipment" && targetItem is Armour)
                        {
                            Armour armour = (Armour)targetItem;

                            // Type -> armourType (Enum)
                            armour.armourType = ParseEnum<ArmourType>(type);

                            // Grade -> armourGrade (Enum)
                            armour.armourGrade = ParseEnum<ArmourGrade>(grade);

                            // DefenceRate -> defenceRate (float)
                            armour.defenceRate = TryParseFloat(columns[7]);
                        }

                        // 아이템 타입 값 파싱
                        if(columns.Length > 8)
                        {
                            int typeNumber = TryParseInt(columns[8]);
                            targetItem.itemType = (ItemType)typeNumber;
                        }

                        EditorUtility.SetDirty(targetItem);
                        updateCount++;
                    }
                    catch(Exception exception)
                    {
                        Debug.LogError($"[ItemDataParsing] {i + 1}번째 줄 ({itemId}) 파싱 중 오류 발생 : {exception.Message}");
                    }
                }
                else
                {
                    Debug.LogWarning($"[ItemDataParsing] CSV의 ItemId '{itemId}' 에 해당하는 Scriptable Object 를 찾지 못함"); 
                }

            } // for문 끝 (데이터 파싱)
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"[ItemDataParsing] 파싱 완료! 총 {updateCount} 개의 Item ScriptableObject 가 업데이트됨");
        }

        // 모든 Scriptable Object 를 itemId 를 Key 로 하는 딕셔너리로 반환
        private static Dictionary<string, Item> LoadAllItemSO()
        {
            Dictionary<string, Item> itemDataBase = new Dictionary<string, Item>();

            // "t:Item" : 타입이 Item 이거나 Item을 상속받는 모든 에셋을 찾아달라는 검색어
            string[] guids = AssetDatabase.FindAssets("t:Item");

            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                Item item = AssetDatabase.LoadAssetAtPath<Item>(assetPath);

                // 이름이 itemId의 Scriptable Object 가 비어있지 않고, 딕셔너리에 중복되지 않았다면 추가. (ScriptableObject 있고 딕셔너리에 없으면 추가)
                if (item != null && !string.IsNullOrEmpty(item.itemId) && !itemDataBase.ContainsKey(item.itemId))
                {
                    itemDataBase.Add(item.itemId, item);
                }
            }
            Debug.Log($"[ItemDataParsing] {itemDataBase.Count}개의 Item Scriptable Object 에셋을 로드함. ItemId 가 입력된 에셋만");
            return itemDataBase;
        }


        #region 파싱을 안전히 도우기 위한 함수들
        // 문자열을 Enum 값으로 안전하게 변환 (Type, Grade 에 사용)
        private static T ParseEnum<T>(string value) where T : struct
        {
            if (string.IsNullOrEmpty(value)) return default(T);

            // 대소문자 무시하고 파싱 (true)
            if (Enum.TryParse<T>(value, true, out T result))
            {
                return result;
            }
            else
            {
                Debug.LogWarning($"[ItemDataParsing] ParseEnum() '{value}를 {typeof(T).Name} Enum으로 변환할 수 없습니다. 기본값으로 처리합니다.");
                return default(T);
            }
        }

        // 문자열을 int 로 반환 (damage 에 사용)
        private static int TryParseInt(string value)
        {
            if (int.TryParse(value, out int result)) return result;
            return 0;           // 실패 시 0 반환
        }

        private static float TryParseFloat(string value)
        {
            if (float.TryParse(value, out float result)) return result;
            return 0f;          // 실패 시 0.0 반환
        }
        #endregion
    }
}

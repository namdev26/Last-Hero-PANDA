//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace Inventory.Model
//{
//    [CreateAssetMenu]
//    public class BookItemSO : ItemSO, IDestroyableItem, IItemAction
//    {
//        [SerializeField]
//        private List<ModifierData> modifiersData = new List<ModifierData>();

//        public string ActionName => "Consume";

//        [field: SerializeField]
//        public AudioClip actionSFX { get; private set; }
//        public BookItemSO()
//        {
//            equipmentType = EquipmentType.Accessory; // Gán loại mặc định là Book
//        }
//        public bool PerformAction(GameObject character, List<ItemParameter> itemState)
//        {
//            foreach (var data in modifiersData)
//            {
//                data.statModifier.AffectCharacter(character, data.value);
//            }
//            return true;
//        }

//        public bool PerformUnequipAction(GameObject character)
//        {
//            throw new NotImplementedException();
//        }
//    }

//    public interface IDestroyableItem
//    {

//    }

//    [Serializable]
//    public class ModifierData
//    {
//        public PlayerStatsModifierSO statModifier;
//        public float value;
//    }
//}
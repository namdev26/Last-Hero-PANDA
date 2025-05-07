using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    public enum EquipmentType
    {
        Weapon,
        Helmet,
        Armor,
        Boots,
        Accessory,
        Cape,
        Quest
    }

    public abstract class ItemSO : ScriptableObject
    {
        [field: SerializeField]
        public bool IsStackable { get; set; }

        public int ID => GetInstanceID();

        [field: SerializeField]
        public int MaxStackSize { get; set; } = 1;

        [field: SerializeField]
        public string Name { get; set; }

        [field: SerializeField]
        [field: TextArea]
        public string Description { get; set; }

        [field: SerializeField]
        public Sprite ItemImage { get; set; }

        [field: SerializeField]
        public List<ItemParameter> DefaultParametersList { get; set; }


        [field: SerializeField]
        public int SetId { get; set; } // Thêm ID của bộ đồ (0 nếu không thuộc bộ nào)


        [field: SerializeField]
        public EquipmentType equipmentType { get; set; } // Thêm loại trang bị


    }
    [Serializable]
    public struct ItemParameter : IEquatable<ItemParameter>
    {
        public ItemParameterSO itemParameter;
        public int value;

        public bool Equals(ItemParameter other)
        {
            return other.itemParameter == itemParameter;
        }
    }

}
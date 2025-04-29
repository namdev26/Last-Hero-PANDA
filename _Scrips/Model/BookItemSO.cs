using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu]
    public class BookItemSO : ItemSO, IDestroyableItem, IItemAction
    {
        [SerializeField]
        private List<ModifierData> modifiersData = new List<ModifierData>();

        public string ActionName => "Consume";

        [field: SerializeField]
        public AudioClip actionSFX { get; private set; }

        public bool PerformAction(GameObject character, List<ItemParameter> itemState)
        {
            foreach (var data in modifiersData)
            {
                data.statModifier.AffectCharacter(character, data.value);
            }
            return true;
        }
    }

    public interface IDestroyableItem
    {

    }

    public interface IItemAction
    {
        public string ActionName { get; }
        public AudioClip actionSFX { get; }
        bool PerformAction(GameObject character, List<ItemParameter> itemState);
    }

    [Serializable]
    public class ModifierData
    {
        public PlayerStatsModifierSO statModifier;
        public float value;
    }
}
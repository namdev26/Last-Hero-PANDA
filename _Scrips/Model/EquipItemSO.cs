using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu]
    public class EquipItemSO : ItemSO, IDestroyableItem, IItemAction
    {
        public string ActionName => "Equip";

        [field: SerializeField]
        public AudioClip actionSFX { get; private set; }
        public EquipItemSO()
        {
            equipmentType = EquipmentType.Weapon; // Gán loại mặc định là Weapon
        }
        public bool PerformAction(GameObject character, List<ItemParameter> itemState = null)
        {
            AgentWeapon agentWeapon = character.GetComponent<AgentWeapon>();
            PlayerStats playerStats = character.GetComponent<PlayerStats>();

            if (agentWeapon != null)
            {
                // Gán vật phẩm cho AgentWeapon
                agentWeapon.SetWeapon(this, itemState == null ? DefaultParametersList : itemState);

                // Cộng chỉ số vào PlayerStats
                if (playerStats != null)
                {
                    List<ItemParameter> parameters = itemState ?? DefaultParametersList;
                    foreach (var param in parameters)
                    {
                        playerStats.AddStatBonus(param.itemParameter.ParameterName, param.value);
                    }
                }

                return true;
            }

            return false;
        }
        public bool PerformUnequipAction(GameObject character)
        {
            AgentWeapon agentWeapon = character.GetComponent<AgentWeapon>();
            if (agentWeapon != null)
            {
                agentWeapon.Unequip();
                return true;
            }
            return false;
        }
    }
}
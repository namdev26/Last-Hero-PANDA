using Inventory.Model;
using System.Collections.Generic;
using UnityEngine;

public class AgentWeapon : MonoBehaviour
{
    [SerializeField]
    private EquipItemSO weapon;

    [SerializeField]
    private InventorySO inventoryData;

    [SerializeField]
    private List<ItemParameter> parametersToModify, itemCurrentState;

    public void SetWeapon(EquipItemSO weaponItemSO, List<ItemParameter> itemState)
    {
        if (weapon != null)
        {
            PlayerStats playerStats = GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                foreach (var param in itemCurrentState)
                {
                    playerStats.RemoveStatBonus(param.itemParameter.ParameterName, param.value);
                }
            }
        }

        this.weapon = weaponItemSO;
        this.itemCurrentState = new List<ItemParameter>(itemState);
        ModifyParameters();
    }

    public void Unequip()
    {
        if (weapon != null)
        {
            PlayerStats playerStats = GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                foreach (var param in itemCurrentState)
                {
                    playerStats.RemoveStatBonus(param.itemParameter.ParameterName, param.value);
                }
            }

            weapon = null;
            itemCurrentState.Clear();
        }
    }

    public EquipItemSO GetCurrentWeapon()
    {
        return weapon;
    }

    private void ModifyParameters()
    {
        foreach (var parameter in parametersToModify)
        {
            if (itemCurrentState.Contains(parameter))
            {
                int index = itemCurrentState.IndexOf(parameter);
                int newValue = itemCurrentState[index].value + parameter.value;
                itemCurrentState[index] = new ItemParameter
                {
                    itemParameter = parameter.itemParameter,
                    value = newValue
                };
            }
        }
    }
}
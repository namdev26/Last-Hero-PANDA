using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/Weapon Data")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public RuntimeAnimatorController animatorController;
    public string[] comboTriggers;
}

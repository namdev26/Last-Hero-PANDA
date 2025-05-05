using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/Weapon Data")]
public class WeaponData : ScriptableObject
{
    public string weaponName;
    public RuntimeAnimatorController animatorController;
    public string[] comboTriggers;
    public AttackData[] comboAttacks; // Mảng chứa thông tin về từng đòn tấn công
}

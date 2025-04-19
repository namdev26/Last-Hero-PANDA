using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private WeaponData[] allWeapons; // Danh sách vũ khí có thể dùng
    private int currentWeaponIndex = 0;

    private ComboAttackController comboAttack;

    private void Start()
    {
        comboAttack = GetComponent<ComboAttackController>();
        EquipWeapon(currentWeaponIndex); // Trang bị vũ khí đầu tiên khi bắt đầu game
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchWeapon();
        }
    }

    private void SwitchWeapon()
    {
        currentWeaponIndex++;
        if (currentWeaponIndex >= allWeapons.Length)
            currentWeaponIndex = 0;

        EquipWeapon(currentWeaponIndex);
    }

    private void EquipWeapon(int index)
    {
        WeaponData weapon = allWeapons[index];
        comboAttack.SetWeapon(weapon);
        Debug.Log($"Switched to weapon: {weapon.weaponName}");
    }
}

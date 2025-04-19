using UnityEngine;
using System;

[Serializable]
public class PlayerStats : MonoBehaviour
{
    [SerializeField] public int baseMaxHealth = 100;
    [SerializeField] public int baseMaxMana = 100;
    [SerializeField] public int baseDamage = 20;
    [SerializeField] public int baseDefence = 20;


    //public int BaseMaxHealth => baseMaxHealth;
    //public int BaseMaxMana => baseMaxMana;
    //public int BaseDamage => baseDamage;
    //public int BaseDefence => baseDefence;

    public int Damage => baseDamage;
    public int Defence => baseDefence;
}

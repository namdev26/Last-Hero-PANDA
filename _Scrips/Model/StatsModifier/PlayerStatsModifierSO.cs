using UnityEngine;

public abstract class PlayerStatsModifierSO : ScriptableObject
{
    public abstract void AffectCharacter(GameObject character, float val);
}

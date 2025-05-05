using UnityEngine;

[System.Serializable]
public class AttackData
{
    public int damage = 10;
    public float knockbackForceX = 0f;   // đẩy lùi
    public float knockbackForceY = 0f;   // đẩy lùi
    public bool launchEnemy = false;    //hất tung
    public float attackRadius = 1f;     // vùng tấn công
    public Vector2 hitboxOffset = Vector2.zero; // vị trí tâm
    public float hitboxActiveTime = 0.2f;   // tồn tại hit box
    public float hitStunDuration = 0.1f;    // địch choáng
    //[Header("Effects")]
    //public GameObject hitEffect;
    //public AudioClip hitSound;
}
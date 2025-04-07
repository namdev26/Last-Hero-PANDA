using UnityEngine;

public class BossAttackController : MonoBehaviour
{
    public LayerMask playerLayers;
    // thong so tung don danh
    //BasicAttack
    public float basicAttackRange = 2.5f;
    public Transform basicAttackPoint;
    public int basicAttackDamage = 10;

    //ChainAttack
    public Vector2 chainAttackRange = new Vector2(11f, 1.5f);
    public int chainAttackDamage = 10;
    public Transform chainAttackPoint;

    //RangeAttack
    public float rangeAttackRange = 5f;
    public int rangeAttackDamage = 10;
    public Transform rangeAttackPoint;

    //DasAttack
    public Vector2 dashAttackRange = new Vector2(9f, 1.5f);
    public int dashAttackDamage = 10;
    public Transform dashAttackPoint;



    public void BasicAttack()
    {
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(basicAttackPoint.position, basicAttackRange, playerLayers);
        foreach (Collider2D player in hitPlayer)
        {
            if (player.CompareTag("Player"))
            {
                PlayerHealth health = player.GetComponent<PlayerHealth>();
                if (health != null)
                {
                    bool attackFromRight = transform.position.x > player.transform.position.x;
                    health.TakeDamage(chainAttackDamage, attackFromRight);
                    Debug.Log("BasicAttack");
                }
                else
                {
                    Debug.Log("Khong tim thay playerHealth");
                }
            }
        }
    }
    public void RangeAttack()
    {
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(rangeAttackPoint.position, rangeAttackRange, playerLayers);
        foreach (Collider2D player in hitPlayer)
        {
            if (player.CompareTag("Player"))
            {
                PlayerHealth health = player.GetComponent<PlayerHealth>();
                if (health != null)
                {
                    bool attackFromRight = transform.position.x > player.transform.position.x;
                    health.TakeDamage(chainAttackDamage, attackFromRight);
                    Debug.Log("RangeAttack");
                }
                else
                {
                    Debug.Log("Khong tim thay playerHealth");
                }
            }
        }
    }
    public void ChainAttack()
    {
        Collider2D[] hitPlayer = Physics2D.OverlapBoxAll(chainAttackPoint.position, chainAttackRange, 0f, playerLayers);
        foreach (Collider2D player in hitPlayer)
        {
            if (player.CompareTag("Player"))
            {
                PlayerHealth health = player.GetComponent<PlayerHealth>();
                if (health != null)
                {
                    bool attackFromRight = transform.position.x > player.transform.position.x;
                    health.TakeDamage(chainAttackDamage, attackFromRight);
                    Debug.Log("ChainAttack");
                }
                else
                {
                    Debug.Log("Khong tim thay playerHealth");
                }
            }
        }
    }
    public void DashAttack()
    {
        Collider2D[] hitPlayer = Physics2D.OverlapBoxAll(dashAttackPoint.position, dashAttackRange, 0f, playerLayers);
        foreach (Collider2D player in hitPlayer)
        {
            if (player.CompareTag("Player"))
            {
                PlayerHealth health = player.GetComponent<PlayerHealth>();
                if (health != null)
                {
                    bool attackFromRight = transform.position.x > player.transform.position.x;
                    health.TakeDamage(chainAttackDamage, attackFromRight);
                    Debug.Log("DashAttack");
                }
                else
                {
                    Debug.Log("Khong tim thay playerHealth");
                }
            }
        }
    }
    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(basicAttackPoint.position, basicAttackRange);
        Gizmos.color = Color.blue;
        if (dashAttackPoint == null) return;
        Gizmos.DrawWireCube(dashAttackPoint.position, dashAttackRange);
        Gizmos.color = Color.green;
        if (rangeAttackPoint == null) return;
        Gizmos.DrawWireSphere(rangeAttackPoint.position, rangeAttackRange);
        Gizmos.color = Color.yellow;
        if (chainAttackPoint == null) return;
        Gizmos.DrawWireCube(chainAttackPoint.position, chainAttackRange);
        Gizmos.color = Color.white;
    }
}

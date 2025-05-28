using UnityEngine;
using System.Collections;

public class ComboAttackController : MonoBehaviour
{
    [Header("Combo Settings")]
    [SerializeField] private float comboTimeWindow = 1.2f;    // Thời gian cho phép để tiếp tục combo
    [SerializeField] private WeaponData weaponData;


    [Header("Attack Settings")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private bool showHitboxGizmo = true;

    private bool hitboxActive = false;
    private Animator animator;
    private PlayerController player;
    private int currentComboCount = 0;
    private float lastAttackTime = 0f;
    private bool canStartNextAttack = false;  // Đợi animation kết thúc mới kích hoạt đòn tiếp theo
    private bool comboWindowOpen = false;
    private bool attackInputQueued = false;   // Biến lưu trữ input tấn công trong khi chờ animation kết thúc
    private Coroutine comboWindowCoroutine;


    [SerializeField] private MonsterController monster;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private AttackData attackData;
    private void Start()
    {
        animator = GetComponent<Animator>();
        player = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!player.isAttacking || canStartNextAttack)
            {
                HandleAttackInput();
            }
            else if (player.isAttacking && comboWindowOpen)
            {
                attackInputQueued = true;
            }
        }

        if (comboWindowOpen && Time.time - lastAttackTime > comboTimeWindow)
        {
            ResetCombo();
        }
    }

    public void ActivateHitbox()
    {
        if (weaponData == null || currentComboCount <= 0 || currentComboCount > weaponData.comboAttacks.Length)
            return;

        AttackData currentAttack = weaponData.comboAttacks[currentComboCount - 1];
        StartCoroutine(ActiveHitboxRoutine(currentAttack));
    }
    private IEnumerator ActiveHitboxRoutine(AttackData attackData)
    {
        hitboxActive = true;

        PerformAttack(attackData);

        yield return new WaitForSeconds(attackData.hitboxActiveTime);

        hitboxActive = false;
    }
    private void PerformAttack(AttackData attackData)
    {
        bool isFacingRight = transform.localScale.x > 0;

        Vector2 hitboxPosition = (Vector2)attackPoint.position +
            (isFacingRight ? attackData.hitboxOffset : new Vector2(-attackData.hitboxOffset.x, attackData.hitboxOffset.y));

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(hitboxPosition, attackData.attackRadius, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            // Tính toán hướng tấn công dựa trên vị trí của từng enemy cụ thể
            bool attackFromRight = transform.position.x > enemy.transform.position.x;

            MonsterHealth monsterHealth = enemy.GetComponent<MonsterHealth>();
            if (monsterHealth != null)
            {
                monsterHealth.TakeDamage(attackData.damage + playerStats.Damage, transform, attackFromRight);

                if (attackData.launchEnemy)
                {
                    Rigidbody2D enemyRb = enemy.GetComponent<Rigidbody2D>();
                    if (enemyRb != null)
                    {
                        float knockbackDirectionX = enemy.transform.position.x - transform.position.x;
                        float directionSign = Mathf.Sign(knockbackDirectionX);

                        enemyRb.velocity = Vector2.zero; // Reset velocity trước
                        enemyRb.AddForce(new Vector2(directionSign * attackData.knockbackForceX, attackData.knockbackForceY), ForceMode2D.Impulse);
                    }
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (!showHitboxGizmo || attackPoint == null || weaponData == null ||
            weaponData.comboAttacks == null || currentComboCount <= 0 ||
            currentComboCount > weaponData.comboAttacks.Length)
            return;

        Gizmos.color = hitboxActive ? Color.red : Color.yellow;
        AttackData attackData = weaponData.comboAttacks[currentComboCount - 1];

        // Tính toán vị trí hitbox dựa trên hướng nhân vật
        bool isFacingRight = transform.localScale.x > 0;
        Vector2 hitboxPosition = (Vector2)attackPoint.position +
            (isFacingRight ? attackData.hitboxOffset : new Vector2(-attackData.hitboxOffset.x, attackData.hitboxOffset.y));

        Gizmos.DrawWireSphere(hitboxPosition, attackData.attackRadius);
    }

    public void SetWeapon(WeaponData data)
    {
        weaponData = data;

        if (data.animatorController != null)
        {
            GetComponent<Animator>().runtimeAnimatorController = data.animatorController;
        }
    }

    private void HandleAttackInput()
    {
        if (weaponData == null || weaponData.comboTriggers == null || weaponData.comboTriggers.Length == 0)
            return;

        // Nếu đã quá thời gian combo thì reset về đòn đầu tiên
        if (Time.time - lastAttackTime > comboTimeWindow && currentComboCount > 0)
        {
            currentComboCount = 0;
        }

        lastAttackTime = Time.time;
        currentComboCount++;

        // Vòng lại nếu combo vượt giới hạn số trigger được khai báo
        if (currentComboCount > weaponData.comboTriggers.Length)
        {
            currentComboCount = 1;
        }

        player.isAttacking = true;
        canStartNextAttack = false;
        attackInputQueued = false;

        // Kích hoạt trigger tương ứng từ WeaponData
        string triggerName = weaponData.comboTriggers[currentComboCount - 1];
        animator.SetTrigger(triggerName);

        StartComboWindow();
    }


    // Mở cửa sổ thời gian để tiếp nhận đòn combo tiếp theo
    private void StartComboWindow()
    {
        // Hủy coroutine cũ nếu còn đang chạy
        if (comboWindowCoroutine != null)
        {
            StopCoroutine(comboWindowCoroutine);
        }

        comboWindowCoroutine = StartCoroutine(ComboWindowTimer());
    }

    private IEnumerator ComboWindowTimer()
    {
        // Đợi animation bắt đầu chạy một chút
        yield return new WaitForSeconds(0.1f);

        // Mở cửa sổ combo nhưng vẫn không cho bắt đầu đòn mới cho đến khi animation kết thúc
        comboWindowOpen = true;

        // Đợi hết thời gian combo window
        yield return new WaitForSeconds(comboTimeWindow);

        // Nếu không nhận được input tiếp theo, reset combo
        ResetCombo();
    }

    // Gọi từ animation event khi animation tấn công kết thúc
    public void OnAttackAnimationEnd()
    {
        // Đánh dấu có thể bắt đầu đòn tấn công tiếp theo
        canStartNextAttack = true;

        // Nếu cửa sổ combo vẫn mở và có input được queue
        if (comboWindowOpen && attackInputQueued)
        {
            // Xử lý input tấn công tiếp theo
            HandleAttackInput();
        }
        // Nếu đã hết thời gian combo window, reset trạng thái tấn công
        else if (!comboWindowOpen || Time.time - lastAttackTime > comboTimeWindow)
        {
            ResetCombo();
        }
    }

    // Reset combo và trạng thái tấn công
    private void ResetCombo()
    {
        player.isAttacking = false;
        currentComboCount = 0;
        comboWindowOpen = false;
        canStartNextAttack = false;
        attackInputQueued = false;

        if (comboWindowCoroutine != null)
        {
            StopCoroutine(comboWindowCoroutine);
        }
    }

    // Gọi khi bị ngắt animation tấn công (ví dụ: bị đánh, nhảy, né...)
    public void InterruptAttack()
    {
        ResetCombo();
    }
}
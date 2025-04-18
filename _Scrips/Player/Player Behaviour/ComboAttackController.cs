using UnityEngine;
using System.Collections;

public class ComboAttackController : MonoBehaviour
{
    [Header("Combo Settings")]
    [SerializeField] private float comboTimeWindow = 1.2f;    // Thời gian cho phép để tiếp tục combo
    [SerializeField] private string attack1Trigger = "Attack1";
    [SerializeField] private string attack2Trigger = "Attack2";
    [SerializeField] private string attack3Trigger = "Attack3";

    [Header("Debug")]
    [SerializeField] private bool showDebugInfo = true;

    private Animator animator;
    private PlayerController player;
    private int currentComboCount = 0;
    private float lastAttackTime = 0f;
    private bool canStartNextAttack = false;  // Đợi animation kết thúc mới kích hoạt đòn tiếp theo
    private bool comboWindowOpen = false;
    private bool attackInputQueued = false;   // Biến lưu trữ input tấn công trong khi chờ animation kết thúc
    private Coroutine comboWindowCoroutine;

    private void Start()
    {
        animator = GetComponent<Animator>();
        player = GetComponent<PlayerController>();
    }

    private void Update()
    {
        // Xử lý input tấn công
        if (Input.GetMouseButtonDown(0))
        {
            // Nếu chưa tấn công hoặc có thể bắt đầu đòn tiếp theo, xử lý ngay
            if (!player.isAttacking || canStartNextAttack)
            {
                HandleAttackInput();
            }
            // Nếu đang trong animation tấn công, queue input để xử lý sau
            else if (player.isAttacking && comboWindowOpen)
            {
                attackInputQueued = true;
            }
        }

        // Hiển thị debug info
        if (showDebugInfo && comboWindowOpen)
        {
            float remainingTime = comboTimeWindow - (Time.time - lastAttackTime);
            Debug.Log($"Combo Window: {remainingTime:F2}s remaining");
        }

        // Kiểm tra hết thời gian combo window
        if (comboWindowOpen && Time.time - lastAttackTime > comboTimeWindow)
        {
            ResetCombo();
        }
    }

    private void HandleAttackInput()
    {
        // Nếu đã quá thời gian cho phép combo, reset về đòn đầu tiên
        if (Time.time - lastAttackTime > comboTimeWindow && currentComboCount > 0)
        {
            currentComboCount = 0;
        }

        // Cập nhật thời gian tấn công gần nhất
        lastAttackTime = Time.time;

        // Tăng đếm combo
        currentComboCount++;
        if (currentComboCount > 3)
        {
            currentComboCount = 1; // Quay lại đòn đầu tiên nếu vượt quá số đòn tối đa
        }

        // Đánh dấu đang tấn công và reset các trạng thái
        player.isAttacking = true;
        canStartNextAttack = false;
        attackInputQueued = false;

        // Kích hoạt animation tương ứng
        switch (currentComboCount)
        {
            case 1:
                animator.SetTrigger(attack1Trigger);
                break;
            case 2:
                animator.SetTrigger(attack2Trigger);
                break;
            case 3:
                animator.SetTrigger(attack3Trigger);
                break;
        }

        // Bắt đầu hoặc reset combo window
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
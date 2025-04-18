using UnityEngine;

public abstract class AttackState : PlayerState
{
    protected float comboWindowStart; // Thời điểm bắt đầu cửa sổ combo
    protected float comboWindowDuration; // Độ dài cửa sổ combo
    protected bool canCombo; // Có thể chuyển sang đòn tiếp theo không
    protected string animationName; // Tên animation tương ứng

    public AttackState(PlayerController player) : base(player)
    {
    }

    public override void EnterState()
    {
        // Chạy trực tiếp animation tương ứng
        //animator.Play(animationName);
        canCombo = false;
        // Mở cửa sổ combo sau 0.2s, kéo dài 0.4s
        comboWindowStart = Time.time + 0.2f;
        comboWindowDuration = 0.4f;
    }

    public override void UpdateState()
    {
        // Kiểm tra nếu đang trong cửa sổ combo
        if (Time.time >= comboWindowStart && Time.time <= comboWindowStart + comboWindowDuration)
        {
            canCombo = true;
        }
        else
        {
            canCombo = false;
        }

        // Nếu nhấn chuột trong cửa sổ combo, chuyển sang trạng thái tiếp theo
        if (canCombo && Input.GetMouseButtonDown(0))
        {
            player.ChangeState(GetNextAttackState());
        }
    }

    public override void ExitState()
    {
        // Không cần reset trigger vì không dùng
    }

    // Phương thức trừu tượng để trả về trạng thái tấn công tiếp theo
    protected abstract PlayerState GetNextAttackState();
}
//using UnityEngine;

//public class WallSlideState : PlayerState
//{
//    private float slideSpeed = -3f; // Tốc độ trượt, điều chỉnh tùy ý

//    public WallSlideState(PlayerController player) : base(player)
//    {
//        this.player = player;
//    }

//    public override void EnterState()
//    {
//        animator.Play("WallSlide");
//    }

//    public override void UpdateState()
//    {
//        // Giảm tốc độ rơi khi trượt tường
//        player.Rigidbody.velocity = new Vector2(player.Rigidbody.velocity.x, slideSpeed);

//        // Thoát state nếu không còn chạm tường hoặc chạm đất
//        if (!player.InWall || player.IsGrounded)
//        {
//            player.ChangeState(new IdleState(player)); // Quay lại state idle hoặc state khác
//        }
//    }
//}
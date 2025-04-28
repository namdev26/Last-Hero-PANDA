//using UnityEngine;

//public class ParticleController : MonoBehaviour
//{
//    [SerializeField] ParticleSystem movementParticle;

//    [Range(0, 10)]
//    [SerializeField] float occurAfterVelocity = 1f; // Ngưỡng vận tốc tối thiểu để phát bụi

//    [Range(0, 0.2f)]
//    [SerializeField] float dustFormationPeriod = 0.1f; // Khoảng thời gian giữa các lần phát bụi

//    [SerializeField] Rigidbody2D playerRb;

//    float counter = 0f;

//    private void Start()
//    {
//        if (playerRb == null || movementParticle == null)
//        {
//            Debug.LogError("playerRb hoặc movementParticle chưa được gán!");
//        }
//    }

//    private void UpdateInventoryUI()
//    {
//        if (playerRb == null || movementParticle == null) return;

//        counter += Time.deltaTime;

//        if (Mathf.Abs(playerRb.velocity.x) > occurAfterVelocity)
//        {
//            if (counter >= dustFormationPeriod)
//            {
//                PlayDust();
//                counter = 0;
//            }
//        }
//        else if (movementParticle.isPlaying)
//        {
//            movementParticle.Stop(); // Dừng bụi khi vận tốc không đủ
//        }
//    }

//    private void PlayDust()
//    {
//        movementParticle.Play();
//    }
//}
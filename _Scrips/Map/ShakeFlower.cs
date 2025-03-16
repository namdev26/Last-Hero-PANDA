using System.Collections;
using UnityEngine;

public class ShakeFlower : MonoBehaviour
{
    private Quaternion originQuaternion;
    [SerializeField] private float shakeSpeed = 0.1f;
    [SerializeField] private float shakeTime = 1f;
    [SerializeField] private float shakeAngle = 5f;
    private void Start()
    {
        originQuaternion = transform.localRotation;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(ShakeEffect());
        }
    }
    private IEnumerator ShakeEffect()
    {
        float time = 0f;

        while (time < shakeTime)
        {
            time += Time.deltaTime;
            // Sử dụng Mathf.PingPong để tạo dao động nghiêng rõ hơn
            float angle = Mathf.Sin(time * shakeSpeed * Mathf.PI * 2) * shakeAngle;
            transform.localRotation = originQuaternion * Quaternion.Euler(0, 0, angle);
            yield return null;
        }
        transform.localRotation = originQuaternion;
    }
}

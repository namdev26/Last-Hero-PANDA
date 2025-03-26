using System.Collections;
using UnityEngine;

public class FallThrough : MonoBehaviour
{
    [SerializeField] private Collider2D collider; // Thay Collider2D bằng CompositeCollider2D
    private bool playerOnPlatform;
    private float lastDownPressTime;

    private void Start()
    {
        collider = GetComponent<Collider2D>(); // Lấy CompositeCollider2D
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (Time.time - lastDownPressTime < 0.3f && playerOnPlatform)
            {
                collider.enabled = false; // Tắt va chạm của CompositeCollider2D
                StartCoroutine(EnableCollider());
            }
            lastDownPressTime = Time.time;
        }

    }


    private IEnumerator EnableCollider()
    {
        yield return new WaitForSeconds(0.07f);
        collider.enabled = true; // Bật va chạm của CompositeCollider2D
    }

    private void SetPlayerOnPlatform(Collider2D collider, bool value)
    {
        var player = collider.GetComponent<PlayerController>();
        if (player != null)
        {
            playerOnPlatform = value;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        SetPlayerOnPlatform(collision.collider, true);
        Debug.Log("Player on platform");
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        SetPlayerOnPlatform(collision.collider, false);
        Debug.Log("Player exit platform");
    }
}
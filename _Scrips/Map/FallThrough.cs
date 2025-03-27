using System.Collections;
using UnityEngine;

public class FallThrough : MonoBehaviour
{
    [SerializeField] private Collider2D _collider; // Collider của Map
    private bool playerOnPlatform;
    private float lastDownPressTime;

    private void Start()
    {
        _collider = GetComponent<Collider2D>(); // Lấy Collider của map auto
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (Time.time - lastDownPressTime < 0.3f && playerOnPlatform)
            {
                _collider.enabled = false; // Tắt va chạm của Collider2D
                StartCoroutine(EnableCollider());
            }
            lastDownPressTime = Time.time;
        }

    }


    private IEnumerator EnableCollider()
    {
        yield return new WaitForSeconds(0.05f);
        _collider.enabled = true; // Bật va chạm của Collider2D
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
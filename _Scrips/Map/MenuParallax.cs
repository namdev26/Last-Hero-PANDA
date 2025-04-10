using UnityEngine;

public class MenuParallax : MonoBehaviour
{
    public float offsetMuiltiplier = 1f;
    public float smoothTime = 0.3f;

    private Vector2 startPos;
    private Vector2 velocity;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        Vector2 offset = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        transform.position = Vector2.SmoothDamp(transform.position, startPos + (offset * offsetMuiltiplier), ref velocity, smoothTime);
    }
}

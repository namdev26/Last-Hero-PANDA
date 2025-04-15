using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform target;
    private Vector3 velocity = Vector3.zero;

    public float MinModifier = 0.2f;
    public float MaxModifier = 0.4f;
    private float smoothTime;

    private bool isFollowing = false;

    private void Start()
    {
        StartFollowing();
    }
    public void StartFollowing()
    {
        //smoothTime = Random.Range(MinModifier, MaxModifier);
        isFollowing = true;
    }

    void FixedUpdate()
    {
        if (isFollowing)
        {
            transform.position = Vector3.SmoothDamp(transform.position, target.position, ref velocity, 0.2f);
        }
    }
}

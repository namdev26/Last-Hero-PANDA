using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private Transform target;
    private Rigidbody2D rb;
    private Gold goldScript;
    private Vector3 velocity = Vector3.zero;

    [SerializeField] private float smoothTime = 0.2f;
    [SerializeField] private float attractionDelay = 1f;
    [SerializeField] private float attractionDistance = 3f;
    private bool isFollowing = false;
    private bool isCollected = false;
    private float timer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        goldScript = GetComponent<Gold>();

        StartFollowing();
    }

    public void StartFollowing()
    {
        GameObject player = GameObject.FindGameObjectWithTag("DropLootTarget");
        if (player != null)
        {
            target = player.transform;
            isFollowing = true;
        }
        else
        {
            Debug.LogWarning("Không tìm thấy Player! Vui lòng kiểm tra tag 'Player'.");
            isFollowing = false;
        }
    }

    void Update()
    {
        if (isCollected || !isFollowing || target == null || !enabled) return;

        timer += Time.deltaTime;

        if (timer >= attractionDelay)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, target.position);
            if (distanceToPlayer <= attractionDistance)
            {
                if (goldScript != null) goldScript.SetTrail(true);
                if (rb != null) rb.isKinematic = true;

                Vector3 targetPos = target.position;
                targetPos.z = transform.position.z;
                transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);

                if (distanceToPlayer < 0.2f && goldScript != null)
                {
                    isCollected = true;
                    enabled = false;
                    goldScript.SetTrail(false);
                    goldScript.Collect();
                }
            }
        }
    }
}
using System;
using UnityEngine;

public class Gold : MonoBehaviour, IColectiable
{
    public int amount = 200;
    public static event Action OnGoldCollected;

    private TrailRenderer trail;
    private ParticleSystem sparkleEffect;
    private bool isCollected = false;

    void Start()
    {
        trail = GetComponent<TrailRenderer>();
        //sparkleEffect = GetComponent<ParticleSystem>();

        if (trail != null) trail.emitting = false;
        if (sparkleEffect != null) sparkleEffect.Play();
    }

    public void Collect()
    {
        if (isCollected) return;

        isCollected = true;
        enabled = false;

        OnGoldCollected?.Invoke();
        Debug.Log("Gold collected!");

        if (trail != null)
        {
            trail.emitting = false;
            trail.Clear();
        }
        if (sparkleEffect != null) sparkleEffect.Stop();

        Destroy(gameObject);
    }

    public void SetTrail(bool enable)
    {
        if (trail != null && !isCollected) trail.emitting = enable;
    }
}
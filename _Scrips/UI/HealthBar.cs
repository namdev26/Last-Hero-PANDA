using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image healthBarFillImage;
    [SerializeField] private Image healthBarTrailFillImage;
    [SerializeField] private float trailDelay = 0.4f;
    [SerializeField] private PlayerHealth playerHealth;
    private Tween healthTween;

    private void Awake()
    {
        if (playerHealth == null)
            playerHealth = FindObjectOfType<PlayerHealth>();
    }

    private void OnEnable()
    {
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged += UpdateHealthBar;
            playerHealth.OnMaxHealthChanged += UpdateHealthBarMax;
        }
    }

    private void OnDisable()
    {
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged -= UpdateHealthBar;
            playerHealth.OnMaxHealthChanged -= UpdateHealthBarMax;
        }
    }

    private void UpdateHealthBar(float healthRatio)
    {
        healthTween?.Kill();
        healthTween = DOTween.Sequence()
            .Append(healthBarFillImage.DOFillAmount(healthRatio, 0.25f).SetEase(Ease.InOutSine))
            .AppendInterval(trailDelay)
            .Append(healthBarTrailFillImage.DOFillAmount(healthRatio, 0.3f).SetEase(Ease.InOutSine))
            .Play();
    }

    private void UpdateHealthBarMax(float maxHealth)
    {
        // Cập nhật thanh máu khi max health thay đổi
        float healthRatio = playerHealth.CurrentHealth / maxHealth;
        healthBarFillImage.fillAmount = healthRatio;
        healthBarTrailFillImage.fillAmount = healthRatio;
    }
}
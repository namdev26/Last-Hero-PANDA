using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image healthBarFillImage;
    [SerializeField] private Image healthBarTrailFillImage;
    [SerializeField] private float trailDelay = 0.4f;
    [SerializeField] private MonoBehaviour targetHealthScript;

    private IHealth healthTarget;
    private Tween healthTween;

    private void Awake()
    {
        healthTarget = targetHealthScript as IHealth;
    }

    private void OnEnable()
    {
        if (healthTarget != null)
        {
            healthTarget.OnHealthChanged += UpdateHealthBar;
            healthTarget.OnMaxHealthChanged += ForceUpdateHealthBar;
        }
    }

    private void OnDisable()
    {
        if (healthTarget != null)
        {
            healthTarget.OnHealthChanged -= UpdateHealthBar;
            healthTarget.OnMaxHealthChanged -= ForceUpdateHealthBar;
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

    private void ForceUpdateHealthBar(float maxHealth)
    {
        if (healthTarget == null || maxHealth <= 0f) return;
        float ratio = healthTarget.CurrentHealth / maxHealth;
        healthBarFillImage.fillAmount = ratio;
        healthBarTrailFillImage.fillAmount = ratio;
    }
}

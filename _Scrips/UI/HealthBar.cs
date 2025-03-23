using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.InputSystem;


public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image healthBarFillImage;
    [SerializeField] private Image healthBarTrailFillImage;
    [SerializeField] private float trailDelay = 0.4f;
    [SerializeField] private PlayerStats player;

    private void Awake()
    {
        player.currentHealth = player.maxHealth;
        healthBarFillImage.fillAmount = 1f;
        healthBarTrailFillImage.fillAmount = 1f;
    }

    private void Update()
    {
        if (Keyboard.current.ctrlKey.wasPressedThisFrame)
        {
            DrainHealthBar();
        }
    }

    void DrainHealthBar()
    {
        player.currentHealth -= 10;
        float ratio = player.currentHealth / player.maxHealth;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(healthBarFillImage.DOFillAmount(ratio, 0.25f))
        .SetEase(Ease.InOutSine);
        sequence.AppendInterval(trailDelay);
        sequence.Append(healthBarTrailFillImage.DOFillAmount(ratio, 0.3f))
        .SetEase(Ease.InOutSine);

        sequence.Play();
    }
}

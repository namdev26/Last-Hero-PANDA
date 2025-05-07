using TMPro;
using UnityEngine;

namespace Inventory.UI
{
    public class UICharacterInfo : MonoBehaviour
    {
        [SerializeField] private TMP_Text healthText;
        [SerializeField] private TMP_Text damageText;
        [SerializeField] private TMP_Text defenceText;
        [SerializeField] private TMP_Text speedText;

        private PlayerStats playerStats;

        public void Initialize(PlayerStats stats)
        {
            playerStats = stats;
            if (playerStats != null)
            {
                playerStats.OnStatsChanged += UpdateUI; // Đăng ký sự kiện
                if (gameObject.activeSelf)
                {
                    UpdateUI(); // Chỉ cập nhật nếu panel đang bật
                }
            }
        }

        private void UpdateUI()
        {
            if (playerStats == null)
            {
                Debug.LogError("PlayerStats is null in UICharacterInfo.");
                return;
            }

            if (!gameObject.activeSelf)
            {
                return;
            }

            healthText.text = $"Ben Bi: {playerStats.Health}";
            damageText.text = $"Suc Manh: {playerStats.Damage}";
            defenceText.text = $"Kien Cuong: {playerStats.Defence}";
            speedText.text = $"Kheo Leo: {playerStats.Speed}";
        }

        public void Show()
        {
            gameObject.SetActive(true);
            UpdateUI(); // Cập nhật ngay khi hiển thị
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            if (playerStats != null)
            {
                playerStats.OnStatsChanged -= UpdateUI; // Hủy đăng ký sự kiện
            }
        }
    }
}
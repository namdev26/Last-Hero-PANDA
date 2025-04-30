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
        //[SerializeField] private Image characterPortrait; // Tùy chọn

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

            // Chỉ cập nhật UI nếu panel đang bật
            if (!gameObject.activeSelf)
            {
                return;
            }

            healthText.text = $"Health: {playerStats.Health}";
            damageText.text = $"Damage: {playerStats.Damage}";
            defenceText.text = $"Defence: {playerStats.Defence}";
            speedText.text = $"Speed: {playerStats.Speed}";

            // Cập nhật portrait nếu có
            //if (characterPortrait != null)
            //{
            //    // characterPortrait.sprite = playerStats.PortraitSprite; // Nếu có
            //}

            Debug.Log($"UICharacterInfo updated: Health={playerStats.Health}, Damage={playerStats.Damage}, Defence={playerStats.Defence}, Speed={playerStats.Speed}");
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
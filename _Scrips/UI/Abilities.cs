using UnityEngine;
using UnityEngine.UI;

public class Abilities : MonoBehaviour
{
    [Header("Skill 1")]
    public Image skill1Image;
    public float skill1Cooldown = 5f;
    private bool skill1IsCooldown = false;
    public KeyCode skill1Key = KeyCode.Q;


    private void Start()
    {
        skill1Image.fillAmount = 0;
    }

    private void Update()
    {
        Skill1();
    }

    void Skill1()
    {
        if (Input.GetKeyDown(skill1Key) && !skill1IsCooldown)
        {
            skill1IsCooldown = true;
            skill1Image.fillAmount = 1;
        }
        if (skill1IsCooldown)
        {
            skill1Image.fillAmount -= 1 / skill1Cooldown * Time.deltaTime;
            if (skill1Image.fillAmount <= 0)
            {
                skill1Image.fillAmount = 0;
                skill1IsCooldown = false;
            }
        }
    }
}

using UnityEngine;
using TMPro;

public class CollectionManager : MonoBehaviour
{
    public TextMeshProUGUI gemUI;
    int numberOfGolds = 0;

    private void OnEnable()
    {
        Gold.OnGoldCollected += GoldCollected;
    }

    private void OnDisable()
    {
        Gold.OnGoldCollected -= GoldCollected;
    }

    private void GoldCollected()
    {
        numberOfGolds++;
        gemUI.text = numberOfGolds.ToString();
    }
}

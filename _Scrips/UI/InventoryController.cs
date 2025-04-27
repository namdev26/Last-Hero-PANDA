using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] UIInventoryPage inventoryPage;


    public int inventorySize = 20;


    private void Start()
    {
        inventoryPage.InitInventoryUI(inventorySize);
    }

    public void Update()
    {
        if ((Input.GetKeyDown(KeyCode.I)))
        {
            if (inventoryPage.isActiveAndEnabled == false)
            {
                inventoryPage.Show();
            }
            else
            {
                inventoryPage.Hide();
            }
        }
    }
}

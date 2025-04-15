using UnityEngine;

public class Collector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        IColectiable collectible = other.GetComponent<IColectiable>();
        if (collectible != null)
        {
            collectible.Collect();
        }
    }
}

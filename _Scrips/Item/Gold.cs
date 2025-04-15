using System;
using UnityEngine;

public class Gold : MonoBehaviour, IColectiable
{
    public int amount = 10; // số vàng của item
    public static event Action OnGoldCollected;
    public void Collect()
    {
        OnGoldCollected?.Invoke();
        Debug.Log("Glod collected!");
        Destroy(gameObject);
    }
}

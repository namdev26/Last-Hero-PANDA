using UnityEngine;

public class FlowerSpawner : MonoBehaviour
{
    public GameObject[] flowerPrefabs; // Các Prefab hoa có thể spawn
    public int flowerCount = 20; // Số lượng hoa muốn tạo
    public Vector2 minPosition; // Góc dưới bên trái của vùng spawn
    public Vector2 maxPosition; // Góc trên bên phải của vùng spawn

    void Start()
    {
        SpawnFlowers();
    }

    void SpawnFlowers()
    {
        for (int i = 0; i < flowerCount; i++)
        {
            // Chọn vị trí ngẫu nhiên trong vùng spawn
            float randomX = Random.Range(minPosition.x, maxPosition.x);
            float randomY = Random.Range(minPosition.y, maxPosition.y);
            Vector2 spawnPos = new Vector2(randomX, randomY);

            // Spawn hoa với Prefab ngẫu nhiên
            GameObject flower = Instantiate(flowerPrefabs[Random.Range(0, flowerPrefabs.Length)], spawnPos, Quaternion.identity);
            //flower.transform.localScale = new Vector3(Random.Range(0.8f, 1.2f), Random.Range(0.8f, 1.2f), 1f); // Biến đổi kích thước nhẹ để tạo sự tự nhiên
        }
    }
}

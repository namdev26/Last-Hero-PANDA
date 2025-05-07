using UnityEngine;
using UnityEngine.InputSystem;

public abstract class NPC : MonoBehaviour, IInteractable
{
    [SerializeField] private SpriteRenderer interactSprite;
    [SerializeField] private float interactDistance = 1.5f;
    [SerializeField] private GameObject textTab;
    [SerializeField] private Transform playerTransform;

    private bool wasWithinDistanceLastFrame = false;

    void Start()
    {
        if (!ValidateReferences()) return;

        textTab.SetActive(false);
        interactSprite.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!ValidateReferences()) return;

        bool isWithinDistance = isWhithinInteractDistance();

        // Xử lý phím Tab để tương tác
        if (Keyboard.current.tabKey.wasPressedThisFrame && isWithinDistance)
        {
            Interact();
        }

        // Cập nhật trạng thái interactSprite và textTab chỉ khi cần thiết
        if (isWithinDistance != wasWithinDistanceLastFrame)
        {
            interactSprite.gameObject.SetActive(isWithinDistance);
            textTab.SetActive(isWithinDistance);
            wasWithinDistanceLastFrame = isWithinDistance;
        }
    }

    public abstract void Interact();

    private bool isWhithinInteractDistance()
    {
        if (playerTransform == null)
        {
            Debug.LogError("NPC: PlayerTransform is null! Cannot calculate distance.");
            return false;
        }
        return Vector2.Distance(playerTransform.position, transform.position) < interactDistance;
    }

    private bool ValidateReferences()
    {
        if (playerTransform == null)
        {
            Debug.LogError("NPC: PlayerTransform không được gán trong Inspector!");
            return false;
        }
        if (interactSprite == null)
        {
            Debug.LogError("NPC: InteractSprite không được gán trong Inspector!");
            return false;
        }
        if (textTab == null)
        {
            Debug.LogError("NPC: TextTab không được gán trong Inspector!");
            return false;
        }
        return true;
    }
}
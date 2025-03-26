using UnityEngine;
using UnityEngine.InputSystem;

public abstract class NPC : MonoBehaviour, IInteractable
{
    [SerializeField] private SpriteRenderer interactSprite;
    [SerializeField] private float interactDistance = 1.5f;
    private Transform playerTransform;

    void Start()
    {
        playerTransform = GameObject.Find("Player").transform;
    }
    private void Update()
    {
        if (Keyboard.current.tabKey.wasPressedThisFrame && isWhithinInteractDistance())
        {
            Interact();
        }

        if (interactSprite.gameObject.activeSelf && !isWhithinInteractDistance())
        {
            // ở ngoài khoảng thì tắt
            interactSprite.gameObject.SetActive(false);
        }
        else if (!interactSprite.gameObject.activeSelf && isWhithinInteractDistance())
        {
            // ở trong khoảng thì bật
            interactSprite.gameObject.SetActive(true);
        }
    }

    public abstract void Interact();

    private bool isWhithinInteractDistance()
    {
        return Vector2.Distance(playerTransform.position, transform.position) < interactDistance;
    }


}

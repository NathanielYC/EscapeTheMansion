using System;
using UnityEngine;
using UnityEngine.InputSystem;

public enum ItemType
{
    Gem,
    Healthpack,
    Booster,
    Weapon
}
public class Collectable : MonoBehaviour
{
    public ItemType itemType;
    public Sprite sprite;
    public int value;
    public InputActionReference pickupReference;
    
    public InventoryManager inventoryManager;
    public BoxCollider2D playerCollider;

    private void OnEnable()
    {
        if (pickupReference != null && pickupReference.action != null)
        {
            pickupReference.action.performed += OnPickup;
        }
    }

    private void OnDisable()
    {
        if (pickupReference != null && pickupReference.action != null)
        {
            pickupReference.action.performed -= OnPickup;
        }
    }
    private void Ability()
    {
        
    }

    private void OnPickup(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (playerCollider.IsTouching(GetComponent<Collider2D>()))
        {
            gameObject.SetActive(false); // Simulate picking up by deactivating the object
            inventoryManager.AddItem(this); // Add this item to the inventory manager
            Debug.Log("Picked up: " + itemType + " with value: " + value);
        }
    }
    

}


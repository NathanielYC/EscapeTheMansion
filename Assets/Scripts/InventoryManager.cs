using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public GameObject inventoryBar;
    private int currentItemIndex;
    private GameObject[] inventoryItems = new GameObject[8];
    public UnityEngine.UI.Image[] slotHighlight = new UnityEngine.UI.Image[8];
    public UnityEngine.UI.Image[] itemSlot = new UnityEngine.UI.Image[8];
    public InputActionReference nextItemReference;
    public InputActionReference prevItemReference;
    public InputActionReference dropReference;
    public InputActionReference useReference;

    void Start()
    {
        //Debug.Log(inventorySlots[0].isActiveAndEnabled);
        currentItemIndex = 0;
        for(int i = 1; i <= 7; i++)
        {
            slotHighlight[i].enabled = false; // Disable all slots except the first one
        }
        for(int i = 0; i < itemSlot.Length; i++)
        {
            itemSlot[i].enabled = false; // Disable all item slots at the start
        }
        //Debug.Log(slotHighlight[0].isActiveAndEnabled);
        //for (int i=0;i<slotHighlight.Length;i++)
        //Debug.Log($"slot[{i}] = {(slotHighlight[i]!=null ? slotHighlight[i].name : "NULL")}");
    }

    private void OnEnable()
    {
        if (nextItemReference != null && nextItemReference.action != null)
        {
            nextItemReference.action.performed += OnNextItem;
            if (!nextItemReference.action.enabled)
                nextItemReference.action.Enable();
        }

        if (prevItemReference != null && prevItemReference.action != null)
        {
            prevItemReference.action.performed += OnPrevItem;
            if (!prevItemReference.action.enabled)
                prevItemReference.action.Enable();
        }

        if (dropReference != null && dropReference.action != null)
        {
            dropReference.action.performed += RemoveItem;
            if (!dropReference.action.enabled)
                dropReference.action.Enable();
        } 
        
        if (useReference != null && useReference.action != null)
        {
            useReference.action.performed += UseItem;
            if (!useReference.action.enabled)
                useReference.action.Enable();
        }  
    }

    private void OnDisable()
    {
        if (nextItemReference != null && nextItemReference.action != null)
        {
            nextItemReference.action.performed -= OnNextItem;
            if (nextItemReference.action.enabled)
                nextItemReference.action.Disable();
        }

        if (prevItemReference != null && prevItemReference.action != null)
        {
            prevItemReference.action.performed -= OnPrevItem;
            if (prevItemReference.action.enabled)
                prevItemReference.action.Disable();
        }

        if (dropReference != null && dropReference.action != null)
        {
            dropReference.action.performed -= RemoveItem;
            if (dropReference.action.enabled)
                dropReference.action.Disable();
        }
        
        if (useReference != null && useReference.action != null)
        {
            useReference.action.performed -= UseItem;
            if (useReference.action.enabled)
                useReference.action.Disable();
        }
    }

    private void OnNextItem(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        // next item (e.g. scroll down)
        slotHighlight[currentItemIndex].enabled = false;
        currentItemIndex = (currentItemIndex + 1) % inventoryItems.Length;
        Debug.Log("Current inventory item index: " + currentItemIndex);
        slotHighlight[currentItemIndex].enabled = true;
    }
    private void OnPrevItem(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        // previous item (e.g. scroll up)
        slotHighlight[currentItemIndex].enabled = false;
        currentItemIndex = (currentItemIndex - 1 + inventoryItems.Length) % inventoryItems.Length;
        Debug.Log("Current inventory item index: " + currentItemIndex);
        slotHighlight[currentItemIndex].enabled = true;
    }

    public void AddItem(Collectable item)
    {
        for (int i = 0; i < inventoryItems.Length; i++)
        {
            if (inventoryItems[i] == null)
            {
                inventoryItems[i] = item.gameObject;
                // hide the picked up item from the scene
                if (item.gameObject != null)
                    item.gameObject.SetActive(false);
                itemSlot[i].sprite = item.GetComponent<SpriteRenderer>().sprite; // Set the slot image to the item's sprite
                itemSlot[i].enabled = true; // Enable the slot to show the item
                Debug.Log("Added item: " + item.name + " to inventory slot " + i);
                return;
            }
            else
            {
                Debug.Log("Inventory slot " + i + " is occupied by: " + (inventoryItems[i] != null ? inventoryItems[i].name : "NULL"));
            }
        }
        Debug.Log("Inventory is full! Cannot add item: " + item.name);
    }

    public void RemoveItem(InputAction.CallbackContext context)
    {
        if (inventoryItems[currentItemIndex] != null)
        {
            GameObject itemObj = inventoryItems[currentItemIndex];
            // reactivate the original game object in the scene
            itemObj.SetActive(true);
            // place it at this manager's position (adjust later if needed)
            itemObj.transform.position = transform.position;

            // clear inventory slot
            inventoryItems[currentItemIndex] = null;
            itemSlot[currentItemIndex].sprite = null;
            itemSlot[currentItemIndex].enabled = false;
            return;
        }
    }
    public void UseItem(InputAction.CallbackContext context)
    {
        
    }
}

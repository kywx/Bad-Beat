



using UnityEngine;


public class CanAddInteractableItem : MonoBehaviour, IInteractable
{   
    public Item itemData; // Reference to inventory representation
    public InventoryManager InventoryManager;

    public bool CanInteract()
    {
        return true;
    }

    public void Interact(){
    if (!CanInteract()) return;

    // Add item to inventory
    InventoryManager.instance.AddItem(itemData);

    Object.Destroy(gameObject);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

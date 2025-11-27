using UnityEngine;
using UnityEngine.UI; // Needed for UI Text
using System.Collections.Generic;
using TMPro;

public class InventoryManager : MonoBehaviour{
    public List<Item> items = new List<Item>();
    public GameObject inventoryUI;
    public Transform itemListParent; // assign Content from Scroll View
    public GameObject itemSlotPrefab; // assign prefab for UI slot
    public TMPro.TextMeshProUGUI notificationText; // Assign in Inspector if you want UI feedback
    public Dictionary<Item, int> itemCounts = new Dictionary<Item, int>();

    

    public static InventoryManager instance;



    // test
    public Item testBatteryItem;
    public Item testBatteryItem_2;
    void Start() {
     }
    // test end

    void Awake(){
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        inventoryUI.SetActive(false);
    }
    
    

    void Update(){
    if (Input.GetKeyDown(KeyCode.I)){
        ToggleInventoryUI();
        }
    }
    public void ToggleInventoryUI() {
    bool willOpen = !inventoryUI.activeSelf;
    inventoryUI.SetActive(willOpen);
    Time.timeScale = willOpen ? 0 : 1; // Pause when open, resume when closed
}

    public bool HasItem(Item item)
    {
        return itemCounts.ContainsKey(item) && itemCounts[item] > 0;
    }




    public void AddItem(Item item) {
        if (itemCounts.ContainsKey(item))
            itemCounts[item]++;
        else
            itemCounts[item] = 1;
        UpdateInventoryUI();
        NotifyNewItem(item);
    }

    public void RemoveItem(Item item) {
        if (itemCounts.ContainsKey(item)) {
            itemCounts[item]--;
            if (itemCounts[item] <= 0)
                itemCounts.Remove(item);
            UpdateInventoryUI();
        }
    }

    void UpdateInventoryUI() {
        foreach (Transform child in itemListParent)
            Destroy(child.gameObject);
        foreach (var kvp in itemCounts) {
            GameObject slot = Instantiate(itemSlotPrefab, itemListParent);
            slot.GetComponent<ItemSlotUI>().Setup(kvp.Key, this, kvp.Value); // Pass count!
        }
    }
    
    void NotifyNewItem(Item item){
        Debug.Log("Added new item: " + item.itemName);

        // If you have a UI Text element, you can do:
        if (notificationText != null)
        {
            notificationText.text = "Added: " + item.itemName;
            // Optionally clear the message after a short delay:
            CancelInvoke("ClearNotification");
            Invoke("ClearNotification", 2f); // clears in 2 seconds
        }
    }
    void ClearNotification(){
        if (notificationText != null)
            notificationText.text = "";
    }

    




}

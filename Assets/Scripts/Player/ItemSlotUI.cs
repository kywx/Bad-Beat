using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // ← required for PointerEventData and IPointerClickHandler
using TMPro;
public class ItemSlotUI : MonoBehaviour , IPointerClickHandler{
    public Image icon;
    public TMPro.TextMeshProUGUI nameText;
    public TMPro.TextMeshProUGUI countText; // Add an extra Text field

    Item myItem;
    InventoryManager manager;

    // Change Setup to accept count
    public void Setup(Item item, InventoryManager im, int count) {
        myItem = item;
        manager = im;
        icon.sprite = item.icon;
        nameText.text = item.itemName;
        countText.text = count > 1 ? count.ToString() : ""; // Only show when >1
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == PointerEventData.InputButton.Right) {
            manager.RemoveItem(myItem);
        }
    }
}

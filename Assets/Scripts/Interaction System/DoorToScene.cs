using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorToScene : MonoBehaviour, IInteractable
{
    [Header("Door Settings")]
    public string sceneName;              // Name of the scene to load
    public Item requiredKey;             // Assign the key item in Inspector
    DialogueData dialogueData = null;

    private void Start()
    {
        dialogueData = GetComponent<DialogueData>();
    }


    public bool CanInteract()
    {

        return true;
    }

    public void Interact()
    {
        if (!CanInteract())
        {
            return;
        }

         if (InventoryManager.instance != null &&
               InventoryManager.instance.HasItem(requiredKey)){
                 Debug.Log("Opening door to scene: " + sceneName);

        Time.timeScale = 1f;//in case if player open inventory
        SceneManager.LoadScene(sceneName);
        }
        else{
            DialogueManager.Instance.StartDialogue(dialogueData);
            Debug.Log("No key");
            // Optionally consume the key:
            // InventoryManager.instance.RemoveItem(requiredKey);
        }

  

       
    }

}

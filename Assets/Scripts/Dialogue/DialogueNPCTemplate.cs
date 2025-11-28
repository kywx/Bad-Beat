using UnityEngine;

public class DialogueNPCTemplate : MonoBehaviour, IInteractable
{
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

        // Callback parameter is optional
        DialogueManager.Instance.StartDialogue(dialogueData, OnDialogueEnd);
    }

    private void OnDialogueEnd()
    {
        Debug.Log("Finished talking. Can do something here.");
    }
}

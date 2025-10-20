using UnityEngine;

public class InteractionDetector : MonoBehaviour
{
    private IInteractable interactableInRange = null;
    public GameObject interactionPrompt;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        interactionPrompt.SetActive(false);
    }

    private void Update()
    {
        if (InputManager.InteractWasPressed && interactableInRange != null) 
        {
            interactableInRange?.Interact();
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out IInteractable interactable) && interactable.CanInteract())
        {
            interactableInRange = interactable;
            interactionPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable) && interactable == interactableInRange)
        {
            interactableInRange = null;
            interactionPrompt.SetActive(false);
        }
    }


}

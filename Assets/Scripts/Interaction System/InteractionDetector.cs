using System.Collections.Generic;
using UnityEngine;

public class InteractionDetector : MonoBehaviour
{
    private List<IInteractable> interactableInRange = new List<IInteractable>();
    public GameObject interactionPrompt;

    void Start()
    {
        interactionPrompt.SetActive(false);
    }

    private void Update()
    {
        if (InputManager.InteractWasPressed && interactableInRange.Count > 0) 
        {
            foreach (var interactable in interactableInRange)
            {
                if (interactable.CanInteract())
                {
                    interactable?.Interact();
                }
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IInteractable[] interactables = collision.GetComponents<IInteractable>();
        foreach (var interactable in interactables)
        {
            // skip disabled scripts
            var script = interactable as MonoBehaviour;
            if (!script.enabled)
                continue;

            if (interactable.CanInteract())
            {
                interactableInRange.Add(interactable);
            }
        }
        if (interactableInRange.Count > 0)
            interactionPrompt.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        IInteractable[] interactables = collision.GetComponents<IInteractable>();

        foreach (var interactable in interactables)
        {
            interactableInRange.Remove(interactable);
        }

        if (interactableInRange.Count == 0)
            interactionPrompt.SetActive(false);
    }
}

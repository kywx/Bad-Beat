using UnityEngine;

public class TestItem : MonoBehaviour, IInteractable
{
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

        print("You have collected the item");
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

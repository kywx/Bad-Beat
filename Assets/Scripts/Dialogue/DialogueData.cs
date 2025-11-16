using System.Collections.Generic;
using UnityEngine;

public class DialogueData : MonoBehaviour, IInteractable
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

        DialogueManager.Instance.StartDialogue(this);
    }

    [SerializeField] private Sprite OtherSprite = null;

    [SerializeField]
    private int speakerCount = 1;

    [System.Serializable]
    public class DialogueLine
    {
        public string speakerName;
        public string lineText;
    }

    [SerializeField]
    private List<DialogueLine> speakerLines = new List<DialogueLine>();

    public List<DialogueLine> getLines()
    {
        return speakerLines;
    }

    public int getSpeakerCount()
    {
        return speakerCount;
    }

    public Sprite getOtherSprite()
    {
        return OtherSprite;
    }
}

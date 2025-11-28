using System.Collections.Generic;
using UnityEngine;

public class DialogueData : MonoBehaviour
{
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
}

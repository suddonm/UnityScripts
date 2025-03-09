using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogueNode", menuName = "Dialogue/Dialogue Node")]
public class DialogueNode : ScriptableObject
{
    [TextArea(3, 10)]
    public string dialogueText; // The dialogue text for this node

    public List<DialogueChoice> choices; // List of possible player choices
}

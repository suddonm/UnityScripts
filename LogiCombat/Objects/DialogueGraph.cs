using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogueGraph", menuName = "Dialogue/Dialogue Graph")]
public class DialogueGraph : ScriptableObject
{
    public DialogueNode startingNode; // The first node in the dialogue
}

[System.Serializable]
public class DialogueChoice
{
    public string choiceText; // Text shown to the player
    public DialogueNode nextNode; // The node to go to when this choice is selected
}

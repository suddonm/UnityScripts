using System.Collections.Generic;

public interface IInteractable
{
    List<string> GetInteractions(); // Returns a list of possible interactions
    void PerformInteraction(string interaction); // Executes the selected interaction
}

using UnityEngine;

public class MechUnitController : UnitController
{
    public int bonusMoveRange = 2; // Extra movement for the Overdrive ability

    // Override the bonus action to provide extra movement (Overdrive)
    public override void BonusAction()
    {
        base.BonusAction();

        // Extra movement for Mech (Overdrive ability)
        Move(transform.position + transform.forward * bonusMoveRange);
        Debug.Log($"{gameObject.name} used Overdrive for extra movement.");
    }

    // Reset the turn and any other mech-specific mechanics
    public override void ResetTurn()
    {
        base.ResetTurn();
        // Additional resets for mech-specific features if needed
    }
}
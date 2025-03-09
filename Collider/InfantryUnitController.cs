using UnityEngine;

public class InfantryUnitController : UnitController
{
    // Example: Infantry-specific bonus action (Take Cover)
    public override void BonusAction()
    {
        base.BonusAction();

        // Take cover action grants +2 defense
        defense += 2;
        Debug.Log($"{gameObject.name} took cover, defense increased to {defense}.");
    }

    // Reset the turn and also reset defense bonus
    public override void ResetTurn()
    {
        base.ResetTurn();

        // Reset defense after the turn ends
        defense = defense - 2 < 0 ? 0 : defense - 2; // Only remove the bonus if it was applied
    }
}
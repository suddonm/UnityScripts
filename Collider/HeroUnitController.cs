using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroUnitController : UnitController
{
    // Range of the inspiring command
    public int inspireRange = 2;
    public int inspireBonus = 1;

    // Hero-specific bonus action
    public override void BonusAction()
    {
        base.BonusAction();

        // Inspiring Command: Buffs nearby allied units
        Collider[] nearbyUnits = Physics.OverlapSphere(transform.position, inspireRange);

        foreach (var collider in nearbyUnits)
        {
            UnitController unit = collider.GetComponent<UnitController>();
            if (unit != null && unit != this) // Exclude self
            {
                unit.weapon.weapon_damage += inspireBonus;
                Debug.Log($"{gameObject.name} inspired {unit.gameObject.name}, increasing their attack damage.");
            }
        }
    }
}


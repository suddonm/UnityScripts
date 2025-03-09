using UnityEngine;

[CreateAssetMenu(fileName = "NewUnitStats", menuName = "Combat/UnitStats", order = 1)]
public class UnitStats : ScriptableObject
{
    [Header("Unit Details")]
    public string unitName;
    public int maxHealth;
    public int veterancy; // Higher veterancy = better accuracy and performance
    public float movementSpeed;

    [Header("Combat")]
    public Weapon weapon;

    public float GetEffectiveAccuracy()
    {
        return weapon.baseAccuracy + (0.01f * veterancy); // Accuracy improves by 1% per veterancy level
    }
}

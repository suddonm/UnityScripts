using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Combat/Weapon", order = 0)]
public class Weapon : ScriptableObject
{
    public string weaponName;
    public float fireRate; // Shots per second
    public float range; // Maximum range
    public float baseAccuracy; // Base accuracy (e.g., 0.8 = 80% accuracy)
    public int damage; // Damage dealt per shot

    [HideInInspector]
    public float lastFiredTime; // Tracks the last time the weapon fired

    public float Cooldown => 1f / fireRate; // Cooldown between shots

    public bool CanFire()
    {
        return Time.time - lastFiredTime >= Cooldown;
    }

    public void Fire()
    {
        lastFiredTime = Time.time;
    }

    public void ResetWeapon()
    {
        lastFiredTime = 0f;
    }
}

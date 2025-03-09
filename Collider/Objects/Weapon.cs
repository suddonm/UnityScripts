using UnityEngine;

[CreateAssetMenu(menuName = "Unit_Weapon", order = 1)]
public class Weapon : ScriptableObject
{
    public string weapon_name;
    public int weapon_damage;
    public int weapon_range;

    [SerializeField]
    public GameObject weapon_prefab;

    [SerializeField]
    public GameObject weapon_projectile_prefab;
}
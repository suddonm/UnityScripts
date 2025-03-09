using UnityEngine;

[CreateAssetMenu(menuName = "Unit_Armor", order = 2)]
public class Armor : ScriptableObject
{
    public string armor_name;
    public int armor_defense;

    [SerializeField]
    public GameObject armor_prefab;
}
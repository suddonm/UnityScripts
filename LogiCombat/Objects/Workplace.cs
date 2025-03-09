using UnityEngine;

[CreateAssetMenu(fileName = "Workplace", menuName = "Workplace")]
public class Workplace : ScriptableObject
{
    [Range(0, 24)] public float workStart = 9f;  // Work start time (24-hour format)
    [Range(0, 24)] public float workEnd = 17f;   // Work end time
}
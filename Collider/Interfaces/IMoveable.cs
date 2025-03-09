using UnityEngine;

internal interface IMoveable
{
    void Move(Vector3 targetPosition);
    void DisplayMoveRange();
    void HideMoveRange();
    void CalculateMoveArea();
}
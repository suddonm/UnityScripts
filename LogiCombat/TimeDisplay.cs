using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeDisplay : MonoBehaviour
{
    public TMP_Text timeText;

    private void Update()
    {
        if (TimeManager.Instance != null)
        {
            timeText.text = TimeManager.Instance.GetCurrentTime();
        }
    }
}

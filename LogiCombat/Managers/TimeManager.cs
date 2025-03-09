using System;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    public event Action<string> OnTimeChanged; // Event for time updates
    public float gameSpeed = 1f; // Speed multiplier for in-game time (1x, 2x, etc.)
    public int minutesPerSecond = 1; // How many in-game minutes pass per real second

    private float timeElapsed = 0f; // Tracks real time elapsed
    private int currentHour = 8; // Start hour
    private int currentMinute = 0; // Start minute
    private string currentTime;

    private void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Initialize time
        currentTime = FormatTime(currentHour, currentMinute);
    }

    private void Update()
    {
        // Progress time based on gameSpeed and minutesPerSecond
        timeElapsed += Time.deltaTime * gameSpeed;

        if (timeElapsed >= 1f) // 1 real second elapsed
        {
            timeElapsed -= 1f;
            UpdateTime();
        }
    }

    private void UpdateTime()
    {
        // Add in-game minutes based on the minutesPerSecond setting
        currentMinute += minutesPerSecond;

        // Handle overflow of minutes into hours
        if (currentMinute >= 60)
        {
            currentMinute -= 60;
            currentHour++;
        }

        // Handle overflow of hours into a new day
        if (currentHour >= 24)
        {
            currentHour = 0;
        }

        // Format the new time and send the event
        currentTime = FormatTime(currentHour, currentMinute);
        OnTimeChanged?.Invoke(currentTime);
    }

    private string FormatTime(int hour, int minute)
    {
        return string.Format("{0:00}:{1:00}", hour, minute); // Returns time as "HH:mm"
    }

    // Optional: Method to set a custom time (for testing or game events)
    public void SetTime(int hour, int minute)
    {
        currentHour = hour % 24;
        currentMinute = minute % 60;
        currentTime = FormatTime(currentHour, currentMinute);
        OnTimeChanged?.Invoke(currentTime);
    }

    // Optional: Method to get the current time
    public string GetCurrentTime()
    {
        return currentTime;
    }
}

using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Timer : MonoBehaviour
{
    [Header("Component")]
    public TextMeshProUGUI timerText;

    [Header("Timer Settings")]
    public float currentTime;
    public bool countDown;

    [Header("Limit Settings")]
    public bool hasLimit;
    public float timerLimit;

    [Header("Format Settings")]
    public bool hasFormat;
    public TimerFormats format;

    private Dictionary<TimerFormats, string> timeFormats = new Dictionary<TimerFormats, string>();

    public enum TimerFormats
    {
        Minutes,
        TenthDecimal,
        HundredDecimal,
        MinutesWithMilliseconds // 🆕 Tambahan format ini
    }

    void Start()
    {
        timeFormats.Add(TimerFormats.Minutes, @"mm\.ss");
        timeFormats.Add(TimerFormats.TenthDecimal, "0.0");
        timeFormats.Add(TimerFormats.HundredDecimal, "0.00");
        timeFormats.Add(TimerFormats.MinutesWithMilliseconds, @"mm\.ss\.ff"); // 🆕 mm:ss:ff
    }

    void Update()
    {
        currentTime = countDown ? currentTime - Time.deltaTime : currentTime + Time.deltaTime;

        if ((hasLimit && currentTime >= timerLimit) || (countDown && currentTime <= 0))
        {
            currentTime = timerLimit;
            SetTimerText();
            timerText.color = Color.red;
            enabled = false;
            return;
        }

        SetTimerText();
    }

    private void SetTimerText()
    {
        if (hasFormat)
        {
            if (format == TimerFormats.Minutes || format == TimerFormats.MinutesWithMilliseconds)
            {
                TimeSpan timeSpan = TimeSpan.FromSeconds(currentTime);
                timerText.text = timeSpan.ToString(timeFormats[format]); // Contoh: 01:23:45
            }
            else
            {
                timerText.text = currentTime.ToString(timeFormats[format]);
            }
        }
        else
        {
            timerText.text = currentTime.ToString();
        }
    }
}

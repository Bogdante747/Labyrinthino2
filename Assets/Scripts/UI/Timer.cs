using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private Image timerSlider;
    [SerializeField] private Text timerText;
    [SerializeField] private float startTime;
    [SerializeField] private float timer;
    [SerializeField] private bool pause = false;
    public bool TimeExpired = false;

    void Start()
    {
        timer = startTime;
    }

    void Update()
    {
        if (!TimeExpired && !pause)
        {
            timer -= Time.deltaTime;
        }
            timerSlider.fillAmount = timer / startTime;
            timerText.text = Mathf.Ceil(timer).ToString("0"); 

        if (timer <= 0)
        {
            TimeExpired = true;
            timer = 0;
        }
    }


    /// <summary>
    /// Поставить или снять с паузы
    /// </summary>
    /// <param name="boolean">Булевое значение (поставить на паузу или снять)</param>
    public void Pause(bool boolean)
    {
        pause = boolean;
    }


    /// <summary>
    /// Установить таймер
    /// </summary>
    /// <param name="time">Время таймера</param>
    public void SetTimer(float time)
    {
        startTime = time;
        timer = time;
        TimeExpired = false;
    }
}

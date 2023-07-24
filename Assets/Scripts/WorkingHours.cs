using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class WorkingHours : MonoBehaviour
{
    private DateTime time;
    private int localTimeHour = 3;
    public float timeHours;
    public float timeMinutes;
    public float timeSeconds;
    private const float hoursDegree = 360f / 12f;
    private const float minutesDegree = 360f / 60f;
    private const float secondsDegree = 360f / 60f;
    public Transform Hours;
    public Transform Minutes;
    public Transform Seconds;

    private float _elepsedTimae = 0;
    public float CurentTime;
    public Coroutine coroutine;

    private void Start()
    {
        time = CheckGlobalTime();
    }

    private void Update()
    {
        _elepsedTimae += Time.deltaTime;

        timeHours = localTimeHour + time.Hour + (Time.time / 3600);
        timeMinutes = time.Minute + (Time.time / 60);
        timeSeconds = time.Second + Time.time;

        var timeCurentHours = timeHours - localTimeHour;

        Hours.localRotation = Quaternion.Euler(0f, timeCurentHours * hoursDegree, 0f);
        Minutes.localRotation = Quaternion.Euler(0f, timeMinutes * minutesDegree,0f);
        Seconds.localRotation = Quaternion.Euler(0f, timeSeconds * secondsDegree,0f);

        if(_elepsedTimae >= CurentTime)
        {
            if(coroutine == null)
            {
                coroutine = StartCoroutine(CheckTime());
            }
            else
            {
                StopCoroutine(coroutine);
                coroutine = null;
                coroutine= StartCoroutine(CheckTime());
            }
        }
    }

    private IEnumerator CheckTime()
    {
        time = CheckGlobalTime();
        _elepsedTimae = 0;
        yield return null;
    }

    private DateTime CheckGlobalTime()
    {
        DateTime globDateTime;

        var www = new WWW("https://time100.ru/Moscow");

        while (!www.isDone && www.error == null)
        {
            Thread.Sleep(1);
        }

         var timeStr = www.responseHeaders["Date"];

        print(timeStr);
        

        if (!DateTime.TryParse(timeStr, out globDateTime))
        {
            return DateTime.MinValue;
        }

        return globDateTime.ToUniversalTime();
    }
}

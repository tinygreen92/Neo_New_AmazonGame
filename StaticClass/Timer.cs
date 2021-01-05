using System;
using System.Collections;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public event Action<float> PointOneSecondAction = delegate { };
    public event Action<float> SecondAction = delegate { };

    private void Awake()
    {
        //StartCoroutine(PointOneSecond());
        //StartCoroutine(OneSecond());
    }

    private IEnumerator PointOneSecond()
    {
        WaitForSeconds delay = new WaitForSeconds(0.1f);

        while (true)
        {
            yield return delay;

            PointOneSecondAction(0.1f);
        }
    }

    private IEnumerator OneSecond()
    {
        WaitForSeconds delay = new WaitForSeconds(1f);

        while (true)
        {
            yield return delay;

            SecondAction(1f);
        }
    }

    float _TestDelay;
    void StartTimer()
    {
        PointOneSecondAction += PointOneTimer;
    }
    void PointOneTimer(float delay)
    {
        _TestDelay += delay;
        if (_TestDelay > 3)
        {
            /// TODO : 행위

            _TestDelay = 0;
        }
    }

}
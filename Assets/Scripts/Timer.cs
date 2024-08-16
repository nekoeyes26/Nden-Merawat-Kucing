using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Timer : MonoBehaviour
{
    public float time;
    public Image fill;
    public float max;

    void Start()
    {

    }

    void Update()
    {
        time -= Time.deltaTime;
        fill.fillAmount = time / max;

        if (time < 0)
        {
            time = 0;
        }
    }
}

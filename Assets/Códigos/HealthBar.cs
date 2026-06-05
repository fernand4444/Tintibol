using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Barradevida : MonoBehaviour
{
    private Slider _mySlider;

    private void Awake()
    {
        _mySlider = GetComponent<Slider>();
    }

    public void Initialize(int maxLP, int currentLP)
    {
        if (_mySlider == null)
            return;

        _mySlider.maxValue = maxLP;
        _mySlider.value = currentLP;
    }

    public void SetHealth(int currentLP)
    {
        if (_mySlider == null)
            return;

        _mySlider.value = currentLP;
    }
}
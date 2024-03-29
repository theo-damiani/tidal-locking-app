﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderSync : MonoBehaviour
{
    [SerializeField] TidalLockingSlideController slideController;
    [SerializeField] Slider slider;

    public enum SliderValueName {
        MoonPeriodFactor,
        MoonSpinSpeed
    };
    [SerializeField] public SliderValueName sliderValueName;

    [Header("UI Paramaters")]
    [SerializeField] TextMeshProUGUI TMPgui;
    [SerializeField] Image fillImage;
    [SerializeField] Image syncImage;
    [SerializeField] TextMeshProUGUI syncLabel;

    [Header("Sync Parameters")]
    [SerializeField] Color syncColor;
    [SerializeField] public float syncValue;
    [SerializeField] List<Color> defaultColors;

    public void Start() {
        if (slider) {
            slider.onValueChanged.AddListener(delegate {SliderValueChange();});
        }
        if (TMPgui) {
            float valueLabel = slideController.getMoonPeriod();
            TMPgui.text = valueLabel.ToString("F1");
        }
    }

    public void SliderValueChange() {
        if (sliderValueName==SliderValueName.MoonPeriodFactor) {
            slideController.SetMoonPeriodFactor(slider2sim(slider.value));
            if (TMPgui) {
                float valueLabel = slideController.getMoonPeriod();
                if (valueLabel>5000f) {
                    valueLabel = Mathf.Infinity;
                }
                TMPgui.text = valueLabel.ToString("F1");
            }
        } 
        else {
            float spinSpeed=slider2sim(slider.value);
            slideController.SetMoonSpinSpeed(spinSpeed);
            if (TMPgui) {
                float valueLabel = slideController.getMoonPeriod()/slider.value;
                if (valueLabel>5000f) {
                    valueLabel = Mathf.Infinity;
                }
                if (slider.value==2)
                {
                    // Cheat to have the same value's range in slide 6 & 7: [+inf, 9.2]
                    valueLabel = 9.2f;
                }
                TMPgui.text = valueLabel.ToString("F1");
            }
        }
    }

    public void updateValue(float valueLabel, float simValue) {
        if (slider) {
            //float newValue=sim2slider(simValue);
            slider.value = simValue;
            if (simValue == syncValue) {
                fillImage.color=syncColor;
                syncImage.color=syncColor;
                syncLabel.color=syncColor;
            }
        }
    }

    public void resetSlider() {
        if (fillImage) {
            fillImage.color=defaultColors[0];
            syncImage.color=defaultColors[1];
            syncLabel.color=defaultColors[1];
        }
        slider.value=syncValue;
    }

    public float slider2sim(float value) {
        if (sliderValueName==SliderValueName.MoonPeriodFactor) {
            return 1/(Mathf.Pow(2, value)-1);
        }
        else {
            return Mathf.Pow(value, 4.32193f)-1;
        }
    }

    public float sim2slider(float value) {
        if (sliderValueName==SliderValueName.MoonPeriodFactor) {
            return Mathf.Log((1/value)+1 , 2);
        }
        else {
            return Mathf.Pow(value+1, 1/4.32193f);
        }
    }
}

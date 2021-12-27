using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderCorrector : MonoBehaviour
{
    public GameObject Fill;
    public Slider slider;


    void Update()
    {
        if (slider.value == slider.minValue)
        {
            Fill.SetActive(false);
        } else if (!Fill.activeInHierarchy && slider.value > slider.minValue)
        {
            Fill.SetActive(true);
        }
    }
}

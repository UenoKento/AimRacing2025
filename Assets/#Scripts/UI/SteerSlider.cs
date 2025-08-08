using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SteerSlider : MonoBehaviour
{
    [SerializeField]
    VehicleController vehicleController;

    [SerializeField]
    Slider m_slider;

    void Reset()
    {
        TryGetComponent<Slider>(out m_slider);

        m_slider.minValue = -1f;
        m_slider.maxValue = 1f;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_slider.value = vehicleController.Steering;
    }
}

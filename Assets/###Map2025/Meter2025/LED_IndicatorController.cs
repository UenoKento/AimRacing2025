using UnityEngine;
using System.Collections;
using NUnit.Framework;

public class LED_IndicatorController : MonoBehaviour
{
    // LEDÇäiî[
    [SerializeField] private LED_Controller[] m_LED_List;

    [SerializeField] float m_yellowZoneRPM_min;
    [SerializeField] float m_yellowZoneRPM_max;

    [SerializeField] float m_greenZoneRPM_min;
    [SerializeField] float m_greenZoneRPM_max;

    [SerializeField] float m_blueZoneRPM_min;
    [SerializeField] float m_blueZoneRPM_max;

    [SerializeField] float m_redZoneRPM_min;
    [SerializeField] float m_redZoneRPM_max;

    void Start()
    {
    }

    void Update()
    {
        
    }

    public void Init()
    {
        for (int i = 0; i < m_LED_List.Length; i++)
        {
            m_LED_List[i].Init();
        }
    }

    /// <summary>
    /// LED IndicatorÇÃçXêVèàóù
    /// </summary>
    public void Run(float _engineRPM)
    {
        if(_engineRPM > 4000.0f && _engineRPM < 4300.0f)
        {
            m_LED_List[0].Blink();
            m_LED_List[1].LightOut();
            m_LED_List[2].LightOut();
            m_LED_List[3].LightOut();
            m_LED_List[4].LightOut();
            m_LED_List[5].LightOut();
            m_LED_List[6].LightOut();
            m_LED_List[7].LightOut();
            m_LED_List[8].LightOut();
        }
        else if(_engineRPM > 4300.0f && _engineRPM < 4600.0f)
        {
            m_LED_List[0].Blink();
            m_LED_List[1].Blink();
            m_LED_List[2].LightOut();
            m_LED_List[3].LightOut();
            m_LED_List[4].LightOut();
            m_LED_List[5].LightOut();
            m_LED_List[6].LightOut();
            m_LED_List[7].LightOut();
            m_LED_List[8].LightOut();
        }
        else if(_engineRPM > 4600.0f && _engineRPM < 5000.0f)
        {
            m_LED_List[0].Lit();
            m_LED_List[1].Blink();
            m_LED_List[2].Blink();
            m_LED_List[3].LightOut();
            m_LED_List[4].LightOut();
            m_LED_List[5].LightOut();
            m_LED_List[6].LightOut();
            m_LED_List[7].LightOut();
            m_LED_List[8].LightOut();
        }
        else if(_engineRPM > 5000.0f && _engineRPM < 5400.0f){
            m_LED_List[0].Lit();
            m_LED_List[1].Lit();
            m_LED_List[2].Blink();
            m_LED_List[3].Blink();
            m_LED_List[4].LightOut();
            m_LED_List[5].LightOut();
            m_LED_List[6].LightOut();
            m_LED_List[7].LightOut();
            m_LED_List[8].LightOut();
        }
        else if(_engineRPM > 5400.0f && _engineRPM < 6000.0f)
        {
            m_LED_List[0].Lit();
            m_LED_List[1].Lit();
            m_LED_List[2].Lit();
            m_LED_List[3].Blink();
            m_LED_List[4].Blink();
            m_LED_List[5].LightOut();
            m_LED_List[6].LightOut();
            m_LED_List[7].LightOut();
            m_LED_List[8].LightOut();
        }
        else if(_engineRPM > 6000.0f && _engineRPM < 6500.0f)
        {
            m_LED_List[0].Lit();
            m_LED_List[1].Lit();
            m_LED_List[2].Lit();
            m_LED_List[3].Lit();
            m_LED_List[4].Blink();
            m_LED_List[5].Blink();
            m_LED_List[6].LightOut();
            m_LED_List[7].LightOut();
            m_LED_List[8].LightOut();
        }
        else if(_engineRPM > 6500.0f && _engineRPM < 6800.0f)
        {
            m_LED_List[0].Lit();
            m_LED_List[1].Lit();
            m_LED_List[2].Lit();
            m_LED_List[3].Lit();
            m_LED_List[4].Lit();
            m_LED_List[5].Blink();
            m_LED_List[6].Blink();
            m_LED_List[7].LightOut();
            m_LED_List[8].LightOut();
        }
        else if(_engineRPM > 6800.0f && _engineRPM < 7200.0f)
        {
            m_LED_List[0].Lit();
            m_LED_List[1].Lit();
            m_LED_List[2].Lit();
            m_LED_List[3].Lit();
            m_LED_List[4].Lit();
            m_LED_List[5].Lit();
            m_LED_List[6].Blink();
            m_LED_List[7].Blink();
            m_LED_List[8].LightOut();
        }
        else if(_engineRPM > 7200.0f && _engineRPM < 7500.0f)
        {
            m_LED_List[0].Lit();
            m_LED_List[1].Lit();
            m_LED_List[2].Lit();
            m_LED_List[3].Lit();
            m_LED_List[4].Lit();
            m_LED_List[5].Lit();
            m_LED_List[6].Lit();
            m_LED_List[7].Blink();
            m_LED_List[8].Blink();
        }
        //else if(_engineRPM < 7500.0f)
        //{
        //    m_LED_List[0].Lit();
        //    m_LED_List[1].Lit();
        //    m_LED_List[2].Lit();
        //    m_LED_List[3].Lit();
        //    m_LED_List[4].Lit();
        //    m_LED_List[5].Lit();
        //    m_LED_List[6].Lit();
        //    m_LED_List[7].Blink();
        //    m_LED_List[8].Blink();
        //}
        else
        {
            m_LED_List[0].LightOut();
            m_LED_List[1].LightOut();
            m_LED_List[2].LightOut();
            m_LED_List[3].LightOut();
            m_LED_List[4].LightOut();
            m_LED_List[5].LightOut();
            m_LED_List[6].LightOut();
            m_LED_List[7].LightOut();
            m_LED_List[8].LightOut();
        }
    }

}

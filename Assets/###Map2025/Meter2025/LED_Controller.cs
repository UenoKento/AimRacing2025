using UnityEngine;
using UnityEngine.UI;

public class LED_Controller : MonoBehaviour
{
    // LED
    private Image m_LED;

    // �_�ő��x
    private float m_blinkSpeed;

    // �F
    enum LED_Color 
    {
        Red,
        Blue,
        Green,
        Yellow,
    }

    [SerializeField] LED_Color m_LED_Color;

    void Start()
    {
    }

    void Update()
    {
        //Blink();
    }

    public void Init()
    {
        // Image���擾
        m_LED = GetComponent<Image>();

        LightOut();
    }

    // �_��
    public void Lit()
    {
        m_LED.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    }

    // ����
    public void LightOut()
    {
        m_LED.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    }


    // �_��
    public void Blink()
    {
        // �_�ő��x��ݒ�(�_�ő��x�ɕ����������Ă��������ɂ���)
        m_blinkSpeed = Random.Range(48.0f, 50.0f);

        // �A���t�@�X�V
        m_LED.color = new Color(1.0f, 1.0f, 1.0f, Mathf.Sin(Time.time * m_blinkSpeed));
    }
}

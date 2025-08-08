using UnityEngine;
using UnityEngine.UI;

public class LED_Controller : MonoBehaviour
{
    // LED
    private Image m_LED;

    // 点滅速度
    private float m_blinkSpeed;

    // 色
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
        // Imageを取得
        m_LED = GetComponent<Image>();

        LightOut();
    }

    // 点灯
    public void Lit()
    {
        m_LED.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    }

    // 消灯
    public void LightOut()
    {
        m_LED.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    }


    // 点滅
    public void Blink()
    {
        // 点滅速度を設定(点滅速度に幅を持たせていい感じにする)
        m_blinkSpeed = Random.Range(48.0f, 50.0f);

        // アルファ更新
        m_LED.color = new Color(1.0f, 1.0f, 1.0f, Mathf.Sin(Time.time * m_blinkSpeed));
    }
}

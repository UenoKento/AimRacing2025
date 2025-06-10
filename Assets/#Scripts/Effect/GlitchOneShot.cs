using UnityEngine;
using UnityEngine.Rendering;

public class GlitchOneShot : MonoBehaviour
{
    [SerializeField]
    Volume m_volume;

    [SerializeField]
    float m_activeTime;
    float m_activedTime;

    bool m_isActive = false;
    bool m_isEnd = false;
    
    public bool IsEnd => m_isEnd;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
		m_volume.enabled = false;
	}

    // Update is called once per frame
    void Update()
    {
        if(Time.time - m_activedTime < m_activedTime)
        {
            m_volume.enabled = true;
        }
        else
        {
			m_volume.enabled = false;
            m_isEnd = true;
		}
    }

    public void Active()
    {
        if (m_isEnd || !m_isActive)
            return;

        m_isActive = true;
        m_activedTime = Time.time;
    }

	void ReUse()
	{
        m_isActive = false;
		m_isEnd = false;
	}
}

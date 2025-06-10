using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class AxisPressdOnce
{
    // ���͈ꎞ�ҋ@�p
    bool m_standby = false;
    // �V�[���J�ڋ���
    [SerializeField,ShowInInspector]
    bool m_allowScene = false;

    public bool PressedOnce => m_allowScene;

    public void AxisCheck(float _inputValue)
    {
        if (!m_standby)
        {
            // �y�_���𗣂������Ƃ��m�F
            if (_inputValue <= 0.1f)
            {
                m_standby = true;
            }
        }
        else
        {
            // �y�_���𓥂񂾂��Ƃ��m�F
            if (_inputValue >= 0.8f)
            {
                m_standby = false;
                m_allowScene = true;
            }
        }

        Debug.Log("Standby::[" + m_standby + "] Input::" + _inputValue);
    }
}

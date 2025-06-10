using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class InputDebuger : MonoBehaviour
{
    [SerializeField]
    string n_displayName;
	[SerializeField]
    TextMeshProUGUI m_text;

    bool m_isActive = false;
    float m_inputValue;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Reset()
    {
        TryGetComponent<TextMeshProUGUI>(out m_text);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
            m_isActive = m_text.enabled = !m_isActive;

        m_text.text = n_displayName + m_inputValue.ToString();
	}

    public void OnInput(InputAction.CallbackContext _context)
    { 
        m_inputValue = _context.ReadValue<float>();
    }

    public void OnInputPedal(InputAction.CallbackContext _context)
    {
        float value = _context.ReadValue<float>();
		value = 1 - (value + 1) / 2;

		m_inputValue = value;
    }
}

using UnityEngine;
using UnityEngine.UI;

public class Bouncing_Animation : MonoBehaviour
{
	Vector3 m_Now_Position;
	Vector3 m_Start_Position;
	Vector3 m_End_Position;

	[SerializeField]
	float m_AnimationSpeed;

	[SerializeField]
	float m_Movelenght;

	void Start()
	{
		m_Start_Position = GetComponent<RectTransform>().anchoredPosition;
		m_Now_Position = m_Start_Position;
		m_End_Position = new Vector3(m_Start_Position.x + m_Movelenght, m_Start_Position.y, m_Start_Position.z);
	}

	void Update()
	{
		m_Now_Position = new Vector3(m_Now_Position.x + m_AnimationSpeed,
			m_Now_Position.y,
			m_Now_Position.x);

		if (m_Now_Position.x >= m_End_Position.x)
		{
			Debug.Log("a");
			m_Now_Position = m_Start_Position;
		}

		GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(GetComponent<RectTransform>().anchoredPosition,m_Now_Position,0.1f);
	}
}

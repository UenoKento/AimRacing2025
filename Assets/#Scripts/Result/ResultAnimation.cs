//ResultUIのアニメーション

using System.Collections;
using UnityEngine;

public class ResultAnimation : MonoBehaviour
{

	[SerializeField]
	float StartAnimation_Time;

	[SerializeField]
	GameObject m_goalImage;

	[SerializeField]
	GameObject m_goalflagImage;

	[SerializeField]
	GameObject m_goalpositionImage;

	[SerializeField]
	float SetPos_Y = 500;

	[SerializeField]
	float LerpSpeed;

	void Start()
	{
		StartCoroutine(StartAnimation(StartAnimation_Time));
	}

	IEnumerator StartAnimation(float WaitTime)
	{
		yield return new WaitForSeconds(WaitTime);

		float UpdatePos_y;
		Vector3 UpdatePos = m_goalImage.GetComponent<RectTransform>().anchoredPosition;

		while (m_goalImage.GetComponent<RectTransform>().anchoredPosition.y < SetPos_Y)
		{
			UpdatePos_y = Mathf.Lerp(m_goalImage.GetComponent<RectTransform>().anchoredPosition.y, SetPos_Y, LerpSpeed);
			UpdatePos.y = UpdatePos_y;

			m_goalImage.GetComponent<RectTransform>().anchoredPosition = UpdatePos;

			yield return null;
		}

		UpdatePos.y = SetPos_Y;
		m_goalImage.GetComponent<RectTransform>().anchoredPosition = UpdatePos;
	}

}
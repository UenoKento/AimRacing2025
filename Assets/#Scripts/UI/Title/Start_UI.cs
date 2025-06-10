using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Start_UI : MonoBehaviour
{
	public CanvasRenderer canvasRenderer;
	public float fadeDuration = 1.0f;

	void Start()
	{
		// ���炩�ɓ_�ł�����R���[�`��
		StartCoroutine(SmoothBlink());
	}

	IEnumerator SmoothBlink()
	{
		while (true)
		{
			// �t�F�[�h�A�E�g
			for (float t = 0.0f; t < fadeDuration; t += Time.deltaTime)
			{
				Color color = canvasRenderer.GetColor();
				color.a = Mathf.Lerp(1.0f, 0.0f, t / fadeDuration);
				canvasRenderer.SetColor(color);
				yield return null;
			}

			// �t�F�[�h�C��
			for (float t = 0.0f; t < fadeDuration; t += Time.deltaTime)
			{
				Color color = canvasRenderer.GetColor();
				color.a = Mathf.Lerp(0.0f, 1.0f, t / fadeDuration);
				canvasRenderer.SetColor(color);
				yield return null;
			}
		}
	}
}
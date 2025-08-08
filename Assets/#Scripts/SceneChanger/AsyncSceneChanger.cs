/**
 * @file    AsyncSceneChanger.cs
 * @brief   �񓯊��ŃV�[�����[�h���Ă����@�\�̂���SceneChanger
 * @author  22cu0235 ������a�@�񓯊����[�h�����쐬
 *          22cu0219 ��ؗF��  �S�̐݌v
 */

using AIM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class AsyncSceneChanger : MonoBehaviour
{
	// �J�n���ɃV�[�����A�N�e�B�u�ɂ���
	[SerializeField]
	bool m_activeThisSceneOnAwake = true;

	[SerializeField]    // ���[�h����V�[��
	string m_loadScene;
	[SerializeField]	// �A�����[�h����V�[��
	string m_unloadScene;

	[SerializeField, ShowInInspector]
	float m_progress;

	private bool m_isAllowActive = false; // �V�[���J�ڋ��t���O
	bool m_isLoading = false;

	private void Start()
	{
		if(m_activeThisSceneOnAwake)
			SceneManager.SetActiveScene(gameObject.scene);
	}

	/// <summary>
	/// ���[�h�ς݂̃V�[���ɐ؂�ւ���
	/// </summary>
	public void ChangeScene()
	{
		if(!m_isAllowActive)
		{
			m_isAllowActive = true;
			SceneManager.UnloadSceneAsync(m_unloadScene);
			//Debug.Log("Called ChangeScene");
		}
	}

	/// <summary>
	/// ���[�h���J�n���郁�\�b�h
	/// </summary>
	public void StartLoad()
	{
		// �A�����[�h���̓��[�h�ł��Ȃ��悤�ɂ���
		if(!GameManager.Instance.IsAllUnloading && !m_isLoading)
		{
			StartCoroutine(Load());
			m_isLoading = true;
		}
	}

	/// <summary>
	/// ���[�h�����s���郁�\�b�h
	/// </summary>
	/// <returns></returns>
	private IEnumerator Load()
	{
		AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(m_loadScene,LoadSceneMode.Additive);
		asyncOperation.allowSceneActivation = false; // ���[�h�㏟��ɑJ�ڂ��Ȃ��悤��


		while (!asyncOperation.isDone)
		{
			// �i�s��
			m_progress = asyncOperation.progress;

			// allowSceneActivation��true�̏ꍇ0.9�Ń��[�h���~�܂適0.9 is ���H
			if (asyncOperation.progress >= 0.9f )
			{
				// �V�[���J��
				// �A�����[�h���͋����I�ɑS�V�[���A�N�e�B�u��
				if (m_isAllowActive || GameManager.Instance.IsAllUnloading)
                {
					asyncOperation.allowSceneActivation = true;
				}
			}

			yield return null;
		}
	}

}

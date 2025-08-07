/**
 * @file    AsyncSceneChanger.cs
 * @brief   非同期でシーンロードしておく機能のついたSceneChanger
 * @author  22cu0235 諸星大和　非同期ロード部分作成
 *          22cu0219 鈴木友也  全体設計
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
	// 開始時にシーンをアクティブにする
	[SerializeField]
	bool m_activeThisSceneOnAwake = true;

	[SerializeField]    // ロードするシーン
	string m_loadScene;
	[SerializeField]	// アンロードするシーン
	string m_unloadScene;

	[SerializeField, ShowInInspector]
	float m_progress;

	private bool m_isAllowActive = false; // シーン遷移許可フラグ
	bool m_isLoading = false;

	private void Start()
	{
		if(m_activeThisSceneOnAwake)
			SceneManager.SetActiveScene(gameObject.scene);
	}

	/// <summary>
	/// ロード済みのシーンに切り替える
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
	/// ロードを開始するメソッド
	/// </summary>
	public void StartLoad()
	{
		// アンロード中はロードできないようにする
		if(!GameManager.Instance.IsAllUnloading && !m_isLoading)
		{
			StartCoroutine(Load());
			m_isLoading = true;
		}
	}

	/// <summary>
	/// ロードを実行するメソッド
	/// </summary>
	/// <returns></returns>
	private IEnumerator Load()
	{
		AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(m_loadScene,LoadSceneMode.Additive);
		asyncOperation.allowSceneActivation = false; // ロード後勝手に遷移しないように


		while (!asyncOperation.isDone)
		{
			// 進行状況
			m_progress = asyncOperation.progress;

			// allowSceneActivationがtrueの場合0.9でロードが止まる←0.9 is 何？
			if (asyncOperation.progress >= 0.9f )
			{
				// シーン遷移
				// アンロード中は強制的に全シーンアクティブ化
				if (m_isAllowActive || GameManager.Instance.IsAllUnloading)
                {
					asyncOperation.allowSceneActivation = true;
				}
			}

			yield return null;
		}
	}

}

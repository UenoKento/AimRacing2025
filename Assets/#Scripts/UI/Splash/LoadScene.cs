using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/*
 * ロードするシーンを読み込むためのクラス
 */
public class LoadScene : MonoBehaviour
{
    [SerializeField]    // ロードするシーン名
    private string m_sceneName;

    [SerializeField]    // 進捗状況のUI
    private GameObject loadingUI;

    private AsyncOperation m_async; // 進捗状況の管理

    /// <summary>
    /// ロードを開始するメソッド
    /// </summary>
    public void StartLoad()
    {
        StartCoroutine(Load());
    }

    /// <summary>
    /// ロードを実行するメソッド
    /// </summary>
    /// <returns></returns>
    private IEnumerator Load()
    {
        // UIを表示する
        loadingUI.SetActive(true);

        // シーンを非同期でロードする
        m_async = SceneManager.LoadSceneAsync(m_sceneName);

        // ロードが完了するまで待機する
        while (!m_async.isDone)
        {
            yield return null;
        }

        // UIを非表示する
        loadingUI.SetActive(false);
    }
}

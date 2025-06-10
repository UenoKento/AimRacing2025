/**
 * @file    ManagerSceneLoader.cs
 * @brief   Manager用のシーンを読み込むための機能を実装
 * @author  22CU0219 鈴木友也
 * @date    2024/09/01  作成
 */

using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerSceneLoader : MonoBehaviour
{
    [SerializeField]
    string m_managerSceneName;
    static bool didLoad { get; set; }

    [Header("GameManager")]
    [SerializeField]
    GameStateManagerBase m_firstState;
    bool didSetState { get; set; }

    private void Awake()
    {
        if (didLoad) return;

        SceneManager.LoadScene(m_managerSceneName, LoadSceneMode.Additive);
        didLoad = true;
    }

    private void Update()
    {
        // ステートの設定が終わっていれば処理を抜ける
        if (didSetState) return;

        //ロード済みのシーンであれば、名前で別シーンを取得できる
        Scene scene = SceneManager.GetSceneByName(m_managerSceneName);
        if(scene.isLoaded)
        {
            GameManager.Instance.ChangeState(m_firstState);
            didSetState = true;
        }
    }
}

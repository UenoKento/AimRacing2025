using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneEntry : MonoBehaviour
{
    public string SceneName = "";
    private bool NextScene() { return Next; }   //アンロードしたかどうかを確かめる関数
    private bool Next;
    [Header("目標のスクリプトが終わったら次のシーンに移行")]
    public ATMTWindow Script;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadScene(SceneName));
    }
    IEnumerator LoadScene(string name)
    {
        //読み込むシーン
        AsyncOperation LoadAsync = SceneManager.LoadSceneAsync(name);
        LoadAsync.allowSceneActivation = false;
        //NextSceneがtrueになったらロードしたシーンに切り替える
        yield return new WaitUntil(NextScene);
        LoadAsync.allowSceneActivation = true;
        yield return null;
    }
}


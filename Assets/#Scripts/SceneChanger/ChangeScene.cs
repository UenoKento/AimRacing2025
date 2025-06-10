using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
public class ChangeScene : MonoBehaviour
{
    public string SceneName="";
    public bool Isload;

    public GameObject FadeOut;
    public G29 g29;
    //アンロードしたかどうかを確かめる関数
    private bool NextScene() 
    { 
        if(((g29.rec.lY / (float)-Int16.MaxValue + 1.0f) * 0.5f) >= 0.6f)
        {
            Isload = true;
        }
        else if(g29.accel <= -0.1f)
        {
            Isload = true;
        }
        else
        {
            Isload = false;
        }
        return Isload;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (SceneName == "")
        {
            SceneName = "Result_Scene";
        }
        Isload = false;
        
        StartCoroutine(LoadScene(SceneName));
    }
    IEnumerator LoadScene(string name)
    {
        //読み込むシーン
        AsyncOperation LoadAsync = SceneManager.LoadSceneAsync(name);
        LoadAsync.allowSceneActivation = false;
        yield return new WaitForSeconds(1);
        if (g29 == null)
        {
            //注意事項がスライドしたらシーン移行
            yield return new WaitForSeconds(3);
        }
        else
        {
            //NextSceneがtrueになったらロードしたシーンに切り替える
            yield return new WaitUntil(NextScene);
        }
    

        if (FadeOut)
        {
            FadeOut.SetActive(true);
        }
        yield return new WaitForSeconds(1.4f);
        SoundManager.Instance.StopBGM();
        LoadAsync.allowSceneActivation = true;

    }
}


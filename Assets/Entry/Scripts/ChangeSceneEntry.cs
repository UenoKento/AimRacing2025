using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneEntry : MonoBehaviour
{
    public string SceneName = "";
    private bool NextScene() { return Next; }   //�A�����[�h�������ǂ������m���߂�֐�
    private bool Next;
    [Header("�ڕW�̃X�N���v�g���I������玟�̃V�[���Ɉڍs")]
    public ATMTWindow Script;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadScene(SceneName));
    }
    IEnumerator LoadScene(string name)
    {
        //�ǂݍ��ރV�[��
        AsyncOperation LoadAsync = SceneManager.LoadSceneAsync(name);
        LoadAsync.allowSceneActivation = false;
        //NextScene��true�ɂȂ����烍�[�h�����V�[���ɐ؂�ւ���
        yield return new WaitUntil(NextScene);
        LoadAsync.allowSceneActivation = true;
        yield return null;
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/*
 * ���[�h����V�[����ǂݍ��ނ��߂̃N���X
 */
public class LoadScene : MonoBehaviour
{
    [SerializeField]    // ���[�h����V�[����
    private string m_sceneName;

    [SerializeField]    // �i���󋵂�UI
    private GameObject loadingUI;

    private AsyncOperation m_async; // �i���󋵂̊Ǘ�

    /// <summary>
    /// ���[�h���J�n���郁�\�b�h
    /// </summary>
    public void StartLoad()
    {
        StartCoroutine(Load());
    }

    /// <summary>
    /// ���[�h�����s���郁�\�b�h
    /// </summary>
    /// <returns></returns>
    private IEnumerator Load()
    {
        // UI��\������
        loadingUI.SetActive(true);

        // �V�[����񓯊��Ń��[�h����
        m_async = SceneManager.LoadSceneAsync(m_sceneName);

        // ���[�h����������܂őҋ@����
        while (!m_async.isDone)
        {
            yield return null;
        }

        // UI���\������
        loadingUI.SetActive(false);
    }
}

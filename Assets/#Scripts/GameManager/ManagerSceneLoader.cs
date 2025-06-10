/**
 * @file    ManagerSceneLoader.cs
 * @brief   Manager�p�̃V�[����ǂݍ��ނ��߂̋@�\������
 * @author  22CU0219 ��ؗF��
 * @date    2024/09/01  �쐬
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
        // �X�e�[�g�̐ݒ肪�I����Ă���Ώ����𔲂���
        if (didSetState) return;

        //���[�h�ς݂̃V�[���ł���΁A���O�ŕʃV�[�����擾�ł���
        Scene scene = SceneManager.GetSceneByName(m_managerSceneName);
        if(scene.isLoaded)
        {
            GameManager.Instance.ChangeState(m_firstState);
            didSetState = true;
        }
    }
}

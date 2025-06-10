/*------------------------------------------------------------------
* �t�@�C�����FDebugKey
* �T�v�FDebugKey�̎���
* �S���ҁF���ї��P
* �쐬���F08/04
* 
-------------------------------------------------------------------*/
//�X�V����
/*
* 2022/08/04 ���ї��P�@F1�`F5�̃f�o�b�O�L�[�̐ݒ�
*/
//-----------------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugKey : MonoBehaviour
{
    //�|�[�Y���Ă��邩�ǂ����̃t���O
    bool Pause;

    // Start is called before the first frame update
    void Start()
    {
        
    }
   

    //Pause����������ϐ�
    private void NotPause()
    {
        Pause = false;
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        //Escape�����Ƌ����I�����ăG�N�X�v���[���[���J��
        //�̂���exe�̏ꏊ�����߂�exe�̏ꏊ���J���悤�ɂ���B
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //System.Diagnostics.Process.Start("explorer.exe", Application.dataPath);
            Application.Quit();
        }
        //F1�������ƃ^�C�g���Ɉړ�����B
        if (Input.GetKeyDown(KeyCode.F1))
        {
            SoundManager.Instance.StopBGM();
            GameManager.Instance.ReLoadingScene("Splash");
        }
        //F2�������ƃC���Q�[���ɔ��
        if (Input.GetKeyDown(KeyCode.F2))
        {
            SoundManager.Instance.StopBGM();
            GameManager.Instance.ReLoadingScene("Map");
        }
#if UNITY_EDITOR
        //F3�������ƃG�f�B�^���ꎞ��~����B
        if (Input.GetKeyDown(KeyCode.F3))
        {
            UnityEditor.EditorApplication.isPaused = true;
        }
#endif
        //F4�������ƈꎞ��~����B
        if (Input.GetKeyDown(KeyCode.F4))
        {
            Pause = !Pause;
            if (Pause)
            {
                SoundManager.Instance.MuteBGM();
                Time.timeScale = 0;
            }
            if (!Pause)
            {
                SoundManager.Instance.ResumeBGM();
                Time.timeScale = 1;
            }
        }

        //F4��������
        if (Input.GetKeyDown(KeyCode.F4))
        {
            
        }

        //F5������
        if (Input.GetKeyDown(KeyCode.F5))
        {
            
        }
      


        // F6�������ƃG�f�B�^��FPS��\������B
        if (Input.GetKeyDown(KeyCode.F6))
		{
            GameManager.Instance.ShowFPS = !GameManager.Instance.ShowFPS;
		}
        
        // F7��������SIMVR�̏�Ԃ�\������B
        if (Input.GetKeyDown(KeyCode.F7))
		{
            GameManager.Instance.ShowStateWIZMO = !GameManager.Instance.ShowStateWIZMO;
		}
        
        // F9�������ƃ}�E�X�J�[�\����\��/��\��
        if (Input.GetKeyDown(KeyCode.F9))
		{
            GameManager.Instance.ShowMouce = !GameManager.Instance.ShowMouce;
		}
        
        // F10�������ƃf�o�C�X��񋓕\������B
        if (Input.GetKeyDown(KeyCode.F10))
		{
            DebugDevice.Instance.ShowLogInGame = !DebugDevice.Instance.ShowLogInGame;
		}


	}
}
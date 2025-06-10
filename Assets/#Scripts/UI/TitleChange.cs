using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
/*------------------------------------------------------------------
* �t�@�C�����FTitleChange
* �T�v�F�^�C�g����ʂ̐؂�ւ��ƃG�t�F�N�g�̎���
* �S���ҁF���ї��P
* �쐬���F5/1
-------------------------------------------------------------------*/
//�X�V����
/*
* 2022/4/30�@���ї��P�@�摜�؂�ւ��̎���
* 2022/5/13�@���ї��P�@�摜�؂�ւ����̃G�t�F�N�g�̎���
* 2022/5/19�@���ї��P�@Unity���ŃV�[���J�ڐ�̖��O���w��ł���悤�ɐݒ�
* 2022/5/30�@���ї��P�@�^�C�g����ʂŐ�̃V�[�������[�h�ł���悤�ύX
*/
//-----------------------------------------------------------------------------------

public class TitleChange : MonoBehaviour
{
    //�ϐ��錾
    public Image MyImage;           //Unity��Image��ݒ肷�邽�߂̕ϐ�
    public Sprite SecondImage;      //�؂�ւ���摜��Unity�Őݒ肷�邽�߂̕ϐ�
    private float step_time;        //�b���ŉ摜��؂�ւ���p�̕ϐ�
    private float FadeAlpha = 0;    //�����x������ϐ�
    public string SceneName;        //���[�h����V�[����
    public string UnloadScene;      //�A�����[�h����V�[����
    private bool Unloaded = false;  //�A�����[�h���I�����Ă��邩�̃t���O
    private bool FadeIn;
    private bool FadeOut;

    private bool NextScene() { return Unloaded; }   //�A�����[�h�������ǂ������m���߂�֐�

    // �X�^�[�g�{�^��������������s�����
    void Start()
    {
        Cursor.visible = false;

        //������
        step_time = 0.0f;
       
        //�V�[���̃��[�h
        if (SceneName != null)
        {
            StartCoroutine(LoadScene(SceneName)); 
        }
    }

    //�V�[�����Ƀ��[�h����֐�
    IEnumerator LoadScene(string name)
    {
        //�ǂݍ��ރV�[��
        AsyncOperation LoadAsync=SceneManager.LoadSceneAsync(name);
        if (LoadAsync != null)
        {
            LoadAsync.allowSceneActivation = false;
        }
        //NextScene��true�ɂȂ����烍�[�h�����V�[���ɐ؂�ւ���
        yield return new WaitUntil(NextScene);
        if (LoadAsync != null)
        {
            LoadAsync.allowSceneActivation = true;
        }
        yield return null;
    }
    IEnumerator wait()
    {
        yield return new WaitForSeconds(1);
        FadeIn = true;
    }
    IEnumerator Change()
    {
		yield return new WaitForSeconds(0.5f);
		MyImage.sprite = SecondImage;
		FadeIn = false;
		FadeOut = true;
		FadeAlpha = 0;
	}
	// Update is called once per frame
	void Update()
    {
        //���l(�����x)�����̑��x�ŕς���
        //�摜�̐F�𔒂ɂ��ă��l��FadeAlpha�ŊǗ�
        MyImage.color = new Color(255, 255, 255, FadeAlpha);

        if (!FadeIn)
        {
            //�f���^�^�C����2����1�����l�ɑ����Ă���
            FadeAlpha += Time.deltaTime / 3.0f;
            //���l��0�ȉ��ɂȂ�����1�ɖ߂��B
            if (FadeAlpha >= 1)
            {
                StartCoroutine(wait());
            }
        }
        else
        {
            //�f���^�^�C����2����1�����l��������Ă���
            FadeAlpha -= Time.deltaTime ;

			//���l��0�ȉ��ɂȂ�����1�ɖ߂��B
			//�t�F�[�h�A�E�g������摜��؂�ւ���
			if (!FadeOut)
            {
                if (FadeAlpha <= 0)
                {
                    
					StartCoroutine(Change());
                }
            }
            else if (FadeIn && FadeAlpha <= 0)
            {
                FadeAlpha = 10.0f;
            }
        }
        
        //�Q�[�����̎��Ԃ���
        step_time += Time.deltaTime;

        

        //�񖇖ڂ̉摜���t�F�[�h�A�E�g������V�[����؂�ւ���
        if (FadeAlpha >=10.0f)
        {
            Unloaded = true;
            SceneManager.UnloadSceneAsync(UnloadScene);
        }

    }
}

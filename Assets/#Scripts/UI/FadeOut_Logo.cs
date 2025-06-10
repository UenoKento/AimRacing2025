using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
/*------------------------------------------------------------------
* �t�@�C�����FFadeOut_Logo
* �T�v�F���O���͉�ʂ̃t�F�[�h�A�E�g�̎���
* �S���ҁF���ї��P
* �쐬���F5/16
-------------------------------------------------------------------*/
//�X�V����
/*
* 2022/5/16 ���ї��P�@Logo�̃t�F�[�h�A�E�g������
*/

//-----------------------------------------------------------------------------------

public class FadeOut_Logo : MonoBehaviour
{
    //�ϐ��錾
    public Image MyImage;           //Unity��Image��ݒ肷�邽�߂̕ϐ�
    private float FadeAlpha = 0;    //�����x������ϐ�
    private bool isFadeIn = false;
	private bool isFadeOut = false;
	public bool GetIsFade { get{ return isFadeOut; } }

    //public GarageSound GS;

    //�^�C�}�[
    public float timeCnt = 0.0f;

    // �X�^�[�g�{�^��������������s�����
    void Start()
    {
        //GS = GameObject.Find("GarageSound").GetComponent<GarageSound>();

        //������
        timeCnt = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        timeCnt += Time.deltaTime;

        //Fade��false�̏ꍇ�A���l(�����x)�����̑��x�ŕς���
        if (!isFadeIn)
        {
            //�摜�̐F�𔒂ɂ��ă��l��FadeAlpha�ŊǗ�
            MyImage.color = new Color(255, 255, 255, FadeAlpha);

            //�f���^�^�C����2����1�����l��������Ă���
            FadeAlpha += Time.deltaTime;

            //���l��0�ȉ��ɂȂ�����1�ɖ߂��B
            if (FadeAlpha >= 1)
            {
                isFadeIn = true;
                //GS.PlayWelcomAim();
            }
        }
        else
        {
            ////�摜�̐F�𔒂ɂ��ă��l��FadeAlpha�ŊǗ�
            //MyImage.color = new Color(255, 255, 255, FadeAlpha);

            ////�f���^�^�C����2����1�����l��������Ă���
            //FadeAlpha -= Time.deltaTime / 1.5f;

            ////���l��0�ȉ��ɂȂ�����1�ɖ߂��B
            //if (FadeAlpha <= 0)
            //{
            //	isFadeOut = true;
            //}
            if (timeCnt >= 2.5f)
            {
                //�摜�̐F�𔒂ɂ��ă��l��FadeAlpha�ŊǗ�
                //MyImage.color = new Color(255, 255, 255, 0);
                MyImage.enabled = false;
                isFadeOut = true;
            }
        }
    }
}

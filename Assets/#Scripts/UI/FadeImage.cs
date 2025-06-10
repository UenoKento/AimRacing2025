using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
/*------------------------------------------------------------------
* �t�@�C�����FFadeImage
* �T�v�FLoop�pFade�@�\�������̎���
* �S���ҁF�F�F�N
* �쐬���F08/17
-------------------------------------------------------------------*/
//�X�V����
/*
* 2022/05/16 ���ї��P�@Logo�̃t�F�[�h�A�E�g������
* 2022/08/17 �F�F�N�@Loop�pFade�@�\������
*/

//-----------------------------------------------------------------------------------

public class FadeImage : MonoBehaviour
{
    //�ϐ��錾
    [SerializeField] Image MyImage;           //Unity��Image��ݒ肷�邽�߂̕ϐ�
    [Header("�t�F�[�h�C���X�r�[�g�ݒ�")]
    [SerializeField] float FadeInSpeed;
    [Header("�t�F�[�h�A�E�g�X�r�[�g�ݒ�")]
    [SerializeField] float FadeOutSpeed;
    [Header("���݂̃A���t�@�l")]
    [SerializeField] private float FadeAlpha = 0;    //�����x������ϐ�
    [Header("�A���t�@�l�̐ݒ�")]
    public float Alpha = 0;
    private bool isFade = false;
    // �X�^�[�g�{�^��������������s�����
    void Start()
    {
        FadeAlpha = Alpha;
    }

    // Update is called once per frame
    void Update()
    {
        //Fade��false�̏ꍇ�A���l(�����x)�����̑��x�ŕς���
        if ((!isFade) && (FadeInSpeed > 0)) 
        {
            //�摜�̐F�𔒂ɂ��ă��l��FadeAlpha�ŊǗ�
            MyImage.color = new Color(255, 255, 255, FadeAlpha);

            //�f���^�^�C����2����1�����l��������Ă���
            FadeAlpha += Time.deltaTime / FadeInSpeed;

            //���l��0�ȉ��ɂȂ�����1�ɖ߂��B
            if (FadeAlpha >= 1)
            {
                isFade = true;
            }
        }
        if ((isFade) && (FadeOutSpeed > 0))
        {
			//�摜�̐F�𔒂ɂ��ă��l��FadeAlpha�ŊǗ�
			MyImage.color = new Color(255, 255, 255, FadeAlpha);

			//�f���^�^�C����2����1�����l��������Ă���
			FadeAlpha -= Time.deltaTime / FadeOutSpeed;

			//���l��0�ȉ��ɂȂ�����1�ɖ߂��B
			if (FadeAlpha <= 0)
			{
                isFade = false;
			}
		}
    }
}

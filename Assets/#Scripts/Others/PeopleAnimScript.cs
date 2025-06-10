/*
 * �t�@�C�����FPeopleAnimScript.cs
 * ���e      �F�ϋq�A�j���[�V�����R���g���[���[
 * �쐬��    �F20CU0302����V��
 * ����      �F2022/09/02
 * �X�V      �F
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleAnimScript : MonoBehaviour
{
    //-----------�e�ϐ���`-------------
    private Animator visitorAnimator;
    //�A�j���[�V�����؂�ւ���J�E���^�[
    private int cont = 0;

    bool isClap = false;
    bool isCool = false;
    bool isCall = false;
    bool isChangeble = true;

    int waitTime = 0;

    //�����_����
    int randamNum = 0;
    //----------------------------------
    // Start is called before the first frame update
    void Start()
    {
        visitorAnimator = GetComponent<Animator>();
        //Animator�̊e�p�����[�^�Y��������
        visitorAnimator.SetBool("isClap", false);
        visitorAnimator.SetBool("isCool", false);
        visitorAnimator.SetBool("isCall", false);
        visitorAnimator.SetBool("isChangeble", true);


        //�n�߂����A�ϋq�A�j���[�V�����������_���ɂ��邽��
        randamNum = Random.Range(0, 3);
        //�A�j���[�V�������Ԃ������_���ɂ���
        cont = Random.Range(1, 10) * 100;
        //�A�j���[�V����������O�ɑ҂��ԁi������Object�����ɓ����A�j���[�V�������Ȃ��悤�Ɂj
        waitTime = Random.Range(0, 120);
    }

    // Update is called once per frame
    void Update()
    {
        if (waitTime > 0)
        {
            waitTime--;
            return;
        }
        //time_ += Time.deltaTime;
        selectAnimation(randamNum);
        
        if (!visitorAnimator.GetCurrentAnimatorStateInfo(0).IsTag("ClapMain") &&
            !visitorAnimator.GetCurrentAnimatorStateInfo(0).IsTag("CoolMain") &&
            !visitorAnimator.GetCurrentAnimatorStateInfo(0).IsTag("CallMain") &&
            cont == 0)
        {
            cont = Random.Range(2, 10) * 50;
            /*Debug.LogError(Time.time + "         " + cont);*/
        }

        if ((visitorAnimator.GetCurrentAnimatorStateInfo(0).IsTag("ClapMain") ||
            visitorAnimator.GetCurrentAnimatorStateInfo(0).IsTag("CoolMain") ||
            visitorAnimator.GetCurrentAnimatorStateInfo(0).IsTag("CallMain")) &&
            cont > 0)
        {
            isChangeble = false;
            cont--;
            if (cont == 0)
            {
                isChangeble = true;
                randamNum = Random.Range(0, 3);
            }
        }

        visitorAnimator.SetBool("goClap", isClap);
        visitorAnimator.SetBool("goCool", isCool);
        visitorAnimator.SetBool("goCall", isCall);
        visitorAnimator.SetBool("isChangeble", isChangeble);
    }

    //�A�j���[�V������I������֐�
    void selectAnimation(int Num)
    {
        if (Num == 0)
        {
            isClap = true;
            isCool = false;
            isCall = false;
        }
        if (Num == 1)
        {
            isClap = false;
            isCool = true;
            isCall = false;
        }
        if (Num == 2)
        {
            isClap = false;
            isCool = false;
            isCall = true;
        }
    }
}

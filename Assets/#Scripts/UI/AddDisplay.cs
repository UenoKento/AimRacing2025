using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddDisplay : MonoBehaviour
{
    // �菑���ōs���ꍇ�ɕύX���鏈��
    // MeterCamera ����ł��ύX��
    [SerializeField]
    private bool OleDisplay = false;

    public int CountDis = 0;

    // �J�n���ɍs������
    /*void Start()
    {
        // �菑���p
        if (OleDisplay == true)
        {
            // Display.displays[0] �͎�v�ȃf�t�H���g�̃f�B�X�v���C�ŁA��ɃI���ł��B�ł�����A�C���f�b�N�X 1 ����n�܂�܂��B
            // ���̑��̃f�B�X�v���C���g�p�\�����m�F���A���ꂼ����A�N�e�B�u�ɂ��܂��B
            if (Display.displays.Length > 1)
                Display.displays[1].Activate();
            if (Display.displays.Length > 2)
                Display.displays[2].Activate();
        }

        // �����Ŋm�F���Ă����p
        else
        {
            // �ڑ�����Ă���f�B�X�v���C���m�F���ĕ\���ł���悤�ɂ���B
            for (int i = 1; i < Display.displays.Length; i++)
            {
                Display.displays[i].Activate();
                CountDis = i;
            }
            //���j�^�[�𐔂�����ɐ����T�C�Y��ς���kitamura
            Screen.SetResolution(Screen.currentResolution.width * CountDis, Screen.currentResolution.height, false);
            *//*            // ��ʃT�C�Y�̎擾
                        Debug.Log("Screen currentResolution : " + Screen.currentResolution);*//*
        }
    }*/
}
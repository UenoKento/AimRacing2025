using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;

// �J�����ؑւ̃X�N���v�g
public class SwitchingCamera : MonoBehaviour
{
    // �C���X�y�N�^�Őݒ肵���A�J����(CinemachineVirtualCamera)��z��ɕۑ�����
    [SerializeField]
    List<CinemachineVirtualCamera> virtualCameras = new List<CinemachineVirtualCamera>();

    int currentIndex;   // �z��ԍ�

    void Start()
    {
        // ������
        currentIndex = 0;
        virtualCameras[currentIndex].Priority = 11;
    }

    void Update()
    {
        // �z�[���L�[�Ŕz��ɓ����Ă���J�������Ԃɐ؂�ւ�
        if(Input.GetKeyDown(KeyCode.Home))
        {
            virtualCameras[currentIndex].Priority = 10;

            currentIndex++;

            if (virtualCameras.Count <= currentIndex)
                currentIndex = 0;

            virtualCameras[currentIndex].Priority = 11;
        }


    }

}

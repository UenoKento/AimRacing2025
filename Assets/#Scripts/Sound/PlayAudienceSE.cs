using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudienceSE : MonoBehaviour
{

    //�v���C���[���S�[����ʂ����ۂɃt���O��true�ɂ���
    private void OnTriggerEnter(Collider collider)
    {
        //��������Collider�̃^�O��Player�Ȃ�t���O��true��
        if (collider.tag == "Player")
        {
            SoundManager.Instance.FadeIn3DSE(SoundManager.SE3D_Type.Audience,10);
        }
    }
}

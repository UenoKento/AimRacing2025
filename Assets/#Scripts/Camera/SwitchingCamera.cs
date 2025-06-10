using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;

// カメラ切替のスクリプト
public class SwitchingCamera : MonoBehaviour
{
    // インスペクタで設定した、カメラ(CinemachineVirtualCamera)を配列に保存する
    [SerializeField]
    List<CinemachineVirtualCamera> virtualCameras = new List<CinemachineVirtualCamera>();

    int currentIndex;   // 配列番号

    void Start()
    {
        // 初期化
        currentIndex = 0;
        virtualCameras[currentIndex].Priority = 11;
    }

    void Update()
    {
        // ホームキーで配列に入っているカメラ順番に切り替え
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

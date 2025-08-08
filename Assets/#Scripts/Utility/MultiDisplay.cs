// MultiDisplay.cs
// 2560x1440���j�^�[3��(1�̕`��̈�Ƃ݂Ȃ�)��1920x1080�~�j���j�^�[�ŉ�ʂ𕪊�����
// 23CU0307 ��쌒�l


using UnityEngine;

public class MultiDisplay : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height;

    void Start()
    {
        Display.displays[1].Activate(1920, 1080, 60);
        Screen.SetResolution(width, height, FullScreenMode.FullScreenWindow);
    }
}

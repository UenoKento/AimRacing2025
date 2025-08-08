// MultiDisplay.cs
// 2560x1440モニター3枚(1つの描画領域とみなす)と1920x1080ミニモニターで画面を分割する
// 23CU0307 上野健斗


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

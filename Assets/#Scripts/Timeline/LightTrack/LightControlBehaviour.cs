/**
 * @file    LightControlBehaviour.cs
 * @brief   ライトのクリップの動作（及びパラメーター）
 * @author  22CU0219　鈴木友也
 * @date    2024/08/23  作成 
 * 
 * 参考　https://tsubakit1.hateblo.jp/entry/2018/08/26/173345 
 */

using UnityEngine;
using UnityEngine.Playables;

public class LightControlBehaviour : PlayableBehaviour
{
    [SerializeField]
    Color m_color = Color.white;
    [SerializeField]
    float m_intensity = 1f;

    #region プロパティ
    public Color Color
    {
        get => m_color;
        set => m_color = value;
    }

    public float Intensity
    {
        get => m_intensity;
        set => m_intensity = value;
    }
    #endregion
}
/**
 * @file    LightControlBehaviour.cs
 * @brief   ���C�g�̃N���b�v�̓���i�y�уp�����[�^�[�j
 * @author  22CU0219�@��ؗF��
 * @date    2024/08/23  �쐬 
 * 
 * �Q�l�@https://tsubakit1.hateblo.jp/entry/2018/08/26/173345 
 */

using UnityEngine;
using UnityEngine.Playables;

public class LightControlBehaviour : PlayableBehaviour
{
    [SerializeField]
    Color m_color = Color.white;
    [SerializeField]
    float m_intensity = 1f;

    #region �v���p�e�B
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
/**
 * @file    MagicFormula.cs
 * @brief   �ȗ��Ń}�W�b�N�t�H�[�~�����Ȑ�
 * @author�@22cu0219 ��ؗF��
 * @date    24/06/07 �쐬
 *          24/08/04 �ŏI�X�V
 */

using UnityEngine;

[System.Serializable]
public class MagicFormula
{
    // �W��
    [SerializeField]
    float B_stiffness = 10f;  // �����W��
    [SerializeField]
    float C_shape = 1.9f;      // �`��W��
    [SerializeField]
    float D_peak = 1f;       // �s�[�N�l
    [SerializeField]
    float E_curvature = 1f;�@// �ȗ��W��

    const int m_peakSlipResolution = 1000;  // �s�[�N�X���b�v�l���v�Z����𑜓x
    [SerializeField,ShowInInspector]
    float m_peakSlipRatio;
    [SerializeField,ShowInInspector]
    float m_peakSlipAngle;

    #region �v���p�e�B
    public float PeakSlipRatio => m_peakSlipRatio;
    public float PeakSlipAngle => m_peakSlipAngle;
    #endregion

    public void Initialize()
    {
        CalcPeakSlipRatio();
        CalcPeakSlipAngle();
    }

    public float Evaluate(in float _slip)
    {
        var B = B_stiffness;
        var C = C_shape;
        var D = D_peak;
        var E = E_curvature;
        var x = _slip;
        return D * Mathf.Sin(C * Mathf.Atan(B * x - E * (B * x - Mathf.Atan(B * x))));
    }

    void CalcPeakSlipRatio()
    {
        float max = 0f;
        float calcCoeff = 1f / m_peakSlipResolution;

        // �X���b�v����0%�`100%�͈̔͂ŁA�ő�l�̃X���b�v�������߂�
        for(int i = 1; i <= m_peakSlipResolution; ++i)
        {
            float tmp = Evaluate(i * calcCoeff);
            if (max < tmp)
            {
                max = tmp;
                m_peakSlipRatio = i * calcCoeff;
            }
            else
            {
                m_peakSlipRatio = i * calcCoeff;
                break;
            }
        }

    }

    void CalcPeakSlipAngle()
    {
        float max = 0f;
        float calcCoeff = 90f / m_peakSlipResolution;

        // �X���b�v�p��0���`90���͈̔͂ŁA�ő�l�̃X���b�v�p�����߂�
        for (int i = 1; i <= m_peakSlipResolution; ++i)
        {
            float tmp = Evaluate(i * calcCoeff);
            if (max < tmp)
            {
                max = tmp;
                m_peakSlipAngle = i * calcCoeff;
            }
            else
            {
                m_peakSlipAngle = i * calcCoeff;
                break;
            }
        }
    }
}

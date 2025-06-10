/**
 * @file    LightControlMixerBehaviour.cs
 * @brief   ���C�g���N���b�v�S�̂��擾���Đ��䂷�鏈��
 * @author  22CU0219�@��ؗF��
 * @date    2024/08/23  �쐬 
 * 
 * �Q�l�@https://tsubakit1.hateblo.jp/entry/2018/08/26/173345
 */

using UnityEngine;
using UnityEngine.Playables;

public class LightControlMixerBehaviour : PlayableBehaviour
{
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        Light trackBinding = playerData as Light;
        float finalIntensity = 0f;
        Color finalColor = Color.black;

        if (!trackBinding)
            return;

        int inputCount = playable.GetInputCount(); // ���̃g���b�N�̑S�ẴN���b�v�̐����擾

        for (int i = 0; i < inputCount; i++)
        {
            float inputWeight = playable.GetInputWeight(i);
            ScriptPlayable<LightControlBehaviour> inputPlayable = (ScriptPlayable<LightControlBehaviour>)playable.GetInput(i);
            LightControlBehaviour input = inputPlayable.GetBehaviour();

            // ��L�̕ϐ����g�p���āA�e�t���[������������B
            finalIntensity += input.Intensity * inputWeight;
            finalColor += input.Color * inputWeight;
        }

        // �Ō�Ƀo�C���h����Ă��郉�C�g�̃v���p�e�B�ɓK�p����
        trackBinding.intensity = finalIntensity;
        trackBinding.color = finalColor;
    }
}

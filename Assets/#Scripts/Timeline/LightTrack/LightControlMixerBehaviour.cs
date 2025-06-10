/**
 * @file    LightControlMixerBehaviour.cs
 * @brief   ライトをクリップ全体を取得して制御する処理
 * @author  22CU0219　鈴木友也
 * @date    2024/08/23  作成 
 * 
 * 参考　https://tsubakit1.hateblo.jp/entry/2018/08/26/173345
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

        int inputCount = playable.GetInputCount(); // このトラックの全てのクリップの数を取得

        for (int i = 0; i < inputCount; i++)
        {
            float inputWeight = playable.GetInputWeight(i);
            ScriptPlayable<LightControlBehaviour> inputPlayable = (ScriptPlayable<LightControlBehaviour>)playable.GetInput(i);
            LightControlBehaviour input = inputPlayable.GetBehaviour();

            // 上記の変数を使用して、各フレームを処理する。
            finalIntensity += input.Intensity * inputWeight;
            finalColor += input.Color * inputWeight;
        }

        // 最後にバインドされているライトのプロパティに適用する
        trackBinding.intensity = finalIntensity;
        trackBinding.color = finalColor;
    }
}

/**
 * @file    LightControlAsset.cs
 * @brief   ���C�g�̃N���b�v�̃A�Z�b�g���
 * @author  22CU0219�@��ؗF��
 * @date    2024/08/23  �쐬 
 * 
 * �Q�l�@https://tsubakit1.hateblo.jp/entry/2018/08/26/173345
 */


using UnityEngine;
using UnityEngine.Playables;

public class LightControlClip : PlayableAsset
{
    [SerializeField]
    Color color = Color.white;
    [SerializeField]
    float intensity = 1.0f;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<LightControlBehaviour>.Create(graph);

        var lightControlBehaviour = playable.GetBehaviour();
        lightControlBehaviour.Color = color;
        lightControlBehaviour.Intensity = intensity;

        return playable;
    }
}
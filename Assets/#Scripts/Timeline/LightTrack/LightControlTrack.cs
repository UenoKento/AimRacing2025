/**
 * @file    LightControlTrack.cs
 * @brief   ���C�g�̃g���b�N�̃A�Z�b�g���
 * @author  22CU0219�@��ؗF��
 * @date    2024/08/23  �쐬 
 * 
 * �Q�l�@https://tsubakit1.hateblo.jp/entry/2018/08/26/173345
 */


using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

[TrackClipType(typeof(LightControlClip))]
[TrackBindingType(typeof(Light))]
public class LightControlTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<LightControlMixerBehaviour>.Create(graph, inputCount);
    }

    /// <summary>
    /// �v���r���[�Đ��������ɃI�u�W�F�N�g�̒l��ύX���Ȃ��悤�ɂ���
    /// </summary>
    public override void GatherProperties(PlayableDirector director, IPropertyCollector driver)
    {
#if UNITY_EDITOR
        var light = director.GetGenericBinding(this) as Light;

        // ���C�g�I�u�W�F�N�g�̃v���p�e�B��ێ�
        // �V���A���C�Y���ꂽ�v���p�e�B����n��(.unity�t�@�C���̒��g������)
        driver.AddFromName<Light>(light.gameObject, "m_Color");�@
        driver.AddFromName<Light>(light.gameObject, "m_Intensity");
#endif
        base.GatherProperties(director, driver);
    }
}

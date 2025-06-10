/**
 * @file    LightControlTrack.cs
 * @brief   ライトのトラックのアセット情報
 * @author  22CU0219　鈴木友也
 * @date    2024/08/23  作成 
 * 
 * 参考　https://tsubakit1.hateblo.jp/entry/2018/08/26/173345
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
    /// プレビュー再生した時にオブジェクトの値を変更しないようにする
    /// </summary>
    public override void GatherProperties(PlayableDirector director, IPropertyCollector driver)
    {
#if UNITY_EDITOR
        var light = director.GetGenericBinding(this) as Light;

        // ライトオブジェクトのプロパティを保持
        // シリアライズされたプロパティ名を渡す(.unityファイルの中身を見る)
        driver.AddFromName<Light>(light.gameObject, "m_Color");　
        driver.AddFromName<Light>(light.gameObject, "m_Intensity");
#endif
        base.GatherProperties(director, driver);
    }
}

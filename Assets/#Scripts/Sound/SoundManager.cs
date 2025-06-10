/*------------------------------------------------------------------
* ファイル名：SoundManager.cs
* 概　　　要：サウンドの管理を行う
* 担　当　者：20CU0213　小林大輝
* 作　成　日：2022/08/29
-------------------------------------------------------------------*/
//更新履歴
/*
* 2022/08/29　小林大輝　サウンド管理用の処理を追加
* 2022/09/01  小林大輝　フェード処理を追加
* 2022/09/28  小林大輝  使用するクリップを保持しどこからでもアクセス可能に
* 2022/10/05　小林大輝　SEのフェードを追加
*/

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using DG.Tweening;
using CUEngine.Pattern;

public class SoundManager : SingletonMono<SoundManager>
{
    //BGM用enum
    public enum BGM_Type
    {
        //BGM用
        OP,             //タイトル時のOP
        Entry,          //名前入力、ATMT選択中
        InGame,         //インゲーム中
        Result          //リザルト
    }

    //SE用enum
    public enum SE_Type
    {
        //SE用
        OPPushSE,       //OP時入力用のSE
        GlitchSE,       //グリッチ用SE   
        Welcome,        //AIMの世界へようこそ
        GuideName,      //名前入力をしてください
        GuideATMT,      //トランスミッションを選択してください
        CursorSE,       //カーソルを動かしたときのSE
        PushGasPedal,   //アクセルペダルを踏んでください
        EntryPushSE,    //エントリー時にアクセルを踏み込んだ時のSE
        DoonSE,         //文字やロゴを表示する際のSE
        AT,             //オートマを選択
        MT,             //マニュアルを選択
        Three,          //3
        Two,            //2
        One,            //1
        GO,             //Go
        CountSignal,    //インゲーム開始時のカウント用SE
        GOSignal,       //インゲーム開始時のスタート用SE
        Goal,           //ゴール
        LapSignal,      //区間タイム表示用SE
        Easy_L,         //緩めの左カーブ
        Turn_L,         //左カーブ
        Acute_L,        //急な左カーブ
        Easy_R,         //緩めの右カーブ
        Turn_R,         //右カーブ
        Acute_R,        //急な右カーブ
    }

    //3DSE用enum
    public enum SE3D_Type
    {
        //3DSE用
        Audience        //観客のSE
    }

    //フェード
    private int IsFadeBGMType = 0;              //フェード用フラグ（0:none,1:in,2:out,3:cross）
    private int IsFadeSEType = 0;               //フェード用フラグ（0:none,1:in,2:out）
    private int IsFadeSE3DType = 0;             //フェード用フラグ（0:none,1:in,2:out）
    public float FADE_IN_TIME = 1.0f;           //フェードインのみ
    public float FADE_OUT_TIME = 1.0f;          //フェードアウトのみ
    public const float CROSS_FADE_TIME = 1.0f;  //クロスフェード時

    //ボリューム関連
    [Range(0, 1)] public float BGM_Volume = 0.9f;
    [Range(0, 1)] public float SE_Volume = 1f;
    [Range(0, 1)] public float SE3D_Volume = 1f;

    public bool Mute = false;

    //音データ管理用
    [SerializeField, EnumIndex(typeof(BGM_Type))] public AudioClip[] BGM_Clips;
    [SerializeField, EnumIndex(typeof(SE_Type))] public AudioClip[] SE_Clips;
    [SerializeField, EnumIndex(typeof(SE3D_Type))] public AudioClip[] SE3D_Clips;

    //mixer設定用
    public AudioMixerGroup BGMAudioMixer;
    public AudioMixerGroup SEAudioMixer;
    public AudioMixerGroup SE3DAudioMixer;

    //AudioSource
    private AudioSource[] BGMSource = new AudioSource[2];       //BGM再生用のAudioSource
    private AudioSource[] SESource = new AudioSource[5];        //SE再生用のAudioSource(複数個用意)
    private AudioSource[] SE3DSource = new AudioSource[3];      //3DSE再生用のAudioSource

    private int currentBGMIndex = 999;  //現在のBGMIndex

    private IEnumerator BGMEvents;
    private IEnumerator SEEvents;
    private IEnumerator SE3DEvents;


    private static bool nowSound;

    //開始時の処理
    protected override void Awake()
    {
        //基クラスのAwakeを呼び出し
        base.Awake();

        //オブジェクトに使用しているか確認する
        GameObject soundManager = CheckOtherSoundManager();
        bool checkResult = soundManager != null && soundManager != gameObject;
        if (checkResult) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        //使用する分のAudioSourceを作成
        for (int i = 0; i < BGMSource.Length; ++i)
        {
            BGMSource[i] = gameObject.AddComponent<AudioSource>();
            BGMSource[i].outputAudioMixerGroup = BGMAudioMixer;
        }

        for (int i = 0; i < SESource.Length; ++i)
        {
            SESource[i] = gameObject.AddComponent<AudioSource>();
            SESource[i].outputAudioMixerGroup = SEAudioMixer;
        }

        for (int i = 0; i < SE3DSource.Length; ++i)
        {
            SE3DSource[i] = gameObject.AddComponent<AudioSource>();
            SE3DSource[i].spatialBlend = 1;     //3Dサウンドに変更
            SE3DSource[i].outputAudioMixerGroup = SE3DAudioMixer;
        }
    }

    //毎フレーム更新
    // Update is called once per frame
    void Update()
    {
        //ボリューム設定
        if (IsFadeBGMType == 0)
        {
            BGMSource[0].volume = BGM_Volume;
            BGMSource[1].volume = BGM_Volume;
        }

        foreach (AudioSource source in SESource)
        {
            if (IsFadeSEType == 0)
            {
                source.volume = SE_Volume;
            }
        }

        foreach (AudioSource source in SE3DSource)
        {
            if (IsFadeSE3DType == 0)
            {
                source.volume = SE3D_Volume;
            }
        }

        if(BGMSource[0].clip == null)
        {
           //Debug.Log("BGMがありません");
        }

        nowSound = (IsFadeBGMType == 0);
    }

    //BGMの処理
    /// <summary>
    /// BGM再生
    /// </summary>
    /// <returns></returns>
    /// <param name="bgmType"></param>
    /// <param name="loopFlg"></param>
    /// <param name="Vol"></param>
    public void PlayBGM(BGM_Type bgmType, bool loopFlg = true, float Vol = 1)
    {
        //BGMなし状態
        if ((int)bgmType == 999)
        {
            StopBGM();
            return;
        }

        IsFadeBGMType = 0;          //フェードはなし
        int index = (int)bgmType;   //列挙型を整数型に
        currentBGMIndex = index;    //指定されたインデックスを現在のインデックスに適用
        BGM_Volume = Vol;           //ボリュームを設定

        //インデックスが0より下、または設定されたクリップ以上の場合処理しない
        if (index < 0 || BGM_Clips.Length <= index) return;

        //BGMが同じときは何もしない
        if (BGMSource[0].clip != null && BGMSource[0].clip == BGM_Clips[index]) return;
        else if (BGMSource[1].clip != null && BGMSource[1].clip == BGM_Clips[index]) return;

        //フェードでBGM開始しない
        if (BGMSource[0].clip == null && BGMSource[1].clip == null)
        {
            BGMSource[0].loop = loopFlg;
            BGMSource[0].clip = BGM_Clips[index];
            BGMSource[0].Play();
        }
        else
        {
            //クロスフェード処理
            CrossFadeChangeBGM(index, loopFlg);
        }
    }

    /// <summary>
    /// BGMのフェードイン処理
    /// </summary>
    /// <returns></returns>
    /// <param name="bgmType">AudioClipの番号</param>
    /// <param name="fadeTime">フェードにかかる時間</param>
    /// <param name="Vol">音量</param>
    /// <param name="loopFlg">ループ設定。ループしない場合だけfalse指定</param>

    public void FadeInBGM(BGM_Type bgmType, float fadeTime = 1, float Vol = 1, bool loopFlg = true)
    {
        if (IsFadeBGMType != 1)
        {
            IsFadeBGMType = 1;
            int index = (int)bgmType;
            currentBGMIndex = index;
            BGM_Volume = Vol;
            FADE_IN_TIME = fadeTime;

            if (BGMSource[0].clip == null && BGMSource[1].clip == null)
            {
                BGMSource[0].volume = 0;                                            //音量を0に
                BGMSource[0].clip = BGM_Clips[index];                               //クリップを設定
                BGMSource[0].loop = loopFlg;                                        //ループを設定
                BGMSource[0].Play();                                                //再生
                BGMSource[0].DOFade(endValue: BGM_Volume, duration: FADE_IN_TIME);  //フェードを掛ける

                //フェード時間まで処理を待つ 
                BGMEvents = DelayProcess(FADE_IN_TIME, () => 
                {
                    BGMEvents = null;
                    IsFadeBGMType = 0;                                              //フェードタイプを0に
                    return;
                });                                
                StartCoroutine(BGMEvents);
            }
        }
    }

    /// <summary>
    /// BGMのフェードアウト処理
    /// </summary>
    /// <returns></returns>
    /// <param name="fadeTime">フェードにかかる時間</param>
    /// <returns></returns>
    public void FadeOutBGM(float fadeTime)
    {
        if (IsFadeBGMType != 2)
        {
            IsFadeBGMType = 2;
            FADE_OUT_TIME = fadeTime;

            //[0]で再生中
            if (BGMSource[0].clip != null)
            {
                BGMSource[0].DOFade(0, FADE_OUT_TIME).SetEase(Ease.Linear);         //[0]をフェード

                BGMEvents = DelayProcess(FADE_OUT_TIME, () =>                       //フェード時間まで処理を待つ
                {
                    BGMSource[0].Stop();                                            //[0]を停止
                    BGMSource[0].clip = null;                                       //[0]のclipをnullに
                    BGMEvents = null;
                    IsFadeBGMType = 0;                                              //フェードのフラグを切る
                    return;
                });
                StartCoroutine(BGMEvents);
            }
            //[1]で再生中
            else if (BGMSource[1].clip != null)
            {
                BGMSource[1].DOFade(0, FADE_OUT_TIME).SetEase(Ease.Linear);         //[1]をフェード

                BGMEvents = DelayProcess(FADE_OUT_TIME, () =>                       //フェード時間まで処理を待つ
                {
                    BGMSource[1].Stop();                                            //[1]を停止
                    BGMSource[1].clip = null;                                       //[1]のclipをnullに
                    BGMEvents = null;
                    IsFadeBGMType = 0;                                              //フェードのフラグを切る
                    return;
                });
                StartCoroutine(BGMEvents);
            }
        }
    }

    /// <summary>
    /// BGMのクロスフェード処理
    /// </summary>
    /// <returns></returns>
    /// <param name="index">AudioClipの番号</param>
    /// <param name="loopFlg">ループ設定。ループしない場合だけfalse指定</param>
    /// <returns></returns>
    private void CrossFadeChangeBGM(int index, bool loopFlg)
    {
        IsFadeBGMType = 3;     //クロスフェードを適用
        if (BGMSource[0].clip != null)
        {
            //[0]が再生されている場合、[0]の音量を徐々に下げて、[1]を新しい曲として再生
            BGMSource[1].volume = 0;                                        //[1]の音量を0に
            BGMSource[1].clip = BGM_Clips[index];                           //[1]にclipを設定
            BGMSource[1].loop = loopFlg;                                    //[1]のloopを設定
            BGMSource[1].Play();                                            //[1]を再生
            BGMSource[0].DOFade(0, CROSS_FADE_TIME).SetEase(Ease.Linear);   //[0]をフェードさせる

            BGMEvents = DelayProcess(CROSS_FADE_TIME, () =>                 //フェード時間まで処理を待つ
            {
                BGMSource[0].Stop();                                        //[0]を停止
                BGMSource[0].clip = null;                                   //[0]のclipをnullに
                BGMEvents = null;
                IsFadeBGMType = 0;                                          //フェードのフラグを切る
                return;
            });
            StartCoroutine(BGMEvents);
        }
        else
        {
            //[1]が再生されている場合、[1]の音量を徐々に下げて、[0]を新しい曲として再生
            BGMSource[0].volume = 0;                                        //[0]の音量を0に
            BGMSource[0].clip = BGM_Clips[index];                           //[0]にclipを設定
            BGMSource[0].loop = loopFlg;                                    //[0]のloopを設定
            BGMSource[0].Play();                                            //[0]を再生
            BGMSource[1].DOFade(0, CROSS_FADE_TIME).SetEase(Ease.Linear);   //[1]をフェードさせる

            BGMEvents = DelayProcess(CROSS_FADE_TIME, () =>                 //フェード時間まで処理を待つ
            {
                BGMSource[1].Stop();                                        //[1]を停止
                BGMSource[1].clip = null;                                   //[1]のclipをnullに
                BGMEvents = null;
                IsFadeBGMType = 0;                                          //フェードのフラグを切る
                return;
            });
            StartCoroutine(BGMEvents);
        }
    }

    /// <summary>
    /// BGM完全停止
    /// </summary>
    public void StopBGM()
    {
        if(BGMSource[0].clip != null)
        {
            BGMSource[0].Stop();
            BGMSource[0].clip = null;
        }
        if(BGMSource[1].clip != null)
        {
            BGMSource[1].Stop();
            BGMSource[1].clip = null;
        }
        if(BGMEvents != null) StopCoroutine(BGMEvents);
        return;
    }

    /// <summary>
    /// BGM一時停止
    /// </summary>
    public void MuteBGM()
    {
        BGMSource[0].Stop();
        BGMSource[1].Stop();
    }

    /// <summary>
    /// 一時停止したBGMを再生（再開）
    /// </summary>
    public void ResumeBGM()
    {
        BGMSource[0].Play();
        BGMSource[1].Play();
    }

    //SEの処理
    /// <summary>
    /// SE再生
    /// </summary>
    /// <param name="seType"></param>
    public void PlaySE(SE_Type seType, float Vol = 1)
    {
        int index = (int)seType;
        SE_Volume = Vol;
        if (index < 0 || SE_Clips.Length <= index) return;

        //再生中ではないAudioSourceを使ってSEを鳴らす。
        foreach (AudioSource source in SESource)
        {
            if (false == source.isPlaying)
            {
                source.clip = SE_Clips[index];
                source.Play();
                return;
            }
        }
    }

    /// <summary>
    /// SEフェードイン処理
    /// </summary>
    /// <param name="seType"></param>
    /// <param name="Vol"></param>
    /// <param name="loopFlg"></param>
    public void FadeInSE(SE_Type seType, float fadeTime, float Vol = 1, bool loopFlg = true)
    {
        if (IsFadeSEType != 1)
        {
            IsFadeSEType = 1;
            int index = (int)seType;
            SE_Volume = Vol;
            FADE_IN_TIME = fadeTime;
            if (index < 0 || SE_Clips.Length <= index) return;

            //再生中ではないAudioSourceを使ってSEを鳴らす。
            foreach (AudioSource source in SESource)
            {
                if (false == source.isPlaying)
                {
                    source.volume = 0;                                            //音量を0に
                    source.clip = SE_Clips[index];                                //クリップを設定
                    source.loop = loopFlg;                                        //ループを設定
                    source.Play();                                                //再生
                    source.DOFade(SE_Volume, FADE_IN_TIME).SetEase(Ease.Linear);  //フェードを掛ける

                    SEEvents = DelayProcess(FADE_IN_TIME, () =>                   //フェード時間まで処理を待つ
                    {
                        SEEvents = null;
                        IsFadeSEType = 0;
                    });
                    StartCoroutine(SEEvents);
                }
            }
        }
    }

    /// <summary>
    /// SEフェードアウト処理
    /// </summary>
    /// <param name="fadeTime"></param>
    public void FadeOutSE(float fadeTime)
    {
        if (IsFadeSEType != 2)
        {
            IsFadeSEType = 2;

            foreach (AudioSource source in SESource)
            {
                if (true == source.isPlaying && true == source.loop)
                {
                    source.DOFade(0, FADE_OUT_TIME).SetEase(Ease.Linear);       //SEをフェード

                    SEEvents = DelayProcess(FADE_OUT_TIME, () =>                //フェード時間まで処理を待つ
                    {
                        source.Stop();                                          //SEを停止
                        source.clip = null;                                     //SEのclipをnullに
                        source.loop = false;                                    //SEのloopをfalseに
                        SEEvents = null;
                        IsFadeSEType = 0;                                       //フェードのフラグを切る
                    });
                    StartCoroutine(SEEvents);
                }
            }
        }
    }

    /// <summary>
    /// SE停止
    /// </summary>
    public void StopSE()
    {
        //すべてのSE用AudioSourceを停止
        foreach (AudioSource source in SESource)
        {
            source.Stop();
            source.clip = null;
            StopCoroutine(SEEvents);
        }
    }

    /// <summary>
    /// 3DSE再生
    /// </summary>
    /// <param name="se3DType"></param>
    /// <param name="Vol"></param>
    /// <param name="loopFlg"></param>
    public void Play3DSE(SE3D_Type se3DType, float Vol = 1, bool loopFlg = true)
    {
        int index = (int)se3DType;
        SE_Volume = Vol;
        if (index < 0 || SE3D_Clips.Length <= index) return;

        //再生中ではないAudioSourceを使ってSEを鳴らす。
        foreach (AudioSource source in SE3DSource)
        {
            if (false == source.isPlaying)
            {
                source.clip = SE3D_Clips[index];
                source.Play();
                return;
            }
        }
    }

    /// <summary>
    /// 3DSEフェードイン
    /// </summary>
    /// <param name="se3DType"></param>
    /// <param name="fadeTime"></param>
    /// <param name="Vol"></param>
    /// <param name="loopFlg"></param>
    public void FadeIn3DSE(SE3D_Type se3DType, float fadeTime, float Vol = 1, bool loopFlg = true)
    {
        if (IsFadeSE3DType != 1)
        {
            IsFadeSE3DType = 1;
            int index = (int)se3DType;
            SE3D_Volume = Vol;
            FADE_IN_TIME = fadeTime;
            if (index < 0 || SE3D_Clips.Length <= index) return;

            //再生中ではないAudioSourceを使って3DSEを鳴らす。
            foreach (AudioSource source in SE3DSource)
            {
                if (false == source.isPlaying)
                {
                    source.volume = 0;
                    source.clip = SE3D_Clips[index];
                    source.loop = loopFlg;
                    source.Play();
                    source.DOFade(endValue: SE3D_Volume, duration: FADE_IN_TIME).SetEase(Ease.Linear);

                    SE3DEvents = DelayProcess(FADE_IN_TIME, () =>             //フェード時間まで処理を待つ
                    {
                        SE3DEvents = null;
                        IsFadeSE3DType = 0;
                    });
                    StartCoroutine(SE3DEvents);
                }
            }
        }
    }

    /// <summary>
    /// SE3Dフェードアウト処理
    /// </summary>
    /// <param name="fadeTime"></param>
    public void FadeOut3DSE(float fadeTime)
    {
        if (IsFadeSE3DType != 2)
        {
            IsFadeSE3DType = 2;

            foreach (AudioSource source in SE3DSource)
            {
                if (true == source.isPlaying && true == source.loop)
                {
                    source.DOFade(0, FADE_OUT_TIME).SetEase(Ease.Linear);       //SEをフェード

                    SE3DEvents = DelayProcess(FADE_OUT_TIME, () =>              //フェード時間まで処理を待つ
                    {
                        source.Stop();                                          //SEを停止
                        source.clip = null;                                     //SEのclipをnullに
                        source.loop = false;                                    //SEのloopをfalseに
                        SE3DEvents = null;
                        IsFadeSEType = 0;                                       //フェードのフラグを切る
                    });
                    StartCoroutine(SE3DEvents);
                }
            }
        }
    }

    /// <summary>
    /// SE3D停止
    /// </summary>
    public void Stop3DSE()
    {
        //すべてのSE用AudioSourceを停止
        foreach (AudioSource source in SE3DSource)
        {
            source.Stop();
            source.clip = null;
            StopCoroutine(SE3DEvents);
        }
    }

    public void StopSoundCorutine()
    {
        StopCoroutine(BGMEvents);
        StopCoroutine(SEEvents);
        StopCoroutine(SE3DEvents);
    }

    //オブジェクトがあるか確認用
    private GameObject CheckOtherSoundManager()
    {
        return GameObject.FindGameObjectWithTag("SoundManager");
    }

    private IEnumerator DelayProcess(float delaySecond, UnityAction callback)
    {
        yield return new WaitForSeconds(delaySecond);
        callback?.Invoke();
    }

    private void OnDestroy()
    {
        ////サウンドの停止
        //SoundManager.Instance.StopBGM();
        //SoundManager.Instance.StopSE();
        //SoundManager.Instance.Stop3DSE();
    }

    //2023/0921河合追記
    public static bool NowSound()
    {
        return nowSound;
    }
}

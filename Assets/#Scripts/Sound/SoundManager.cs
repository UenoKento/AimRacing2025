/*------------------------------------------------------------------
* �t�@�C�����FSoundManager.cs
* �T�@�@�@�v�F�T�E���h�̊Ǘ����s��
* �S�@���@�ҁF20CU0213�@���ё�P
* ��@���@���F2022/08/29
-------------------------------------------------------------------*/
//�X�V����
/*
* 2022/08/29�@���ё�P�@�T�E���h�Ǘ��p�̏�����ǉ�
* 2022/09/01  ���ё�P�@�t�F�[�h������ǉ�
* 2022/09/28  ���ё�P  �g�p����N���b�v��ێ����ǂ�����ł��A�N�Z�X�\��
* 2022/10/05�@���ё�P�@SE�̃t�F�[�h��ǉ�
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
    //BGM�penum
    public enum BGM_Type
    {
        //BGM�p
        OP,             //�^�C�g������OP
        Entry,          //���O���́AATMT�I��
        InGame,         //�C���Q�[����
        Result          //���U���g
    }

    //SE�penum
    public enum SE_Type
    {
        //SE�p
        OPPushSE,       //OP�����͗p��SE
        GlitchSE,       //�O���b�`�pSE   
        Welcome,        //AIM�̐��E�ւ悤����
        GuideName,      //���O���͂����Ă�������
        GuideATMT,      //�g�����X�~�b�V������I�����Ă�������
        CursorSE,       //�J�[�\���𓮂������Ƃ���SE
        PushGasPedal,   //�A�N�Z���y�_���𓥂�ł�������
        EntryPushSE,    //�G���g���[���ɃA�N�Z���𓥂ݍ��񂾎���SE
        DoonSE,         //�����⃍�S��\������ۂ�SE
        AT,             //�I�[�g�}��I��
        MT,             //�}�j���A����I��
        Three,          //3
        Two,            //2
        One,            //1
        GO,             //Go
        CountSignal,    //�C���Q�[���J�n���̃J�E���g�pSE
        GOSignal,       //�C���Q�[���J�n���̃X�^�[�g�pSE
        Goal,           //�S�[��
        LapSignal,      //��ԃ^�C���\���pSE
        Easy_L,         //�ɂ߂̍��J�[�u
        Turn_L,         //���J�[�u
        Acute_L,        //�}�ȍ��J�[�u
        Easy_R,         //�ɂ߂̉E�J�[�u
        Turn_R,         //�E�J�[�u
        Acute_R,        //�}�ȉE�J�[�u
    }

    //3DSE�penum
    public enum SE3D_Type
    {
        //3DSE�p
        Audience        //�ϋq��SE
    }

    //�t�F�[�h
    private int IsFadeBGMType = 0;              //�t�F�[�h�p�t���O�i0:none,1:in,2:out,3:cross�j
    private int IsFadeSEType = 0;               //�t�F�[�h�p�t���O�i0:none,1:in,2:out�j
    private int IsFadeSE3DType = 0;             //�t�F�[�h�p�t���O�i0:none,1:in,2:out�j
    public float FADE_IN_TIME = 1.0f;           //�t�F�[�h�C���̂�
    public float FADE_OUT_TIME = 1.0f;          //�t�F�[�h�A�E�g�̂�
    public const float CROSS_FADE_TIME = 1.0f;  //�N���X�t�F�[�h��

    //�{�����[���֘A
    [Range(0, 1)] public float BGM_Volume = 0.9f;
    [Range(0, 1)] public float SE_Volume = 1f;
    [Range(0, 1)] public float SE3D_Volume = 1f;

    public bool Mute = false;

    //���f�[�^�Ǘ��p
    [SerializeField, EnumIndex(typeof(BGM_Type))] public AudioClip[] BGM_Clips;
    [SerializeField, EnumIndex(typeof(SE_Type))] public AudioClip[] SE_Clips;
    [SerializeField, EnumIndex(typeof(SE3D_Type))] public AudioClip[] SE3D_Clips;

    //mixer�ݒ�p
    public AudioMixerGroup BGMAudioMixer;
    public AudioMixerGroup SEAudioMixer;
    public AudioMixerGroup SE3DAudioMixer;

    //AudioSource
    private AudioSource[] BGMSource = new AudioSource[2];       //BGM�Đ��p��AudioSource
    private AudioSource[] SESource = new AudioSource[5];        //SE�Đ��p��AudioSource(�����p��)
    private AudioSource[] SE3DSource = new AudioSource[3];      //3DSE�Đ��p��AudioSource

    private int currentBGMIndex = 999;  //���݂�BGMIndex

    private IEnumerator BGMEvents;
    private IEnumerator SEEvents;
    private IEnumerator SE3DEvents;


    private static bool nowSound;

    //�J�n���̏���
    protected override void Awake()
    {
        //��N���X��Awake���Ăяo��
        base.Awake();

        //�I�u�W�F�N�g�Ɏg�p���Ă��邩�m�F����
        GameObject soundManager = CheckOtherSoundManager();
        bool checkResult = soundManager != null && soundManager != gameObject;
        if (checkResult) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        //�g�p���镪��AudioSource���쐬
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
            SE3DSource[i].spatialBlend = 1;     //3D�T�E���h�ɕύX
            SE3DSource[i].outputAudioMixerGroup = SE3DAudioMixer;
        }
    }

    //���t���[���X�V
    // Update is called once per frame
    void Update()
    {
        //�{�����[���ݒ�
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
           //Debug.Log("BGM������܂���");
        }

        nowSound = (IsFadeBGMType == 0);
    }

    //BGM�̏���
    /// <summary>
    /// BGM�Đ�
    /// </summary>
    /// <returns></returns>
    /// <param name="bgmType"></param>
    /// <param name="loopFlg"></param>
    /// <param name="Vol"></param>
    public void PlayBGM(BGM_Type bgmType, bool loopFlg = true, float Vol = 1)
    {
        //BGM�Ȃ����
        if ((int)bgmType == 999)
        {
            StopBGM();
            return;
        }

        IsFadeBGMType = 0;          //�t�F�[�h�͂Ȃ�
        int index = (int)bgmType;   //�񋓌^�𐮐��^��
        currentBGMIndex = index;    //�w�肳�ꂽ�C���f�b�N�X�����݂̃C���f�b�N�X�ɓK�p
        BGM_Volume = Vol;           //�{�����[����ݒ�

        //�C���f�b�N�X��0��艺�A�܂��͐ݒ肳�ꂽ�N���b�v�ȏ�̏ꍇ�������Ȃ�
        if (index < 0 || BGM_Clips.Length <= index) return;

        //BGM�������Ƃ��͉������Ȃ�
        if (BGMSource[0].clip != null && BGMSource[0].clip == BGM_Clips[index]) return;
        else if (BGMSource[1].clip != null && BGMSource[1].clip == BGM_Clips[index]) return;

        //�t�F�[�h��BGM�J�n���Ȃ�
        if (BGMSource[0].clip == null && BGMSource[1].clip == null)
        {
            BGMSource[0].loop = loopFlg;
            BGMSource[0].clip = BGM_Clips[index];
            BGMSource[0].Play();
        }
        else
        {
            //�N���X�t�F�[�h����
            CrossFadeChangeBGM(index, loopFlg);
        }
    }

    /// <summary>
    /// BGM�̃t�F�[�h�C������
    /// </summary>
    /// <returns></returns>
    /// <param name="bgmType">AudioClip�̔ԍ�</param>
    /// <param name="fadeTime">�t�F�[�h�ɂ����鎞��</param>
    /// <param name="Vol">����</param>
    /// <param name="loopFlg">���[�v�ݒ�B���[�v���Ȃ��ꍇ����false�w��</param>

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
                BGMSource[0].volume = 0;                                            //���ʂ�0��
                BGMSource[0].clip = BGM_Clips[index];                               //�N���b�v��ݒ�
                BGMSource[0].loop = loopFlg;                                        //���[�v��ݒ�
                BGMSource[0].Play();                                                //�Đ�
                BGMSource[0].DOFade(endValue: BGM_Volume, duration: FADE_IN_TIME);  //�t�F�[�h���|����

                //�t�F�[�h���Ԃ܂ŏ�����҂� 
                BGMEvents = DelayProcess(FADE_IN_TIME, () => 
                {
                    BGMEvents = null;
                    IsFadeBGMType = 0;                                              //�t�F�[�h�^�C�v��0��
                    return;
                });                                
                StartCoroutine(BGMEvents);
            }
        }
    }

    /// <summary>
    /// BGM�̃t�F�[�h�A�E�g����
    /// </summary>
    /// <returns></returns>
    /// <param name="fadeTime">�t�F�[�h�ɂ����鎞��</param>
    /// <returns></returns>
    public void FadeOutBGM(float fadeTime)
    {
        if (IsFadeBGMType != 2)
        {
            IsFadeBGMType = 2;
            FADE_OUT_TIME = fadeTime;

            //[0]�ōĐ���
            if (BGMSource[0].clip != null)
            {
                BGMSource[0].DOFade(0, FADE_OUT_TIME).SetEase(Ease.Linear);         //[0]���t�F�[�h

                BGMEvents = DelayProcess(FADE_OUT_TIME, () =>                       //�t�F�[�h���Ԃ܂ŏ�����҂�
                {
                    BGMSource[0].Stop();                                            //[0]���~
                    BGMSource[0].clip = null;                                       //[0]��clip��null��
                    BGMEvents = null;
                    IsFadeBGMType = 0;                                              //�t�F�[�h�̃t���O��؂�
                    return;
                });
                StartCoroutine(BGMEvents);
            }
            //[1]�ōĐ���
            else if (BGMSource[1].clip != null)
            {
                BGMSource[1].DOFade(0, FADE_OUT_TIME).SetEase(Ease.Linear);         //[1]���t�F�[�h

                BGMEvents = DelayProcess(FADE_OUT_TIME, () =>                       //�t�F�[�h���Ԃ܂ŏ�����҂�
                {
                    BGMSource[1].Stop();                                            //[1]���~
                    BGMSource[1].clip = null;                                       //[1]��clip��null��
                    BGMEvents = null;
                    IsFadeBGMType = 0;                                              //�t�F�[�h�̃t���O��؂�
                    return;
                });
                StartCoroutine(BGMEvents);
            }
        }
    }

    /// <summary>
    /// BGM�̃N���X�t�F�[�h����
    /// </summary>
    /// <returns></returns>
    /// <param name="index">AudioClip�̔ԍ�</param>
    /// <param name="loopFlg">���[�v�ݒ�B���[�v���Ȃ��ꍇ����false�w��</param>
    /// <returns></returns>
    private void CrossFadeChangeBGM(int index, bool loopFlg)
    {
        IsFadeBGMType = 3;     //�N���X�t�F�[�h��K�p
        if (BGMSource[0].clip != null)
        {
            //[0]���Đ�����Ă���ꍇ�A[0]�̉��ʂ����X�ɉ����āA[1]��V�����ȂƂ��čĐ�
            BGMSource[1].volume = 0;                                        //[1]�̉��ʂ�0��
            BGMSource[1].clip = BGM_Clips[index];                           //[1]��clip��ݒ�
            BGMSource[1].loop = loopFlg;                                    //[1]��loop��ݒ�
            BGMSource[1].Play();                                            //[1]���Đ�
            BGMSource[0].DOFade(0, CROSS_FADE_TIME).SetEase(Ease.Linear);   //[0]���t�F�[�h������

            BGMEvents = DelayProcess(CROSS_FADE_TIME, () =>                 //�t�F�[�h���Ԃ܂ŏ�����҂�
            {
                BGMSource[0].Stop();                                        //[0]���~
                BGMSource[0].clip = null;                                   //[0]��clip��null��
                BGMEvents = null;
                IsFadeBGMType = 0;                                          //�t�F�[�h�̃t���O��؂�
                return;
            });
            StartCoroutine(BGMEvents);
        }
        else
        {
            //[1]���Đ�����Ă���ꍇ�A[1]�̉��ʂ����X�ɉ����āA[0]��V�����ȂƂ��čĐ�
            BGMSource[0].volume = 0;                                        //[0]�̉��ʂ�0��
            BGMSource[0].clip = BGM_Clips[index];                           //[0]��clip��ݒ�
            BGMSource[0].loop = loopFlg;                                    //[0]��loop��ݒ�
            BGMSource[0].Play();                                            //[0]���Đ�
            BGMSource[1].DOFade(0, CROSS_FADE_TIME).SetEase(Ease.Linear);   //[1]���t�F�[�h������

            BGMEvents = DelayProcess(CROSS_FADE_TIME, () =>                 //�t�F�[�h���Ԃ܂ŏ�����҂�
            {
                BGMSource[1].Stop();                                        //[1]���~
                BGMSource[1].clip = null;                                   //[1]��clip��null��
                BGMEvents = null;
                IsFadeBGMType = 0;                                          //�t�F�[�h�̃t���O��؂�
                return;
            });
            StartCoroutine(BGMEvents);
        }
    }

    /// <summary>
    /// BGM���S��~
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
    /// BGM�ꎞ��~
    /// </summary>
    public void MuteBGM()
    {
        BGMSource[0].Stop();
        BGMSource[1].Stop();
    }

    /// <summary>
    /// �ꎞ��~����BGM���Đ��i�ĊJ�j
    /// </summary>
    public void ResumeBGM()
    {
        BGMSource[0].Play();
        BGMSource[1].Play();
    }

    //SE�̏���
    /// <summary>
    /// SE�Đ�
    /// </summary>
    /// <param name="seType"></param>
    public void PlaySE(SE_Type seType, float Vol = 1)
    {
        int index = (int)seType;
        SE_Volume = Vol;
        if (index < 0 || SE_Clips.Length <= index) return;

        //�Đ����ł͂Ȃ�AudioSource���g����SE��炷�B
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
    /// SE�t�F�[�h�C������
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

            //�Đ����ł͂Ȃ�AudioSource���g����SE��炷�B
            foreach (AudioSource source in SESource)
            {
                if (false == source.isPlaying)
                {
                    source.volume = 0;                                            //���ʂ�0��
                    source.clip = SE_Clips[index];                                //�N���b�v��ݒ�
                    source.loop = loopFlg;                                        //���[�v��ݒ�
                    source.Play();                                                //�Đ�
                    source.DOFade(SE_Volume, FADE_IN_TIME).SetEase(Ease.Linear);  //�t�F�[�h���|����

                    SEEvents = DelayProcess(FADE_IN_TIME, () =>                   //�t�F�[�h���Ԃ܂ŏ�����҂�
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
    /// SE�t�F�[�h�A�E�g����
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
                    source.DOFade(0, FADE_OUT_TIME).SetEase(Ease.Linear);       //SE���t�F�[�h

                    SEEvents = DelayProcess(FADE_OUT_TIME, () =>                //�t�F�[�h���Ԃ܂ŏ�����҂�
                    {
                        source.Stop();                                          //SE���~
                        source.clip = null;                                     //SE��clip��null��
                        source.loop = false;                                    //SE��loop��false��
                        SEEvents = null;
                        IsFadeSEType = 0;                                       //�t�F�[�h�̃t���O��؂�
                    });
                    StartCoroutine(SEEvents);
                }
            }
        }
    }

    /// <summary>
    /// SE��~
    /// </summary>
    public void StopSE()
    {
        //���ׂĂ�SE�pAudioSource���~
        foreach (AudioSource source in SESource)
        {
            source.Stop();
            source.clip = null;
            StopCoroutine(SEEvents);
        }
    }

    /// <summary>
    /// 3DSE�Đ�
    /// </summary>
    /// <param name="se3DType"></param>
    /// <param name="Vol"></param>
    /// <param name="loopFlg"></param>
    public void Play3DSE(SE3D_Type se3DType, float Vol = 1, bool loopFlg = true)
    {
        int index = (int)se3DType;
        SE_Volume = Vol;
        if (index < 0 || SE3D_Clips.Length <= index) return;

        //�Đ����ł͂Ȃ�AudioSource���g����SE��炷�B
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
    /// 3DSE�t�F�[�h�C��
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

            //�Đ����ł͂Ȃ�AudioSource���g����3DSE��炷�B
            foreach (AudioSource source in SE3DSource)
            {
                if (false == source.isPlaying)
                {
                    source.volume = 0;
                    source.clip = SE3D_Clips[index];
                    source.loop = loopFlg;
                    source.Play();
                    source.DOFade(endValue: SE3D_Volume, duration: FADE_IN_TIME).SetEase(Ease.Linear);

                    SE3DEvents = DelayProcess(FADE_IN_TIME, () =>             //�t�F�[�h���Ԃ܂ŏ�����҂�
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
    /// SE3D�t�F�[�h�A�E�g����
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
                    source.DOFade(0, FADE_OUT_TIME).SetEase(Ease.Linear);       //SE���t�F�[�h

                    SE3DEvents = DelayProcess(FADE_OUT_TIME, () =>              //�t�F�[�h���Ԃ܂ŏ�����҂�
                    {
                        source.Stop();                                          //SE���~
                        source.clip = null;                                     //SE��clip��null��
                        source.loop = false;                                    //SE��loop��false��
                        SE3DEvents = null;
                        IsFadeSEType = 0;                                       //�t�F�[�h�̃t���O��؂�
                    });
                    StartCoroutine(SE3DEvents);
                }
            }
        }
    }

    /// <summary>
    /// SE3D��~
    /// </summary>
    public void Stop3DSE()
    {
        //���ׂĂ�SE�pAudioSource���~
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

    //�I�u�W�F�N�g�����邩�m�F�p
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
        ////�T�E���h�̒�~
        //SoundManager.Instance.StopBGM();
        //SoundManager.Instance.StopSE();
        //SoundManager.Instance.Stop3DSE();
    }

    //2023/0921�͍��ǋL
    public static bool NowSound()
    {
        return nowSound;
    }
}

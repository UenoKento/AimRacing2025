//===================================================
// ファイル名	：EngineSound.cs
// 概要			：エンジン音の制御スクリプト
// 作成者		：一條真一郎
// 作成日		：2019.06.20
//===================================================
// 更新         ：[豊田達也]2024.08.20 2024Engineに適応
/*
                | MIN-MAX
    SPEED       | 0-250
    RPM         | 0-10k
    ACCEL       | 0-1
    TMSINE      | (-100)-100
    SHIFTCHANGE | (-1)-8
    BACKTURBINE | 0-1
    SINE        | (-100)-100
    CRASHVOL    | 0-1
    SHIFTDOWN   | 0-1
    CRASHSPD    | 0-250
    RUBVOL      | 0-1 
    ENGINEMUTE  | 0-1
    PITCHSINE   | (-100)-100
    INTRO       | 0-1
    TMPITCH     | (-100)-100
    DEBUGMUTE   | 0-1
    BRAKENOIZ   | 0-1
    BURNOUT     | 0-1
    AFTERFIRE   | 0-1
    COAST       | 0-1
    MISSGEAR    | 0-1
    MASTAR      | 0-100
    SKID        | 0-1
 */

using UnityEngine;
public class EngineSound : MonoBehaviour
{
    //Event
    FMOD.Studio.EventInstance m_EngineEvent;
    //Bus
    FMOD.Studio.Bus m_MasterBus;
    
    // Vehicle
    [SerializeField] private VehicleController2024 m_VehicleController;

    // SPEED
    [SerializeField] private float m_SPEED;
    // RPM
    [SerializeField] private float m_RPM;
    [SerializeField] private bool overRPM;
    // ACCEL
    [SerializeField] private float m_ACCEL;
    private bool isAccel;
    private bool acceloff;
    private float accelJudge = 0;
    // SHIFT
    [SerializeField] private int m_SHIFTCHANGE;
    private int prevShiftNum;
    private bool shiftUp;
    [SerializeField] private bool isShift;
    private float m_SHIFTDOWN;
    private float shiftUpTime;
    // BUCKTRUBINE
    private float m_BACKTURBINE;                //バックタービン
    private float turbineTime;                  //
    private bool isTurbine;                     //????
    [SerializeField] private bool turbineOn;    //バックタービン切り替え用変数
    // SINE
    private float sinTime = 0;
    [SerializeField] private float sine;
    private const float sineMax = 40;
    [SerializeField] private float sinWidth = sineMax;
    [SerializeField] private float sinSec = 0.5f;
    private bool sinStart;
    private bool offsin;
    // FRIC
    [SerializeField] private float fric;
    // SLIP
    [SerializeField] private float slip;
    // ANGLE
    [SerializeField] private float angle;
    // CLASH
    [SerializeField] private bool crash;
    // RUB
    [SerializeField] private float m_RUBVOL;
    [SerializeField] private bool rub;
    // FIRE
    [SerializeField] private float afterFireTime;
    // AFTERFIRE
    private float aftertime = 0;
    [SerializeField] private bool isAfterFire;
    // MUTE
    private float muteTime;
    [SerializeField] private bool isMute;
    // TMPITCH
    [SerializeField] private float m_TMPITCH = 0;
    [SerializeField] private float tsinedown = 0;
    // INTRO
    [SerializeField] private float m_INTRO = 0;
    // BRAKE
    [SerializeField] private float brake;
    // PITCHSINE
    [SerializeField] private float m_PITCHSINE;
    private float pitchSinTime;
    private const float pitchSinWidth = 4;
    [SerializeField] private float pitchSinSec = 1f;
    // REV
    private float overRev = 0.02f;
    private bool overRevReset;
    // DEBUGMUTE
    private bool debugmute;
    // BURN
    private bool isBurn;
    // COAST
    private float coastTime;
    private float coastWaitTime;
    private int coast;
    private bool isCoast;
    // SKID
    [SerializeField] private float m_SKID;
    private float skidTime;
    private bool isSkid;
    private bool skidOn;
    private bool skidEnd;

    [Header("--------------------音量--------------------")]
    [SerializeField, Range(0, 100)]
    private float m_MasterVolume;

    //フェード用
    float FadeDeltaTime;
    float FadeDeltaSeconds;                 // フェード時間を設定
    private float m_FadeVolume;             // BGMの現在のボリュームを獲得
    bool IsFade;                            // フェードアウトに設定

    void Reset()
    {
        m_VehicleController = FindFirstObjectByType<VehicleController2024>();
    }

    private void Awake()
    {
        // FMODファイルのEngineデータの読み込み
        m_EngineEvent = FMODUnity.RuntimeManager.CreateInstance("event:/Engine");
        m_MasterBus = FMODUnity.RuntimeManager.GetBus("Bus:/");
        m_MasterBus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        // エンジン音を鳴らし始める
        m_EngineEvent.start();
        m_SHIFTDOWN = 0;
        shiftUpTime = 0;
        m_BACKTURBINE = 0;
        isTurbine = false;

        // フェード用
        FadeDeltaTime = 0;
        FadeDeltaSeconds = 5;              // フェード時間を設定
        m_FadeVolume = 0;                  // BGMの現在のボリュームを獲得
        IsFade = true;                     // フェードアウトに設定

        m_EngineEvent.setParameterByName("AFTERFIRE", 1);
        m_EngineEvent.setParameterByName("DEBUGMUTE", 0);
        m_EngineEvent.setParameterByName("MASTER", m_FadeVolume);
    }
    private void Update()
    {
        if (IsFade == true)
        {
            FadeDeltaTime += Time.deltaTime;

            // フェード時間に達したらBGMを止める
            if (FadeDeltaTime >= FadeDeltaSeconds)
            {
                IsFade = false;
                m_EngineEvent.setParameterByName("MASTER", m_MasterVolume);
            }

            // フェード時間中は徐々に音量を下げていく
            m_FadeVolume = (float)(m_MasterVolume * (FadeDeltaTime / FadeDeltaSeconds));
            m_EngineEvent.setParameterByName("MASTER", m_FadeVolume);
        }
        else
        {
            m_EngineEvent.setParameterByName("MASTER", m_MasterVolume);
        }

        m_EngineEvent.setParameterByName("INTRO", m_INTRO);
        if (/*im.EngineStart &&*/ m_INTRO < 1 /*&& psf.bStart*/)
        {
            m_INTRO += Time.deltaTime / 4.0f;
        }

        // 開幕
        if (m_SHIFTCHANGE <= 1 && !isBurn)
        {
            if (9200 <= m_RPM)
            {
                acceloff = false;
            }
            else if (m_RPM <= 9000 && m_RPM > 1000)
            {
                acceloff = true;
            }
            else
            {
                acceloff = false;
            }

            if (5000 < m_RPM)
            //if(7000 < rpm)
            {
                m_EngineEvent.setParameterByName("BURNOUT", 1);
            }
            else
            {
                m_EngineEvent.setParameterByName("BURNOUT", 0);
            }
        }
        else
        {
            m_EngineEvent.setParameterByName("BURNOUT", 0);
            acceloff = false;
            isBurn = true;
        }
        if (m_RPM > 7200)
        {
            isBurn = true;
        }

        //
        m_TMPITCH = 0;
        //
        if (sinStart)
        {
            sinTime += Time.deltaTime;
            //sinSec = 0.5f / ((rpm / 1000) * 1.0f);
            sinSec = 0.5f / (5 * 1.0f);
            float f = 1.0f / sinSec;
            sine = Mathf.Sin(2 * Mathf.PI * f * sinTime);
            m_TMPITCH = sine * 12;
            sine *= 400;
            if (sinTime > 0.3f)
            {
                sinStart = false;
                sinTime = 0;
            }
        }


        // SPEEDの値をVehicleController.csから取得し、FMODのSPEEDの値に反映させる
        m_SPEED = m_VehicleController.KPH;
        m_EngineEvent.setParameterByName("SPEED", m_SPEED);


        // RPMの値をVehicleController.csから取得し、FMODのRPMの値に反映させる
        m_RPM = m_VehicleController.EngineRPM;
        m_EngineEvent.setParameterByName("RPM", m_RPM);

        RPMOver();


        // ACCELの値をVehicleController.csから取得し、FMODのACCELの値に反映させる
        m_ACCEL = m_VehicleController.Accel;
        m_EngineEvent.setParameterByName("ACCEL", m_ACCEL);

        if (m_ACCEL > accelJudge && accelJudge <= 0)
        {
            sinStart = true;
            sinTime = 0;
            offsin = false;
        }
        else if (m_ACCEL < accelJudge && m_ACCEL <= 0)
        {
            offsin = true;
        }


        // 惰性走行(Acceloff)
        if (accelJudge - m_ACCEL > 0.2f && accelJudge > 0.35f && m_SHIFTCHANGE > 0 && m_RPM > 4000)
        {
            isCoast = true;
        }
        else if (accelJudge - m_ACCEL > 0.2f)
        {
            isCoast = false;
            m_EngineEvent.setParameterByName("COAST", 0);
            coastTime = 0;
        }

        if (isCoast == true)
        {
            m_EngineEvent.setParameterByName("COAST", 1);
            coastTime += Time.deltaTime;
            if (coastTime > 4f || m_ACCEL > 0.2f)
            {
                isCoast = false;
            }
        }

        if (acceloff == true)
        {
            sinStart = true;
            sinTime = 0;
            offsin = false;
            m_EngineEvent.setParameterByName("ENGINEMUTE", 1);
        }
        else
        {
            m_EngineEvent.setParameterByName("ENGINEMUTE", 0);
        }


        // BRALE
        brake = m_VehicleController.Brake;
        if (Mathf.Abs(brake) > 0)
        {
            angle = Mathf.Abs(angle) + Mathf.Abs(brake);
            m_EngineEvent.setParameterByName("BRAKENOIZ", 0.65f);
            m_EngineEvent.setParameterByName("BRAKE", Mathf.Abs(brake));

        }
        else
        {
            m_EngineEvent.setParameterByName("BRAKENOIZ", 0);
        }


        // SHIFTCHANGE
        m_SHIFTCHANGE = m_VehicleController.Transmission.ActiveGear;
        if (isShift)
        {
            m_EngineEvent.setParameterByName("SHIFTCHANGE", m_SHIFTCHANGE);
        }

        m_EngineEvent.setParameterByName("SHIFTCHANGE", m_SHIFTCHANGE);

        // ギアが前フレームの値より小さい、かつギア1、N、Rではないなら
        if (m_SHIFTCHANGE < prevShiftNum && m_SHIFTCHANGE != 1 && m_SHIFTCHANGE != 0 && m_SHIFTCHANGE != -1)
        {
            m_SHIFTDOWN = 1;
            //accel = 0;
        }
        else if (m_SHIFTDOWN > 0)
        {
            m_SHIFTDOWN -= Time.deltaTime;
        }

        // 前フレーム更新
        prevShiftNum = m_SHIFTCHANGE;
        m_EngineEvent.setParameterByName("SHIFTDOWN", m_SHIFTDOWN);

        // シフト上げてから一定時間アクセルを離す
        if (m_SHIFTCHANGE > prevShiftNum || m_SHIFTCHANGE < prevShiftNum)
        {
            shiftUp = true;
        }
        if (shiftUp)
        {
            m_EngineEvent.setParameterByName("ENGINEMUTE", 1);
            m_ACCEL = 0;
            shiftUpTime += Time.deltaTime;
            if (shiftUpTime > 0.1f)
            {
                shiftUp = false;
                m_EngineEvent.setParameterByName("ENGINEMUTE", 0);
                shiftUpTime = 0;
            }
        }

        // TMSINE


        // BACKTURBINE


        // SQUEAL
        angle = m_VehicleController.Steering;
        m_EngineEvent.setParameterByName("SQUEAL", Mathf.Abs(angle));
        

        // アフターファイア
        if (accelJudge - m_ACCEL > 0.2f && accelJudge > 0.35f && m_SHIFTCHANGE > 0 && m_RPM > 4000)
        {
            m_EngineEvent.setParameterByName("AFTERFIRE", 1);
        }
        else
        {
            m_EngineEvent.setParameterByName("AFTERFIRE", 0);
        }
        AfterFire();

        // TMPITCH
        m_EngineEvent.setParameterByName("TMPITCH", m_TMPITCH);


        // RUB
        Rub();


        // PICTHSIN 
        PitchSin();


        // SKID
        if (isSkid && !skidEnd)
        {
            skidTime += Time.deltaTime;
            if (skidTime < 0.3f)
            {
                if (m_SKID < 1)
                {
                    m_SKID += Time.deltaTime * 4;
                }
                else
                {
                    m_SKID = 1;
                }
            }
            else if (skidTime > 0.6f)
            {
                if (m_SKID >= 0)
                {
                    m_SKID -= Time.deltaTime * 1;
                }
                else
                {
                    m_SKID = 0;
                    skidEnd = true;
                }
            }
        }
        m_EngineEvent.setParameterByName("SKID", m_SKID);


        // タービン切り替え
        if (turbineOn)
        {
            Turbine();
        }



        accelJudge = m_ACCEL;
    }

    private void OnDestroy()
    {
        //サウンドストップ
        m_EngineEvent.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        m_MasterBus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    // rpmmax時の点滅処理
    void RPMOver()
    {
        if (!shiftUp && m_SHIFTCHANGE != 0)
        {
            if (9000 < m_RPM && m_ACCEL != 0)
            {
                muteTime += Time.deltaTime;
                //if (0.02f < muteTime)
                if (overRev < muteTime)
                {
                    if (isMute)
                    {
                        isMute = false;
                        overRev = Random.Range(0.01f, 0.12f);
                        muteTime = 0;
                    }
                    else
                    {
                        isMute = true;
                        overRev = 0.005f;
                        muteTime = 0;
                    }
                }

                if (isMute)
                {
                    sinStart = true;
                    sinTime = 0;
                    offsin = false;
                    m_EngineEvent.setParameterByName("ENGINEMUTE", 1);
                }
                else
                {
                    m_EngineEvent.setParameterByName("ENGINEMUTE", 0f);
                }
            }
            else
            {
                isMute = false;
                m_EngineEvent.setParameterByName("ENGINEMUTE", 0);
            }
        }
    }

    void AfterFire()
    {
        if (accelJudge - m_ACCEL > 0.15f && m_RPM > 4000 && accelJudge > 0.35f && m_SHIFTCHANGE != 0)
        {
            isAfterFire = true;
        }

        if (isAfterFire)
        {
            aftertime += Time.deltaTime;
            afterFireTime += Time.deltaTime;
        }

        if (accelJudge - m_ACCEL < -0.2f || aftertime > 0.8f)
        {
            isAfterFire = false;
            aftertime = 0;
        }

        if (afterFireTime > 0.2f && isAfterFire && m_ACCEL < 0.4f)
        {
            float f = Random.Range(0.0f, 1.0f);
            if (f < 0.6f)
            {
                m_EngineEvent.setParameterByName("AFTERFIRE", 1);
            }
            else
            {
                m_EngineEvent.setParameterByName("AFTERFIRE", 0);
            }
            afterFireTime = 0;
        }
    }

    void PitchSin()
    {
        if (m_ACCEL > 0)
        {
            pitchSinTime += Time.deltaTime;
            pitchSinSec = 4f / ((m_RPM / 1000) * 1.0f);
            float f = 1.0f / pitchSinSec;
            m_PITCHSINE = Mathf.Sin(2 * Mathf.PI * f * pitchSinTime);
            m_PITCHSINE *= pitchSinWidth;
            m_EngineEvent.setParameterByName("PITCHSINE", m_PITCHSINE);
        }
        else
        {
            pitchSinTime = 0;
        }
    }

    void Turbine()
    {
        // タービン
        if (isTurbine && m_BACKTURBINE <= 0)
        {
            m_BACKTURBINE = m_RPM / 10000f;
            isTurbine = false;
        }

        // アクセル離してる状態
        if (m_ACCEL <= 0)
        {
            if (isAccel)
            {
                isTurbine = true;
                m_BACKTURBINE = 0;
                turbineTime = 0;
            }
            isAccel = false;
        }
        // アクセル押してる状態
        else
        {
            isAccel = true;
        }

        if (m_BACKTURBINE > 0)
        {
            turbineTime += Time.deltaTime;
            if (turbineTime > 1.75f)
            {
                turbineTime = 0;
                m_BACKTURBINE = 0;
            }
        }
        m_EngineEvent.setParameterByName("BACKTURBINE", m_BACKTURBINE);
    }

    void Rub()
    {
        if (rub)
        {
            m_RUBVOL = 1;
        }
        else
        {
            if (m_RUBVOL > 0)
            {
                m_RUBVOL -= Time.deltaTime * 2;
            }
            else
            {
                m_RUBVOL = 0;
            }
        }
        m_EngineEvent.setParameterByName("RUBVOL", m_RUBVOL);
    }
}
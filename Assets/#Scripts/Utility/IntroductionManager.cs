////===================================================
//// ファイル名	：IntroductionManager.cs
//// 概要			：InGameの管理
//// 作成者		：藤森 悠輝
//// 作成日		：2019.06.06
////===================================================
//using UnityEngine;
//using UnityEngine.UI;
//using CUEngine.Pattern;
//using UnityEngine.Rendering.PostProcessing;
//using UnityEngine.SceneManagement;

//public class IntroductionManager : StateBaseScriptMonoBehaviour
//{
//    [SerializeField] public float time = 0;
//    [SerializeField] GameObject cameraController;
//    [SerializeField] GameObject mainCameraObject;
//    [SerializeField] GameObject introductionObject;
//    [SerializeField] GameObject miniMapObject; // 2019/08/14追加
//    GameObject carObject;
//    [SerializeField] Transform debugPosition;
//    //[SerializeField] Fade fadeObject;
//    AIM.VehicleController vc;
//    [SerializeField] AudioSource SE;
//    [SerializeField] AudioSource SE_glitch;
//    [SerializeField] Logitech_test LogiCon;
//    //[SerializeField] private PlayerData playerData;
//    [SerializeField] private AIM.DesktopInputManager input_Player;
//    //[SerializeField] Recorder recorder;
//    //[SerializeField] TimeFlow timeFlow;
//    //[SerializeField] PostExposureChanger flash;
//    [SerializeField] Canvas resultTimeCanvas;
//    //[SerializeField] FadeEditor fadeEditor;
//    [SerializeField] bool isCountDown;
//    public bool isAuto = false;
//    //   [SerializeField] ThanksManager thanks;
//    //[SerializeField] ResultManager resultManager;
//    [SerializeField] GameObject CarPrefab;
//    [SerializeField]
//    CarSearch carSearch;
//    //[SerializeField] LapTimeObject lapTime;
//    //[SerializeField] AIM.GhostDataManager gdm;

//    //[SerializeField] Loading loading;

//    //=============================================
//    bool isOneced = false;
//    bool isOneced2 = false;
//    //=============================================

//    private bool engineStart = false;       // 8/27 一條
//    public bool EngineStart
//    {
//        get
//        {
//            return engineStart;
//        }
//    }

//    //private CarsRespawn carRespawn;
//    //[SerializeField] Recorder recorder;
//    //[SerializeField] GhostPlayer ghost;
//    private void Start()
//    {
//        if (cameraController == null) Debug.LogWarning("cameraObjectがnullです。");
//        if (miniMapObject == null) Debug.LogWarning("miniMapCameraObjectがnullです。"); // 2019/08/14追加
//        if (introductionObject == null) Debug.LogWarning("introductionObjectがnullです。");
//        //if (fadeObject == null) Debug.LogWarning("fadeObjectがnullです。");
//        //carObject = GameObject.Find("BMW M4 DTM");
//        //vc = carObject.GetComponent<AIM.VehicleController>();
//        //carRespawn = carObject.GetComponent<CarsRespawn>();
//        //if (!gdm.soundPlayEnable)
//        {
//            LogiCon = GameObject.Find("VehicleManager").GetComponent<Logitech_test>();
//        }
//        //if(vc.transmission.transmissionType == AIM.Transmission.TransmissionType.Automatic)
//        //{
//        //    isAuto = true;
//        //}
//        //else
//        //{
//        //    isAuto = false;
//        //}
//        //Instantiate(CarPrefab, CarPrefab.transform.position, CarPrefab.transform.rotation);
//        //if (loading == null)
//        //{
//        //    Debug.LogWarning("loadingがnullです。");
//        //}
//        //else
//        //{
//        //    //if(SceneManager.GetActiveScene().name == "GhostOkutama")
//        //    //{
//        //    loading.LoadSetUp();
//        //    //}
//        //}
//    }

//    private void Update()
//    {
//        time += Time.deltaTime;

//        //if (!carSearch.isCar)
//        //    return;

//        if (!isOneced2)
//        {
//            //if (!carSearch.isIntroductionManager && !gdm.soundPlayEnable)
//            //{
//            //    carObject = carSearch.carObject;
//            //    vc = carSearch.vc;
//            //    carRespawn = carObject.GetComponent<CarsRespawn>();
//            //    carSearch.isIntroductionManager = true;
//            //    vc.enabled = false;
//            //    if (vc.transmission.transmissionType == AIM.Transmission.TransmissionType.Automatic)
//            //    {
//            //        isAuto = true;
//            //    }
//            //    else
//            //    {
//            //        isAuto = false;
//            //    }
//            //}

//            if (GameObject.Find("Main Camera") != null)
//            {
//                mainCameraObject = GameObject.Find("Main Camera");
//            }
//            introductionObject.transform.Find("Time").gameObject.SetActive(false);
//            mainCameraObject.SetActive(false);

//            isOneced2 = true;
//        }
//    }

//    //ノード専用メソッド
//    //フェード呼び出し処理
//    public void StartFadeOut(float sec)
//    {
//        //StartCoroutine(fadeObject.FadeOut(sec));
//    }
//    public void StartFadeIn(float sec)
//    {
//        //StartCoroutine(fadeObject.FadeIn(sec));
//    }
//    //Imageフェード
//    public void StartImageFadeIn(Image image, float sec)
//    {
//        //StartCoroutine(fadeObject.ImageFadeIn(image, sec));
//    }
//    public void StartImageFadeOut(Image image, float sec)
//    {
//        //StartCoroutine(fadeObject.ImageFadeOut(image, sec));
//    }
//    public void StartImageIn(Image image)
//    {
//        image.color += new Color(0, 0, 0, 1);
//    }
//    public void GameObjectON(GameObject gameObject)
//    {
//        gameObject.SetActive(true);
//    }
//    public void GameObjectOFF(GameObject gameObject)
//    {
//        gameObject.SetActive(false);
//    }
//    //public void GlitchFadeON(UnityEngine.Rendering.PostProcessing.GlitchEffect glitch)
//    //{
//    //    glitch.intensity = 1.0f;
//    //    glitch.flipIntensity = 1.0f;
//    //    glitch.colorIntensity = 1.0f;

//    //    glitch._glitchSpeed = 10.0f;
//    //    glitch._glitchSwing = 10.0f;
//    //}
//    //public void GlitchFadeOFF(UnityEngine.Rendering.PostProcessing.GlitchEffect glitch)
//    //{
//    //    glitch.intensity = 0f;
//    //    glitch.flipIntensity = 0f;
//    //    glitch.colorIntensity = 0f;

//    //    glitch._glitchSpeed = 1.0f;
//    //    glitch._glitchSwing = 1.0f;
//    //}
//    //Sound
//    public void SoundStart(AudioClip clip)
//    {
//        SE.clip = clip;
//        SE.Play();
//    }
//    public void Gear0()
//    {
//        //if (gdm.soundPlayEnable)
//        //{
//        //    return;
//        //    //vc.transmission.shift_N_1 = true;
//        //    //vc.transmission.Gear = 0;
//        //}
//        if (vc.engine.rpm > 3000 && (Input.GetAxisRaw("L_R_Trigger") > 0.7 || Input.GetAxisRaw("Vertical") > 0.7))
//            //vc.transmission.Gear = 1;
//            vc.transmission.shift_N_1 = true;
//        else
//            //vc.transmission.Gear = 1;
//            vc.transmission.shift_N_1 = true;
//        vc.transmission.Gear = 0;

//        //===============================================
//        if (!isOneced)
//        {
//            vc.engine.IntroSetPrevRPM(/*vc.engine.maxRPM*/9000);
//            vc.engine.maxRPM = 9000;
//            isOneced = true;
//        }
//        //===============================================
//        vc.engine.minRPM = 4012;

//        if (isAuto)
//        {
//            vc.transmission.transmissionType = AIM.Transmission.TransmissionType.Manual;
//        }
//    }
//    //切り替え処理
//    public void StartInGame()
//    {
//        introductionObject.transform.Find("Meter").gameObject.SetActive(false);
//        //introductionObject.transform.Find("Camera1").gameObject.SetActive(true);
//        miniMapObject.SetActive(false); // 2019/08/14追加
//                                        //mainCameraObject.SetActive(false);
//                                        //vc.gear1_maxRPM = 8000;
//    }
//    public void ChengeCameraPos(GameObject CMvcam, GameObject DollyTrack)
//    {
//        CMvcam.SetActive(false);
//        DollyTrack.SetActive(false);
//    }
//    public void ChengeMainCamera(GameObject mainCamera, GameObject nextCamera)
//    {
//        Destroy(mainCamera);
//        nextCamera.gameObject.SetActive(true);
//    }
//    public void CountDown(GameObject mainCamera)
//    {
//        //if (!gdm.soundPlayEnable)
//        //{
//        //    carObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
//        //    vc.enabled = true;
//        //    if (isAuto)
//        //    {
//        //        vc.transmission.transmissionType = AIM.Transmission.TransmissionType.Manual;
//        //    }
//        //    vc.transmission.Gear = 1;
//        //}
//        Destroy(mainCamera);
//        introductionObject.transform.Find("Meter").gameObject.SetActive(true);

//        //lapTime.TimeUpdate(0.0f);

//        mainCameraObject.SetActive(true);

//        miniMapObject.SetActive(true);  // 2019/08/14追加
//        engineStart = true;
//    }
//    // 2019/08/14追加
//    public void CountNumber(int num)
//    {
//        isCountDown = true;
//        //var bloom = mainCameraObject.GetComponent<Bloom2D>();
//        //if (bloom == null)
//        //{
//        //    Debug.LogWarning("Bloom2Dがnullです。");
//        //    return;
//        //}
//        //bloom.IsActive = true;
//        //bloom.Number = num;
//    }
//    public void PlayInGame()
//    {
//        // 最初の一回の判定をとる
//        //if (!gdm.isRunStart)
//        //{
//        //    gdm.isRunStart = true;
//        //}

//        //recorder.StartRecord();
//        //ghost.Replay();
//        //if (!gdm.soundPlayEnable)
//        //{
//        //    carObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
//        //    vc.transmission.Gear = 1;
//        //    vc.engine.rpm = 7500;
//        //    //if(inGame != null)
//        //    //	inGame.Play();
//        //    vc.transmission.shift_N_1 = false;
//        //    vc.gear1_maxRPM = 6500;

//        //    vc.isStart = true;

//        //    //===============================================
//        //    //vc.engine.maxRPM = 9200;
//        //    vc.engine.IntroSetMaxRPM();
//        //    vc.engine.minRPM = 1800;
//        //    //===============================================
//        //}
//        introductionObject.transform.Find("Time").gameObject.SetActive(true);


//        isCountDown = false;

//        //DebugPrint.Print("PlayInGame");


//        //if (TwoChoice.nowChoiceNum == 2)
//        //    isAuto = true;
//        //if (TwoChoice.nowChoiceNum == 2)
//        //    isAuto = false;

//        //if (!gdm.soundPlayEnable)
//        //{
//        //    if (isAuto == true)
//        //    {
//        //        vc.transmission.transmissionType = AIM.Transmission.TransmissionType.Automatic;
//        //    }
//        //    else
//        //    {
//        //        vc.transmission.transmissionType = AIM.Transmission.TransmissionType.Manual;
//        //    }
//        //}
//    }

//    //車呼び出し
//    public void SummonCar()
//    {
//        Instantiate(CarPrefab, CarPrefab.transform.position, CarPrefab.transform.rotation);
//    }
//    // ここが呼び出し
//    public bool accelePush()
//    {
//        //if (LogiCon.getClutch() >= 0.8f || LogiCon.getAccele() >= 0.8f || Input.GetAxis("Vertical") >= 0.8f)
//        //{
//        //	time = 0;
//        //	return true;
//        //}
//        // 東樹変更
//        // Mathf.Abs()を使用し、確実に入力を取るように変更
//        //if (gdm.soundPlayEnable)
//        //{
//        //    return true;
//        //}
//        //else
//        //{
//            if (Input.GetAxis("L_R_Trigger") >= 0.7f || Mathf.Abs(Input.GetAxis("Vertical")) >= 0.7f || -Input.GetAxis("Accel") >= 0.7f || AIM.InputHandleController.GetHandleAccelCheck() > 0.7f)
//            {
//                time = 0;
//                return true;
//            }
//        //}
//        return false;
//    }
//    //リザルト画面表示
//    public void GoToResult()
//    {
//        // ゴール後すぐシーン切り替え
//        //if (SceneManager.GetActiveScene().name == "GhostOkutama")
//        //{
//        //	loading.GoNextScene();
//        //	time = 0;
//        //}

//        introductionObject.transform.Find("Meter").gameObject.SetActive(false);
//        introductionObject.transform.Find("Time").gameObject.SetActive(false);
//        //リザルト時に車の動きを止める
//        //carObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
//        //だんだんと動かす
//        //if (!gdm.soundPlayEnable)
//        //{
//        //    vc.engine.brakeFrag = true;
//        //}
//        input_Player.isControl = false;
//    }
//    public void StartResult()
//    {
//        // タイム表示前にシーン切り替え
//        //if (SceneManager.GetActiveScene().name == "GhostOkutama")
//        //{
//        //	loading.GoNextScene();
//        //	time = 0;
//        //}

//        //StartCoroutine(resultManager.Replay());
//    }
//    public bool IsEndResult()
//    {
//        //return resultManager.isEnd;
//    }
//    public void Result_position(GameObject mainCamera, GameObject resultPosition)
//    {
//        //リザルトポジションに移動
//        carObject.transform.position = resultPosition.transform.position;
//        carObject.transform.rotation = resultPosition.transform.rotation;

//        miniMapObject.SetActive(false);  // 2019/08/14追加

//        mainCamera.GetComponent<Camera>().enabled = true;
//    }

//    public void jsonUpRanking()
//    {
//        //playerData.time = GoalJudgment.getGoalTime();
//        //playerData.name = InputName.getEntryName();
//        //Record.Save(playerData);
//    }
//    //シーン切り替え
//    public void nextScene()
//    {
//        //gameObject.GetComponent<NextScene>().nextScene();
//        //time = 0;
//    }
//    //状態変移用メソッド
//    public bool flowFade0(float sec)
//    {
//        if (time >= sec)
//        {
//            time = 0;
//            return true;
//        }
//        return false;
//    }
//    public bool flow()
//    {
//        return true;
//    }
//    //ゴール時偏移
//    public bool InGoal(GoalJudgment goalObject)
//    {
//        time = 0;
//        if (goalObject.getIsGoal())
//        {
//            goalObject.debugGoal();
//            return true;
//        }
//        else
//        {
//            return false;
//        }
//    }
//    public void debugGoal()
//    {
//        time = 0;

//        //DebugPrint.Print("ゴール前");

//        if (Input.GetKeyDown(KeyCode.F6))
//        {
//            carObject.transform.position = debugPosition.position;
//            carObject.transform.rotation = debugPosition.rotation;
//            carRespawn.nowPoint = 307;
//        }
//    }
//    //エンター実行処理
//    public bool nextEnter()
//    {
//        if (Input.GetKeyDown(KeyCode.Return))
//        {
//            time = 0;
//            return true;
//        }
//        else
//            return false;
//    }
//    //アクセル実行処理
//    public bool nextAccel()
//    {
//        if (Input.GetAxisRaw("L_R_Trigger") > 0.7 || Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0.7 || -Input.GetAxis("Accel") >= 0.7f || AIM.InputHandleController.GetHandleAccelCheck() > 0.7f)
//        {
//            time = 0;
//            return true;
//        }
//        else
//            return false;
//    }
//    //杉山ゾーン---------------------------------------------------------------------------
//    public void Flash()
//    {
//        StartCoroutine(flash.Flash());
//    }
//    public void DEMORecording()
//    {
//        StartCoroutine(recorder.Recording());
//    }
//    public void DEMORecording_Save()
//    {
//        recorder.Save();
//    }
//    public void ResultRecording(ResultRecorder resultRecorder)
//    {
//        StartCoroutine(resultRecorder.Recording());
//    }
//    public void ResultReplay(ResultPlayer resultPlayer)
//    {
//        resultPlayer.Play();
//    }
//    public bool FlowResultCut(ResultRecorder resultRecorder, float add)
//    {
//        return timeFlow.IsTimeFlow(resultRecorder.timeMax + add);
//    }
//    public bool IsTimeFlow(float time)
//    {
//        return timeFlow.IsTimeFlow(time);
//    }
//    public void GlitchActivate(Glitch glitch)
//    {
//        SE_glitch.Play();
//        glitch.IsActive = true;
//    }
//    public void GlitchUnActivate(Glitch glitch)
//    {
//        glitch.IsActive = false;
//    }
//    public void AudioPlay(AudioSource audio)
//    {
//        audio.Play();
//    }
//    public void AudioVolumeChange(AudioDowner audio)
//    {
//        StartCoroutine(audio.AudioVolumeChange());
//    }
//    public void AudioVolumeChange_gyaku(AudioDowner audio)
//    {
//        StartCoroutine(audio.AudioVolumeChange_gyaku());
//    }
//    public void Fadeout()
//    {
//        StartCoroutine(fadeEditor.FadeoutCol());
//    }
//    public void WhiteOut()
//    {
//        StartCoroutine(flash.WhiteOut());
//    }
//    public void WhiteOut_Slowly()
//    {
//        StartCoroutine(flash.WhiteOut_Slowly());
//    }
//    public void WhiteOut_PanelSynchronize()
//    {
//        StartCoroutine(flash.WhiteOut_PanelSynchronize());
//    }
//    public void WhiteIn()
//    {
//        if (SceneManager.GetActiveScene().name == "GhostOkutama")
//        {
//            loading.GoNextScene();
//            time = 0;
//        }
//        StartCoroutine(flash.WhiteIn());
//    }
//    public void ThanksStart()
//    {
//        if (SceneManager.GetActiveScene().name == "Okutama")
//        {
//            loading.GoNextScene();
//            //thanks.ThanksStart();
//        }
//        //吉家追加
//        //修正必須→現在この仕組み分からないため緊急対応用に
//        //loading.GoNextScene();
//        //time = 0;
//    }
//    public void ThanksUpdate()
//    {
//        thanks.ThanksUpdate();
//    }
//    public bool ThanksEnd()
//    {
//        return thanks.IsEnd();
//    }
//    public void FadeinOutUIStart(FadeInOutUI ui)
//    {
//        StartCoroutine(ui.FadeCap());
//    }
//    public bool IsGasInput()
//    {
//        return Input.GetAxisRaw("L_R_Trigger") >= 0.8f || Input.GetAxis("Vertical") >= 0.8f || -Input.GetAxis("Accel") >= 0.7f || AIM.InputHandleController.GetHandleAccelCheck() > 0.6f;
//    }
//    public bool IsCarSearch()
//    {
//        return carSearch.isCar;
//    }
//}
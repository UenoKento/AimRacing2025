using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(998)]
public class chair : MonoBehaviour
{
    public WIZMOController controller { get; private set; } // 椅子を制御するクラス

    //2023坂本郁樹追記-------------------------------
    private bool b_title = false;
    private bool b_entry = false;
    private bool b_result = false;
    private bool b_map = false;
    //オブジェクトを静的にする
    public static chair instance;
    //----------------------------------------------
    #region デバッグ用
    public bool bIsConnected { get; private set; } = false;         // 椅子に接続したかのフラグ
    private int reconnectCnt = 0;                                   // 再接続までのカウンター
    public bool bEmergencyShutdown { get; private set; } = false;   // 緊急停止中かどうか
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += NextLoad;
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        // 接続のチェック
        #region if(!CheckChairConnect()) return;
#if !UNITY_EDITOR
// 元の処理
if(!CheckChairConnect()) return;
#else
        // デバッグ用
        CheckChairConnect();
#endif
        #endregion

        // 緊急停止キー
        if (!EmergencyShutdown()) return;

        //各シーンで行う椅子の初期設定
        switch (SceneManager.GetActiveScene().name)
        {
            case "Title":
                if (b_title == false)
                {
                    b_title = true;
                    //初期位置の設定
                    controller.speed1_all = 0.1f;
                    controller.accel = 0.1f;
                    controller.yaw = -1;
                    controller.heave = 1;
                }
                break;
            case "Entry_Scene":
                if (b_entry == false)
                {
                    b_entry = true;
                    //初期位置の設定
                    controller.speed1_all = 0.1f;
                    controller.accel = 0.1f;
                    controller.yaw = 0.0f;
                    controller.heave = 0.5f;
                }
                break;
            case "Result_Scene":
                if (b_result == false)
                {
                    b_result = true;
                    //初期位置の設定
                    controller.speed1_all = 0.1f;
                    controller.accel = 0.1f;
                    controller.yaw = -1;
                    controller.heave = 1;
                }
                break;

        }

    }

    //━━━━━━━━━━━━━━━━━━━━━
    // オブジェクトが消される時に呼ばれる関数
    //━━━━━━━━━━━━━━━━━━━━━
    private void OnDestroy()
    {
        // 椅子の位置をリセット
        ResetChair();
    }

    //━━━━━━━━━━━━━━━━━━━━━
    // 初期化用の関数
    //━━━━━━━━━━━━━━━━━━━━━
    public void Init()
    {
        controller = transform.gameObject.GetComponent<WIZMOController>();
        bEmergencyShutdown = false;
        // 椅子をリセット
        ResetChair();
    }

    //━━━━━━━━━━━━━━━━━━━━━
    // 椅子をリセット用の関数
    //━━━━━━━━━━━━━━━━━━━━━
    public void ResetChair()
    {
        // 速度
        controller.speed1_all = 0.4f;

        // 加速度
        controller.accel = 0.25f;

        // 前後
        controller.pitch = 0;

        // 左右
        controller.roll = 0;
        controller.yaw = 0;

        // 上下
        controller.heave = 0;

        // ほかの使っていない変数
        controller.sway = 0;
        controller.surge = 0;
    }

    //━━━━━━━━━━━━━━━━━━━━━
    // 椅子との接続をチェックする関数
    // 戻り値：使っていい場合はtrue、
    // 　　　　それ以外はfalse
    //━━━━━━━━━━━━━━━━━━━━━
    private bool CheckChairConnect()
    {
        // コントローラーが見つけない場合はこの後の処理をしない
        if (controller == null) return false;

        // 椅子が正常に動いているかをチェック
        bIsConnected = controller.isRunning();
        if (!bIsConnected)
        {
            // もし接続していない場合、
            // 600フレーム毎に接続してみる
            if (reconnectCnt++ >= 600)
            {
                // カウンターをリセット
                reconnectCnt = 0;

                // 再接続
                controller.OpenSIMVR();
                return false;
            }
            else return false;
        }
        return true;
    }

    //━━━━━━━━━━━━━━━━━━━━━
    // 椅子の緊急停止の関数
    // 戻り値：使っていい場合はtrue、
    // 　　　　それ以外はfalse
    //━━━━━━━━━━━━━━━━━━━━━
    public bool EmergencyShutdown()
    {
        // 通常状態
        if (!bEmergencyShutdown)
        {
            // キーが押されたら、緊急停止します
            if (Input.GetKeyDown(KeyCode.F7))
            {
                bEmergencyShutdown = true;

                // 値を戻す
                ResetChair();

                controller.CloseSIMVR();
                return false;
            }
            return true;
        }
        // 緊急停止中
        else
        {
            // もう一度押すと元に戻す
            if (Input.GetKeyDown(KeyCode.F7))
            {
                bEmergencyShutdown = false;

                // 値を戻す
                controller.OpenSIMVR();
            }

            return false;
        }
    }

    void NextLoad(Scene nextScene, LoadSceneMode mode)
    {
        if (nextScene.name == "Scene_Map")
        {
            SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
            SceneManager.LoadScene(nextScene.name);
            SceneManager.sceneLoaded -= NextLoad;
            Destroy(this.gameObject);
        }

        else if (nextScene.name == "Title")
        {
            SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
            SceneManager.LoadScene(nextScene.name);
            SceneManager.sceneLoaded -= NextLoad;
            Destroy(this.gameObject);
            b_result = false;
            b_title = false;
            b_entry = false;
        }

    }

}

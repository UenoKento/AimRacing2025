// 入力された車の位置(Pos)・回転(Rot)・時間(Time)を記録してログまたはテキストに出力するプログラム
// 作成者：23CU0325_土田修平
// 作成日：2025/05/29
// 追記日：2025/06/01 位置・回転・時間を配列に保存するように変更
// 追記日：2025/06/05 スクリプト再制作
// 追記日：2025/06/06 Excelが無いPCでもすぐに読めるようにCSVではなく、.txtで保存するように変更。
//      　.txtに正しく出力されない問題の修正。

using UnityEngine;
using System.IO;

public class SavePosRotTime : MonoBehaviour
{
    // 位置・回転・時間を記録する構造体配列
    public struct PosRotTime_Save
    {
        public Vector3 Save_pos;    // 位置
        public Vector3 Save_rot;   // 回転
        public float Save_time;    // 時間

        public PosRotTime_Save(Vector3 pos, Vector3 rot, float time)
        {
            Save_pos = pos;
            Save_rot = rot;
            Save_time = time;
        }
    }

    // 構造体配列宣言(1つで位置・回転・時間を記録する)
    public PosRotTime_Save[] _posRotTime;
    // 保存する最大値
    private const int MaxIndex = 100000;
    // 繰り返しの中で順番にカウントして保存するための変数(保存する配列の場所を1つずつ増やす)
    private int CurrentIndex = 0;
    // ボタン記録用フラグ
    private bool isRecord = false;

    // 毎フレーム保存すると容量がデカくなるので数フレームに一度だけ保存するようにする変数
    private int frameLimiter = 0;
    // 何フレームに一度だけ保存するかの変数(20フレームに1回保存する)
    private const int SaveFrameInterval = 20;

    // 保存するファイルのパス
    private string filepath = Application.dataPath + "PosRotTimeLog.csv";

    // ファイルパスを取得する関数
    public string GetFilePath() { return filepath; }
    // 最大値を取得する関数
    public int GetMaxIndex() { return MaxIndex; }
    public int GetCurrentIndex() { return CurrentIndex; }

    void Start()
    {
        // 構造体配列初期化
        _posRotTime = new PosRotTime_Save[MaxIndex];     
    }

    void Update()
    {
        // 押されていない場合returnする
        if (!Input.GetKeyDown(KeyCode.Space)) { return; }
        // フラグ切り替え(押すたびに切り替える)
        isRecord = !isRecord;

        // 記録を中断・終了したときにCSVファイルに保存する
        if (!isRecord)
        {
            string filepath = Application.dataPath + "/PosRotTimeLog.txt";
            SaveToFile(filepath);
        }
    }

    void FixedUpdate()
    {
        // キーが押されたら記録実行
        if(isRecord) { Recording(); }
    }

    // 車の位置・回転・時間を記録してログに表示するプログラム
    void Recording()
    {
        // 配列の最大値を超えたら記録を終了する
        if (CurrentIndex >= MaxIndex)
        {
            Debug.Log("記録終了: 配列の最大値に達しました。");
            isRecord = false;
            return;
        }
        // 0から開始しないようにする
        ++frameLimiter;

        // 数フレームに一度だけ配列に記録する
        if (frameLimiter % SaveFrameInterval == 0)
        {
            _posRotTime[CurrentIndex] = new PosRotTime_Save(transform.localPosition, transform.localEulerAngles, Time.time);
            Debug.Log("pos" + _posRotTime[CurrentIndex].Save_pos);
            Debug.Log("rot" + _posRotTime[CurrentIndex].Save_rot);
            Debug.Log("time" + _posRotTime[CurrentIndex].Save_time);
            ++CurrentIndex;
        }
    }

    protected void SaveToFile(string filename)
    {
        using(StreamWriter writer = new StreamWriter(filename))
        {
            for(int i = 0; i < CurrentIndex; i++)
            {
                // 配列の位置・回転・時間を.txt形式で保存
                writer.WriteLine(_posRotTime[i].Save_pos.ToString());
                writer.WriteLine(_posRotTime[i].Save_rot.ToString());
                writer.WriteLine(_posRotTime[i].Save_time.ToString());
                writer.WriteLine(); // 空行で区切り
            }
        }
        Debug.Log("保存完了!" + filename);
    }

    // txt形式の文字列を構造体に変換する静的メソッド
    public static PosRotTime_Save FromCSV(string line)
    {
        var values = line.Split(',');
        if (values.Length < 7)
        {
            Debug.LogWarning("txtの列数が不足しています");
            return new PosRotTime_Save();
        }

        return new PosRotTime_Save(
            new Vector3(
                float.Parse(values[0]),
                float.Parse(values[1]),
                float.Parse(values[2])
            ),
            new Vector3(
                float.Parse(values[3]),
                float.Parse(values[4]),
                float.Parse(values[5])
            ),
            float.Parse(values[6])
        );
    }
}

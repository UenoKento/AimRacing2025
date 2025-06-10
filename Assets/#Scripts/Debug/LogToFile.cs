// 車の位置と時間を保存して記録するプログラム
// 作成者：23CU0325_土田修平
// 作成日：2025/06/01
// 追加日：2025/06/02 

using System.IO;
using UnityEngine;

public class LogToFile : TransformLogData
{
    private string filePath;        // 書き出し先のファイル

    [SerializeField]
    private const int MaxLogSize = 100000; // ログの最大サイズ

    void Start()
    {
        // 書き出し先を指定
        filePath = Path.Combine(Application.persistentDataPath, "debug_log.txt");
        Debug.Log($"Log file path: {Application.persistentDataPath}/debug_log.txt");

        // ファイルの初期化
        using (StreamWriter writer = new StreamWriter(filePath, false))
        {
            writer.WriteLine("=== Debug Log Start ===");
        }
    }

    void FixedUpdate()
    {

    }

    void SaveLogText()
    {
        using (StreamWriter writer = new StreamWriter(filePath, true))
        {
            for (int i = 0; i < _index; i++)
            {
                // 各情報をファイルに書き出す
                writer.WriteLine(savePosAndTime._transform);
                writer.WriteLine(savePosAndTime._rotation);
                writer.WriteLine(savePosAndTime._timer);
            }
        }
    }

    private void OnApplicationQuit()
    {
        // アプリケーション終了時にログを保存
        //SaveLogText();
        Debug.Log("Debug log saved to " + filePath);
    }
}
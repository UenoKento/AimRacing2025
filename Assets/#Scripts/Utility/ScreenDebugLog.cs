// https://zenn.dev/daichi_gamedev/articles/unity-debuglog
using UnityEngine;

public class ScreenDebugLog : MonoBehaviour
{
	[SerializeField]
	private int MaxLogLines = 10; // 表示するログの最大行数
	[SerializeField]
	private float clearInterval = 3f;

	private string logText = "";
	private GUIStyle guiStyle = new GUIStyle();
	private bool showLogInGame = false;
	private bool isEditor = false;

	private float lastLogTime = 0f;

	private void Start()
	{
		// ログのテキストをスタイルに設定
		guiStyle.fontSize = 24;
		guiStyle.normal.textColor = Color.green;

		// エディタで実行していない場合のみ、ゲーム画面内のログを表示
		isEditor = Application.isEditor;
	}

	private void OnGUI()
	{
		// ゲーム画面中にログを表示（Windowsのビルド時のみ有効かつエディタで実行していない場合のみ有効かつ0キーで表示/非表示を切り替え）
#if UNITY_STANDALONE_WIN
		if (showLogInGame && !isEditor)
		{
			GUI.Label(new Rect(10, 10, Screen.width, Screen.height), logText, guiStyle);
		}
#endif
	}

	private void OnEnable()
	{
		// デバッグログを表示するためのイベントハンドラを登録
		Application.logMessageReceived += HandleLog;
	}

	private void OnDisable()
	{
		// イベントハンドラを解除
		Application.logMessageReceived -= HandleLog;
	}

	private void Update()
	{
		// ゲーム画面内のログ表示が有効な場合のみ、3秒ごとにログのテキストをクリア
		if (showLogInGame && Time.time - lastLogTime > clearInterval)
		{
			logText = "";
		}

		// デバッグキー
		// F8キーで切り替え
		if (Input.GetKeyDown(KeyCode.F8))
		{
			showLogInGame = !showLogInGame;
		}
	}

	private void HandleLog(string logString, string stackTrace, LogType type)
	{
		// Debug.Log()のテキストをlogTextに追加
		logText += logString + "\n";

		// 表示するログの行数がMaxLogLinesを超えたら、古いログを削除
		string[] logLines = logText.Split('\n');
		if (logLines.Length > MaxLogLines)
		{
			logText = string.Join("\n", logLines, logLines.Length - MaxLogLines, MaxLogLines);
		}

		// 最後にログを表示した時刻を更新
		lastLogTime = Time.time;
	}
}


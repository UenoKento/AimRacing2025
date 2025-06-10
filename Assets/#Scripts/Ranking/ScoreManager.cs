//===================================================
// ファイル名	：ScoreManager.cs
// 概要			：ランキング用のスコアマネージャー
// 作成者		：熊彦哲
// 作成日		：
//===================================================
// 更新履歴     ：
//
//
//===================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    #region HIGH SCORE SYSTEM
    [SerializeField]
    string scoreText = "--:--:---";
    public string ScoreText
    {
        set => scoreText = value;
    }

    int maxRankCounter = 10;
    private static ScoreManager instance;
    public static ScoreManager Instance { get { return instance; } }
    private Animator RankingAnim;

    public int playerRanking = 0;
    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }
    public void Start()
    {
        RankingAnim = GetComponent<Animator>();
        RankingAnim.SetFloat(playerRanking, 0);
        playerRanking = 0;
        SavePlayerScoreData();
    }
    [System.Serializable]
    public class PlayerScore
    {
        public string scoreText;

        public PlayerScore(string scoreText)
        {
            this.scoreText = scoreText;           
        }
    }

    [System.Serializable]
    public class PlayerScoreData
    {
        public List<PlayerScore> list = new List<PlayerScore>();
    }

    readonly string SaveFileName = "Player_Ranking2024.sav";

    /*
     * 型と名前：void SavePlayerScoreData
     * 仕様：セーブデータを書き込む
     * 引数：なし
     * 出力：なし
     */
    public void SavePlayerScoreData()
    {
        var playerScoreData = LoadPlayerScoreData();

        playerScoreData.list.Add(new PlayerScore(scoreText));
        Sort(playerScoreData);
        playerRanking = GetPlayerRanking(playerScoreData);

        SaveSystem.SaveByJson(SaveFileName, playerScoreData);

        GetComponentInParent<ScoringUIController>().ShowScoringScreen();
    }

    public void Sort(PlayerScoreData playerScoreData)
    {
        int len = playerScoreData.list.Count;
        for (int i = 1; i < len; ++i) 
        {
            for (int j = 0; j < len - i; ++j) 
            {
                int temp = Compare(playerScoreData.list[j], playerScoreData.list[j + 1]);
                if(temp == 1)
                {
                    var temp2 = playerScoreData.list[j + 1];
                    playerScoreData.list[j + 1] = playerScoreData.list[j];
                    playerScoreData.list[j] = temp2;
                }
            }
        }
    }

    /*
     * 型と名前：int Compare
     * 仕様：ソート用関数
     * 引数：なし
     * 出力：なし
     */
    public int Compare(PlayerScore _dataA, PlayerScore _dataB)
    {
        if (_dataA.scoreText == "--:--:---") return 1;
        if (_dataB.scoreText == "--:--:---") return -1;

        char[] dataAChar = _dataA.scoreText.ToCharArray();
        char[] dataBChar = _dataB.scoreText.ToCharArray();

        for (int i = 0; i < 8; ++i)
        {
            if (dataAChar[i] > dataBChar[i]) return 1;
            else if (dataAChar[i] < dataBChar[i]) return -1;
        }
        return 0;
    }

    /*
    * 型と名前：PlayerScoreData LoadPlayerScoreData
    * 仕様：セーブデータを読み込む
    * 引数：なし
    * 出力：PlayerScoreData
    */
    public PlayerScoreData LoadPlayerScoreData()
    {
        var playerScoreData = new PlayerScoreData();

        if (SaveSystem.SaveFileExists(SaveFileName))
        {
            playerScoreData = SaveSystem.LoadFromJson<PlayerScoreData>(SaveFileName);
        }
        else
        {
            while (playerScoreData.list.Count < maxRankCounter)
            {
                playerScoreData.list.Add(new PlayerScore("--:--:---"));
            }

            SaveSystem.SaveByJson(SaveFileName, playerScoreData);
        }

        return playerScoreData;
    }

    private int GetPlayerRanking(PlayerScoreData _playerScoreData)
    {
        if (playerRanking != 0) return playerRanking;
        int len = _playerScoreData.list.Count;
        if (len > 10) len = 10;
        for (int i = 0; i < len; ++i)
        {
            if (_playerScoreData.list[i].scoreText == scoreText)
            {
                playerRanking = i + 1;
                RankingAnim.SetInteger("MyRank", playerRanking);
                return i + 1;
            }
        }
        playerRanking = -1;
        return -1;
    }

    //public string GetName()
    //{
    //    return 
    //}
 
    #endregion
}

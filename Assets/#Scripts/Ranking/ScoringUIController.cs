//===================================================
// ファイル名	：ScoringUIController.cs
// 概要			：
// 作成者		：
// 作成日		：
//===================================================
// 更新履歴     ：
//
//
//===================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoringUIController : MonoBehaviour
{
    [Header("----------- SCORING SCREEN -----------")]

    [SerializeField] Canvas scoringScreenCanvas;

    [SerializeField] Transform rankingContainer;

    List<ScoreManager.PlayerScore> playerScoreList;

    /*
     * 型と名前：void ShowScoringScreen
     * 仕様：ランキング画面が表示される
     * 引数：なし
     * 出力：なし
     */
    public void ShowScoringScreen()
    {
        scoringScreenCanvas.enabled = true;
        UpdateHighScoreLeaderboard();
    }

    /*
     * 型と名前：void UpdateHighScoreLeaderboard
     * 仕様：ランキング更新
     * 引数：なし
     * 出力：なし
     */
    void UpdateHighScoreLeaderboard()
    {
        var playerScoreList = ScoreManager.Instance.LoadPlayerScoreData().list;

        for (int i = 0; i < rankingContainer.childCount; i++)
        {
            var child = rankingContainer.GetChild(i);

            child.Find("Rank").GetComponent<Text>().text = (i + 1).ToString();
            child.Find("Score").GetComponent<Text>().text = playerScoreList[i].scoreText.ToString();
        }
    }
}
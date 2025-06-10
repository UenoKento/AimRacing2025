//===================================================
// �t�@�C����	�FScoringUIController.cs
// �T�v			�F
// �쐬��		�F
// �쐬��		�F
//===================================================
// �X�V����     �F
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
     * �^�Ɩ��O�Fvoid ShowScoringScreen
     * �d�l�F�����L���O��ʂ��\�������
     * �����F�Ȃ�
     * �o�́F�Ȃ�
     */
    public void ShowScoringScreen()
    {
        scoringScreenCanvas.enabled = true;
        UpdateHighScoreLeaderboard();
    }

    /*
     * �^�Ɩ��O�Fvoid UpdateHighScoreLeaderboard
     * �d�l�F�����L���O�X�V
     * �����F�Ȃ�
     * �o�́F�Ȃ�
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
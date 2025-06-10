//===================================================
// �t�@�C����	�FScoreManager.cs
// �T�v			�F�����L���O�p�̃X�R�A�}�l�[�W���[
// �쐬��		�F�F�F�N
// �쐬��		�F
//===================================================
// �X�V����     �F
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
     * �^�Ɩ��O�Fvoid SavePlayerScoreData
     * �d�l�F�Z�[�u�f�[�^����������
     * �����F�Ȃ�
     * �o�́F�Ȃ�
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
     * �^�Ɩ��O�Fint Compare
     * �d�l�F�\�[�g�p�֐�
     * �����F�Ȃ�
     * �o�́F�Ȃ�
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
    * �^�Ɩ��O�FPlayerScoreData LoadPlayerScoreData
    * �d�l�F�Z�[�u�f�[�^��ǂݍ���
    * �����F�Ȃ�
    * �o�́FPlayerScoreData
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

/*
 * �t�@�C�����FResultTimeText.cs
 * �T�@�@�@�v�F���U���g��ʂŃ^�C���\��
 * ��@���@�ҁF20CU0213�@���ё�P
 * ��@���@���F2022/05/26
 */

/*
 * �X�V�����F
 * 2022/05/26�@[���ё�P]�@�e�L�X�g���󂯎��\��
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SocialPlatforms.Impl;

public class ResultTImeText : MonoBehaviour
{
    public TextMeshProUGUI ResultTimeText;//�ŏI�^�C���̕�������Ǘ�����ϐ�
    public Text ResultText;
    //private GoalJudgment goaljudge = null;//GolaJudgment�N���X�I�u�W�F�N�g
    //2024/2/5/��{����ǋL
    private string accessKey;//GAS�A�N�Z�X�L�[
    private string score = "";
    private string tempdata = "";
    private bool DataPosted = false;
    //private GoalJudgment Timeobj;

    // Start is called before the first frame update
    void Start()
    {
        //��{����ǋL
        DataPosted = false;
        
        accessKey = "https://script.google.com/macros/s/" + "AKfycbzD7D-O4dA0N4dbPjlI1eCgcJXfHlvMWdGjD9_KwFavuzOPEV9x0MKmHrVToQgvXEOZ" + "/exec";
        //-------------
        //goaljudge = new GoalJudgment();
        if(ResultTimeText)
        {
            //ResultTimeText.text = goaljudge.getResultTime();//���U���g�^�C�����擾
        }
        
        //ResultText.text = goaljudge.getResultTime();
        //score= goaljudge.getResultTime();
        //ResultText.text = score;
        //Timeobj = new GoalJudgment();
        //score = Timeobj.getResultTime().Replace(":", ".");
        //score = Timeobj.getResultTime();
        //score = score.Replace(".", "");


        //��{����ǋL
        //Debug.Log("�R���[�`���O��Time" + Timeobj.getTimeText());
        //StartCoroutine(PostData(ResultText.text));
        StartCoroutine(PostData(score));
        //-------------
    }

    //��{����ǋL------------------
    //�X�v���b�h�V�[�g�ɑ���f�[�^�̏���������
    private IEnumerator PostData(string db_time)
    {
        if (DataPosted == false)
        {
            tempdata = db_time;
            DataPosted = true;
        }
        //readonly string timescore = db_time;
        //�^�C���̎擾
        //���O�̎擾
        //string username = Entry.GetPlayerName();
        //if (username == "")
        //{
        //    username = "Guest";
        //}

        //Debug.Log("���M����f�[�^�̏����Ftime = " + tempdata + ",name = " + username);

        Debug.Log("�f�[�^���M�J�n�E�E�E");
        var form = new WWWForm();
        form.AddField("time", tempdata);
        //form.AddField("time", "04:00:01");
        //form.AddField("name", username);

        //var request = UnityWebRequest.Post(accessKey, form);

        //yield return request.SendWebRequest();

        using (UnityWebRequest request = UnityWebRequest.Post(accessKey, form))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("�f�[�^���M����");
                //Debug.Log("���M�����f�[�^�Ftime = " + tempdata + ",name = " + username);
            }
            else
            {
                Debug.Log("�f�[�^���M���s");
            }
        }
    }
    //--------------------------
}
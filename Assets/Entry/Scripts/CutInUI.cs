using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class CutInUI : MonoBehaviour
{
    [Header("アクセル押す用オブジェクト")]
    //public GameObject ResultForce;
    //AIM.ForcedOnAccel Script;
    public GameObject NewObject;
    [Header("カットイン番号順")]
    public int Counter;
    public int NowCounter;
    public G29 g29;
    // Start is called before the first frame update
    void Start()
    {
        NowCounter = 0;
        //Script = ResultForce.GetComponent<AIM.ForcedOnAccel>();
    }
    private void Update()
    {
        if (((g29.rec.lY / (float)-Int16.MaxValue + 1.0f) * 0.5f) >= 0.6f)
        {
            NowCounter++;
            if (NowCounter > Counter)
            {
                NowCounter = Counter;
            }
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            NowCounter++;
            if (NowCounter > Counter)
            {
                NowCounter = Counter;
            }
        }
        if (NowCounter == Counter)
        {
            NewObject.SetActive(true);
            Destroy(this);
        }
    }
}


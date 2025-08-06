using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCubeSystem : MonoBehaviour
{
    public float Accelrate = 0.1f;
    public float StartSpeed = 1.0f;
    private float CurSpeed = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        CurSpeed = StartSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            this.transform.Translate(0.0f, 0.0f, CurSpeed * Time.deltaTime);
            CurSpeed += Accelrate;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            this.transform.Translate(0.0f, 0.0f, -CurSpeed * Time.deltaTime);
            CurSpeed += Accelrate;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            this.transform.Translate(CurSpeed * Time.deltaTime, 0.0f, 0.0f);
            CurSpeed += Accelrate;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            this.transform.Translate(-CurSpeed * Time.deltaTime, 0.0f, 0.0f);
            CurSpeed += Accelrate;
        }else
        {
            CurSpeed = StartSpeed;
        }

    }
}

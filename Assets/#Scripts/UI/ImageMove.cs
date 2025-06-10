using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ImageMove : MonoBehaviour
{
    //変数宣言
    [SerializeField] private GameObject ScriptObject;
    [SerializeField] private Image MyImage; //UnityのImageを設定するための変数
    [Header("フェードインスビート設定")]
    [SerializeField] private float FadeInSpeed;
    [Header("フェードアウトスビート設定")]
    [SerializeField] private float FadeOutSpeed;
    [Header("-----------座標移動---------")]
    [Header("目標座標")]
    [SerializeField] private Vector3 TargetPosition;
    [Header("移動時間")]
    [SerializeField] private float MoveTime;
    [Header("移動速度")]
    [SerializeField] private float Speed;

    //[Header("-----------サイズ変更---------")]
    //[Header("初期サイズ")]
    //[SerializeField] private float MaxSize;
    //[Header("目標サイズ")]
    //[SerializeField] private float MinSize;
    //[Header("縮小速度(%)")]
    //[SerializeField] private float ReduceSpeed;

    private float Size;                                                                                                                                                                                                                                        //拡大率
    private float FadeAlpha = 0;    //透明度を入れる変数
    private bool isFade = false;
    private float ScriptTime = 0.0f;
    private RectTransform Rect;
    private Vector3 MyObjectPosition;   //現在UIの座標
    private Vector3 MoveSpeed;
    // スタートボタンを押したら実行される
    void Start()
    {
        ScriptTime = 0.0f;
        Rect = ScriptObject.GetComponent<RectTransform>();
        MyObjectPosition = ScriptObject.GetComponent<RectTransform>().position;
        MoveSpeed = SetMoveSpeed(TargetPosition, MyObjectPosition, MoveSpeed, MoveTime);
        //Size = MaxSize;
        //ScriptObject.transform.localScale = new Vector2(MaxSize, MaxSize);
    }

    // Update is called once per frame
    void Update()
    {
        ScriptTime += Time.deltaTime;
        if (ScriptTime <= 0.3)
        {
            fadeImage(MyImage);
        }
        //else if (ScriptTime < MoveTime)
        //{
        //    moveImage(Rect);
        //    sizeUpdate(MinSize);
        //}
        //else
        //{
        //
        //}
    }

    void fadeImage(Image MyImage)
    {
        //Fadeがfalseの場合、α値(透明度)を一定の速度で変える
        if ((!isFade) && (FadeInSpeed > 0))
        {
            //画像の色を白にしてα値をFadeAlphaで管理
            MyImage.color = new Color(255, 255, 255, FadeAlpha);

            //デルタタイムの2分の1をα値から引いていく
            FadeAlpha += Time.deltaTime / FadeInSpeed;

            //α値が0以下になったら1に戻す。
            if (FadeAlpha >= 1)
            {
                isFade = true;
            }
        }
        if ((isFade) && (FadeOutSpeed > 0))
        {
            //画像の色を白にしてα値をFadeAlphaで管理
            MyImage.color = new Color(255, 255, 255, FadeAlpha);

            //デルタタイムの2分の1をα値から引いていく
            FadeAlpha -= Time.deltaTime / FadeOutSpeed;

            //α値が0以下になったら1に戻す。
            if (FadeAlpha <= 0)
            {
                isFade = false;
            }
        }
    }

    //void sizeUpdate(float MinSize)
    //{
    //    //サイズ変更
    //    if (Size > MinSize)
    //    {
    //        Size -= Time.deltaTime * ReduceSpeed / 100;
    //        ScriptObject.transform.localScale = new Vector2(Size, Size);
    //    }
    //}

    //移動速度の設定
    Vector3 SetMoveSpeed(Vector3 Position, Vector3 MyObjectPosition, Vector3 MoveSpeed, float MoveTime)
    {
        if (Position.x - MyObjectPosition.x > 0)
        {
            MoveSpeed.x = 1.0f;
        }
        else
        {
            MoveSpeed.x = 0.0f;
        }
        if (Position.y - MyObjectPosition.y > 0)
        {
            MoveSpeed.y = 1.0f;
        }
        else
        {
            MoveSpeed.y = 0.0f;
        }
        if (Position.z - MyObjectPosition.z > 0)
        {
            MoveSpeed.z = 1.0f;
        }
        else
        {
            MoveSpeed.z = 0.0f;
        }
        //MoveSpeed.x = (Position.x - MyObjectPosition.x) / MoveTime;
        //MoveSpeed.y = (Position.y - MyObjectPosition.y) / MoveTime;
        //MoveSpeed.z = (Position.z - MyObjectPosition.z) / MoveTime;

        return MoveSpeed;

    }
    void moveImage(RectTransform Rect)
    {
        Rect.position += MoveSpeed * Time.deltaTime * Speed;
    }
}
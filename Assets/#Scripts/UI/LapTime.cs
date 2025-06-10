using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class LapTime : MonoBehaviour
{
	[SerializeField] private Texture img_num;       // 数字の画像
	[Space(10)]

	[SerializeField] private RawImage[] imgArray = null; // 画像の配列
	[Space(10)]

	[SerializeField] private Vector2 pos;           // 画像配列の中心座標
	[SerializeField] public float StartSpacing=-30f;		// 文字の間隔
	[SerializeField] private Vector2 Size;			// 文字の画像のサイズ
	[Header("-----------サイズ変更---------")]
	[Header("初期スケール")]
	[SerializeField] private float MaxScale;
	[Header("目標スケール")]
	[SerializeField] private float MinScale;
	[Header("縮小速度(%)")]
	[SerializeField] private float ReduceSpeed;
	private float Scale;

	private bool bFlashing = false;
	private float cnt_flashing;
	private float flashingTime;
	private float flashingSpeed;

	private bool bMoving = false;
	private Vector2 moveTarget;
	private float moveTime;


	// 数字のUV座標
	// (左, 上, サイズx, サイズy)
	public static Rect[] UV = new Rect[12];

    private void Start()
    {
		UV[0] = new Rect(0.0f / 5.0f, 2.0f / 3.0f, 1.0f / 5.0f, 1.0f / 3.0f);  // 0
		UV[1] = new Rect(1.0f / 5.0f, 2.0f / 3.0f, 1.0f / 5.0f, 1.0f / 3.0f);  // 1
		UV[2] = new Rect(2.0f / 5.0f, 2.0f / 3.0f, 1.0f / 5.0f, 1.0f / 3.0f);  // 2
		UV[3] = new Rect(3.0f / 5.0f, 2.0f / 3.0f, 1.0f / 5.0f, 1.0f / 3.0f);  // 3
		UV[4] = new Rect(4.0f / 5.0f, 2.0f / 3.0f, 1.0f / 5.0f, 1.0f / 3.0f);  // 4

		UV[5] = new Rect(0.0f / 5.0f, 1.0f / 3.0f, 1.0f / 5.0f, 1.0f / 3.0f);  // 5
		UV[6] = new Rect(1.0f / 5.0f, 1.0f / 3.0f, 1.0f / 5.0f, 1.0f / 3.0f);  // 6
		UV[7] = new Rect(2.0f / 5.0f, 1.0f / 3.0f, 1.0f / 5.0f, 1.0f / 3.0f);  // 7
		UV[8] = new Rect(3.0f / 5.0f, 1.0f / 3.0f, 1.0f / 5.0f, 1.0f / 3.0f);  // 8
		UV[9] = new Rect(4.0f / 5.0f, 1.0f / 3.0f, 1.0f / 5.0f, 1.0f / 3.0f);  // 9
																			   
		UV[10] = new Rect(0.0f / 5.0f, 0.0f / 3.0f, 1.0f / 5.0f, 1.0f / 3.0f); // :
		UV[11] = new Rect(1.0f / 5.0f, 0.0f / 3.0f, 1.0f / 5.0f, 1.0f / 3.0f); // /

		Init();

		//transform.position = pos;
		Scale = MaxScale;
		transform.localScale = new Vector2(MaxScale, MaxScale);
	}

    //━━━━━━━━━━━━━━━━━━━━
    // 毎フレームの更新
    //━━━━━━━━━━━━━━━━━━━━
    private void Update()
	{
		if(bFlashing)
		{
			cnt_flashing += Time.deltaTime;
			float Alpha;
			if(cnt_flashing >= flashingTime)
			{
				Alpha = 1;
				bFlashing = false;
			}
			else
			{
				Alpha = Mathf.Abs(Mathf.Cos(cnt_flashing * flashingSpeed));
			}

			foreach(RawImage img in imgArray)
			{
				var temp = img.color;
				temp.a = Alpha;
				img.color = temp;
			}
		}
		if(bMoving)
		{
			Vector2 newPos;

			if(moveTime < Time.deltaTime)
			{
				newPos = moveTarget;
				bMoving = false;
			}
			else
			{
				newPos = pos + (moveTarget - pos) * Time.deltaTime / moveTime;
				moveTime -= Time.deltaTime;
			}
			pos = newPos;

			transform.position = newPos;

			ScaleUpdate(MinScale);
		}

		
	}

    public void Init()
    {
		imgArray = new RawImage[9];
		imgArray[0] = transform.Find("min1").GetComponent<RawImage>();
		imgArray[1] = transform.Find("min2").GetComponent<RawImage>();

		imgArray[2] = transform.Find("colon1").GetComponent<RawImage>();

		imgArray[3] = transform.Find("sec1").GetComponent<RawImage>();
		imgArray[4] = transform.Find("sec2").GetComponent<RawImage>();

		imgArray[5] = transform.Find("colon2").GetComponent<RawImage>();

		imgArray[6] = transform.Find("millisec1").GetComponent<RawImage>();
		imgArray[7] = transform.Find("millisec2").GetComponent<RawImage>();
		imgArray[8] = transform.Find("millisec3").GetComponent<RawImage>();
	}

	//━━━━━━━━━━━━━━━━━━━━
	// 表示する時間をセットする関数
	//━━━━━━━━━━━━━━━━━━━━
	public void SetTime(int _min, int _sec, int _millisec)
	{
		//Init();

		// min
		int min = Mathf.Abs(_min);
		imgArray[0].uvRect = UV[min / 10 % 10];
		imgArray[1].uvRect = UV[min % 10];

		// sec
		int sec = Mathf.Abs(_sec);
		imgArray[3].uvRect = UV[sec / 10 % 10];
		imgArray[4].uvRect = UV[sec % 10];

		// sec
		int millisec = Mathf.Abs(_millisec);
		imgArray[6].uvRect = UV[millisec / 100 % 10];
		imgArray[7].uvRect = UV[millisec / 10 % 10];
		imgArray[8].uvRect = UV[millisec % 10];

		// :
		imgArray[2].uvRect = UV[10];
		imgArray[5].uvRect = UV[10];

		
		for (int i = 0; i < 9; ++i)
		{
			Vector2 temp = new Vector2(0, 0);
			temp.x += (i - 4) * StartSpacing + (i - 4) * Size.x;
			imgArray[i].transform.position = temp;
		}

		transform.position = pos;

		foreach(RawImage img in imgArray)
        {
			img.texture = img_num;
		}
	}

	//━━━━━━━━━━━━━━━━━━━━
	// 点滅させる関数
	//━━━━━━━━━━━━━━━━━━━━
	public void Flashing(float _time, float _speed = 1)
	{
		bFlashing = true;
		flashingTime = _time;
		flashingSpeed = _speed;
		cnt_flashing = 0;
	}

	//━━━━━━━━━━━━━━━━━━━━
	// 指定された時間で指定の座標に移動させる関数
	//━━━━━━━━━━━━━━━━━━━━
	public void MoveTo(Vector2 _targetPos, float _time)
	{
		bMoving = true;
		moveTarget = _targetPos;
		moveTime = _time;
		pos = transform.position;
	}

	//━━━━━━━━━━━━━━━━━━━━
	// 指定された時間でゲームオブジェクトを縮小させる関数
	//━━━━━━━━━━━━━━━━━━━━
	public　void ScaleUpdate(float MinScale)
	{
		float tmp;
		tmp = Time.deltaTime * ReduceSpeed / 100;
		//サイズ変更
		if ((Scale - tmp) > MinScale)
		{
			Scale -= tmp;
			transform.localScale = new Vector2(Scale, Scale);
		}
		else
        {
			Scale = MinScale;
			transform.localScale = new Vector2(Scale, Scale);
		}
	}
}

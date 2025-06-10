using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageFade : MonoBehaviour
{
    [SerializeField] private Image image;

    [SerializeField] private bool bInOut;   // フェードイン・アウト切り替え
    [SerializeField] private float fadeTime;

    private float imageAlpha;

    // Start is called before the first frame update
    void Start()
    {
        // フェードイン
        if(bInOut)
        {
            imageAlpha = 1;
        }
        // フェードアウト
        else
        {
            imageAlpha = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // フェードイン
        if(bInOut)
        {
            //Fadeがfalseの場合、α値(透明度)を一定の速度で変える
            if (imageAlpha > 0)
            {
                imageAlpha -= Time.deltaTime / fadeTime;

                //画像の色を白にしてα値をFadeAlphaで管理
                image.color = new Color(image.color.r, image.color.g, image.color.b, imageAlpha);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
        // フェードアウト
        else
        {
            //Fadeがfalseの場合、α値(透明度)を一定の速度で変える
            if (imageAlpha < 1)
            {
                imageAlpha += Time.deltaTime / fadeTime;

                //画像の色を白にしてα値をFadeAlphaで管理
                image.color = new Color(image.color.r, image.color.g, image.color.b, imageAlpha);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }
}

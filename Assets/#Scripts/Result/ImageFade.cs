using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageFade : MonoBehaviour
{
    [SerializeField] private Image image;

    [SerializeField] private bool bInOut;   // �t�F�[�h�C���E�A�E�g�؂�ւ�
    [SerializeField] private float fadeTime;

    private float imageAlpha;

    // Start is called before the first frame update
    void Start()
    {
        // �t�F�[�h�C��
        if(bInOut)
        {
            imageAlpha = 1;
        }
        // �t�F�[�h�A�E�g
        else
        {
            imageAlpha = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // �t�F�[�h�C��
        if(bInOut)
        {
            //Fade��false�̏ꍇ�A���l(�����x)�����̑��x�ŕς���
            if (imageAlpha > 0)
            {
                imageAlpha -= Time.deltaTime / fadeTime;

                //�摜�̐F�𔒂ɂ��ă��l��FadeAlpha�ŊǗ�
                image.color = new Color(image.color.r, image.color.g, image.color.b, imageAlpha);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
        // �t�F�[�h�A�E�g
        else
        {
            //Fade��false�̏ꍇ�A���l(�����x)�����̑��x�ŕς���
            if (imageAlpha < 1)
            {
                imageAlpha += Time.deltaTime / fadeTime;

                //�摜�̐F�𔒂ɂ��ă��l��FadeAlpha�ŊǗ�
                image.color = new Color(image.color.r, image.color.g, image.color.b, imageAlpha);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }
}

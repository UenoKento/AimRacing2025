using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
[DefaultExecutionOrder(100000)]

//[ExecuteInEditMode]
public class SpriteAnimationNew : MonoBehaviour
{
	[SerializeField] bool playOnAwake;
	[SerializeField] SpriteRenderer spr;
	[SerializeField] Image img;
	[SerializeField] bool useSprite = false;
	[SerializeField] List<Sprite> sprites = new List<Sprite>();
	[SerializeField] float animationTime = 5.0f;
	[SerializeField] bool isRoop = false;

	public int spriteNumber = 0;   //!< ���̉摜�ԍ�

	private IEnumerator awakeAnim;

	void Start()
	{
		StartCoroutine(Animation());
		
		if (useSprite)
		{
			if (spr == null)
			{
				spr = (GetComponent<SpriteRenderer>() != null)
				? GetComponent<SpriteRenderer>() : gameObject.AddComponent<SpriteRenderer>();
			}
		}

		if (playOnAwake)
		{
			awakeAnim = Animation();
			StartCoroutine(awakeAnim);
		}
	}
	public void StopAwakeAnim()
	{
		if (playOnAwake)
		{
			StopCoroutine(awakeAnim);
			playOnAwake = false;
		}
	}

	public IEnumerator Animation()
	{
		int countUp = 0;        //!< �߂��閇��
		int countMax = sprites.Count;   //!< �ő吔
		float waitTime = animationTime / countMax;  //!< �҂�����

		//!< �߂��閇��������
		while (Time.deltaTime > waitTime * countUp)
		{
			++countUp;
		}

		//!< �摜���A�j���[�V����������
		while (true)
		{
			if (useSprite)
			{
				spr.sprite = sprites[spriteNumber];
			}
			else
			{
				img.sprite = sprites[spriteNumber];
			}
			spriteNumber += countUp;

			//!< waitTime�b�҂�
			//yield return new WaitForSeconds(waitTime);

			if (countMax <= spriteNumber)
			{
				if (isRoop)
				{
					spriteNumber -= countMax;
				}
				else
				{
					break;
				}
			}
			yield return new WaitForSeconds(waitTime);
		}
		
	}

}
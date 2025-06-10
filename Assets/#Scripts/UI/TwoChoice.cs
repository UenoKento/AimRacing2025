/**
 * @file	TwoChoice.cs
 * @brief	UI�œ���̎��Ɏg����@�\
 * @author	�쐬�ҁF�s��
 *			�X�V�ҁF22CU0219 ��ؗF��
 */

using UnityEngine;
using UnityEngine.UI;
using System;

public class TwoChoice : MonoBehaviour
{
	enum ChoiceType
	{ 
		Left = -1,
		None = 0,
		Right = 1,
	}

	//�v���p�e�B������������������������������������������������������������������������������
	[Header("left�A�C�R��")]
	[SerializeField] Transform leftIcon;
	[Header("right�A�C�R��")]
	[SerializeField] Transform rightIcon;
	[Header("�ŏ��g�嗦")]
	[SerializeField] float minSize;
	[Header("�ő�g�嗦")]
	[SerializeField] float maxSize;
	[Header("left�A�C�R���̐�����")]
	[SerializeField] GameObject leftIconsDescription;
	[Header("right�A�C�R���̐�����")]
	[SerializeField] GameObject rightIconsDescription;
	[Header("�I���J���[")]
	[SerializeField] Color choicedColor;
	[Header("���I���J���[")]
	[SerializeField] Color unChoicedColor;
	[Header("�f�t�H���g�őI�����Ă����")]
	Image leftImage;
	Image rightImage;

	[Header("�J�n�܂ł̑ҋ@����")]
	[SerializeField] float waitTime = 1f;
	float activeTime;

	[Header("�u�A�N�Z������Łv�܂ł̎���")]
	[SerializeField] float pushGasTime = 1f;
	float lastSelectTime;

	[SerializeField,ShowInInspector]
	ChoiceType nowChoiceNum = 0;
	ChoiceType prevChoiceNum = 0;

	

	public void Start()
	{
		nowChoiceNum = 0;       //-1�F�� 0:���I���@1�F�E
		prevChoiceNum = 0;
		leftImage = leftIcon.gameObject.GetComponent<Image>();
		rightImage = rightIcon.gameObject.GetComponent<Image>();
		unChoicedColor.a = 1.0f;

		leftIcon.transform.localScale = new Vector2(minSize, minSize);
		rightIcon.transform.localScale = new Vector2(minSize, minSize);
		leftImage.color = unChoicedColor;
		rightImage.color = unChoicedColor;
		leftIconsDescription.SetActive(false);
		rightIconsDescription.SetActive(false);

		// �X�^�[�g���ԕێ�
		activeTime = Time.time;
	}
	private void Update()
	{ 
		if (Time.time - activeTime > waitTime)
		{
			Choice();
		}
	}

	public void Choice()
	{
		// �I��ԍ��ɂ���ĉ摜���g��
		switch (nowChoiceNum)
		{
			case ChoiceType.Left:
				leftIcon.localScale = new Vector2(maxSize, maxSize);
				leftIconsDescription.SetActive(true);
				rightIconsDescription.SetActive(false);
				rightIcon.localScale = new Vector2(minSize, minSize);

				leftImage.color = choicedColor;
				rightImage.color = unChoicedColor;
				break;
			case ChoiceType.Right:
				rightIcon.localScale = new Vector2(maxSize, maxSize);
				rightIconsDescription.SetActive(true);
				leftIconsDescription.SetActive(false);
				leftIcon.localScale = new Vector2(minSize, minSize);

				rightImage.color = choicedColor;
				leftImage.color = unChoicedColor;
				break;
		}

		// �I��Ώۂ��؂�ւ�莞�̏���
		if (prevChoiceNum != nowChoiceNum)
		{
			if (nowChoiceNum == ChoiceType.Left)
			{
				SoundManager.Instance.PlaySE(SoundManager.SE_Type.AT);
			}
			else if (nowChoiceNum == ChoiceType.Right)
			{
				SoundManager.Instance.PlaySE(SoundManager.SE_Type.MT);
			}
			SoundManager.Instance.PlaySE(SoundManager.SE_Type.CursorSE);

			prevChoiceNum = nowChoiceNum;
			lastSelectTime = Time.time;
		}
		

		if(lastSelectTime - Time.time > pushGasTime && nowChoiceNum != 0)
		{
			SoundManager.Instance.PlaySE(SoundManager.SE_Type.PushGasPedal);
			lastSelectTime = Time.time;
		}
	}

	//�I�����ʂ�Ԃ�
	//0...���I��
	//-1...1�ڂ��I�����ꂽ
	//1...2�ڂ��I�����ꂽ
	public int GetChoice()
	{
		return (int)nowChoiceNum;
	}

	public void SetRight()
	{
		nowChoiceNum = ChoiceType.Right;
	}
	public void SetLeft()
	{
		nowChoiceNum = ChoiceType.Left;
	}
}


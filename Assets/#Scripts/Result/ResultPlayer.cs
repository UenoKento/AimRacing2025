/*
 * �t�@�C�����FResultPlayer.cs
 * �T�@�@�@�v�F���U���g�V�[���Ń��v���C���Đ�
 * ��@���@�ҁF20cu0213 ���ё�P
 * ��@���@���F2022/05/26 �쐬
 */

/*
 * �X�V�����F
 * 2022/05/26 [���ё�P]�@2020�f�[�^����ڐA
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class ResultPlayer : MonoBehaviour
{
    [Header("�Ԃ̃f�[�^")]
    [SerializeField] private GameObject ghost;
    [Header("�^�C���̃f�[�^")]
    [SerializeField] private Transform[] wheels = new Transform[4];
}

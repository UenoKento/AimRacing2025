/*
 * ファイル名：ResultPlayer.cs
 * 概　　　要：リザルトシーンでリプレイを再生
 * 作　成　者：20cu0213 小林大輝
 * 作　成　日：2022/05/26 作成
 */

/*
 * 更新履歴：
 * 2022/05/26 [小林大輝]　2020データから移植
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class ResultPlayer : MonoBehaviour
{
    [Header("車のデータ")]
    [SerializeField] private GameObject ghost;
    [Header("タイヤのデータ")]
    [SerializeField] private Transform[] wheels = new Transform[4];
}

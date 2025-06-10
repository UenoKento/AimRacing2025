//===================================================
// ファイル名	：SaveSystem.cs
// 概要			：Jsonでセーブデータを保存、読み込む、削除用のスクリプト
// 作成者		：熊彦哲
// 作成日		：
//===================================================
// 更新履歴     ：
//
//
//===================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class SaveSystem
{
    #region JSON
    /*
     * 型と名前：void SaveByJson
     * 仕様：セーブデータを書き込む
     * 引数：string saveFileName, object data
     * 出力：なし
     */
    public static void SaveByJson(string saveFileName, object data)
    {
        var json = JsonUtility.ToJson(data);
        var path = Application.dataPath + Path.Combine(@"\dir1", @"\SaveData", saveFileName);

        try
        {
            File.WriteAllText(path, json);

#if UNITY_EDITOR
            Debug.Log($"Susscessfully saved data to {path}.");
#endif
        }
        catch (System.Exception exception)
        {
#if UNITY_EDITOR
            Debug.LogError($"Failed to save data to {path}. \n{exception}");
#endif
        }
    }

    /*
     * 型と名前：T LoadFromJson<T>
     * 仕様：セーブデータを読み込む
     * 引数：string saveFileName
     * 出力：data/default
     */
    public static T LoadFromJson<T>(string saveFileName)
    {
        var path = Application.dataPath + Path.Combine(@"\dir1", @"\SaveData", saveFileName);
        try
        {
            var json = File.ReadAllText(path);
            var data = JsonUtility.FromJson<T>(json);
            return data;

        }
        catch (System.Exception exception)
        {
#if UNITY_EDITOR
            Debug.LogError($"Failed to load data to {path}. \n{exception}");
#endif
            return default;
        }
    }
    #endregion

    /*
     * 型と名前：void DeleteSaveFile
     * 仕様：セーブデータを削除する
     * 引数：string saveFileName
     * 出力：なし
     */
    #region Deleting
    public static void DeleteSaveFile(string saveFileName)
    {
        var path = Application.dataPath + Path.Combine(@"\dir1", @"\SaveData", saveFileName);
        try
        {
            File.Delete(path);
        }
        catch (System.Exception exception)
        {
#if UNITY_EDITOR
            Debug.LogError($"Failed to delete {path}. \n{exception}");
#endif
        }
    }
    #endregion

    /*
     * 型と名前：bool SaveFileExists
     * 仕様：セーブデータがあるかどうかの確認
     * 引数：string saveFileName
     * 出力：true/false
     */
    public static bool SaveFileExists(string saveFileName)
    {
        var path = Application.dataPath + Path.Combine(@"\dir1", @"\SaveData", saveFileName);

        return File.Exists(path);
    }
}

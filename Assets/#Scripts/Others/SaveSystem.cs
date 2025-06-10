//===================================================
// �t�@�C����	�FSaveSystem.cs
// �T�v			�FJson�ŃZ�[�u�f�[�^��ۑ��A�ǂݍ��ށA�폜�p�̃X�N���v�g
// �쐬��		�F�F�F�N
// �쐬��		�F
//===================================================
// �X�V����     �F
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
     * �^�Ɩ��O�Fvoid SaveByJson
     * �d�l�F�Z�[�u�f�[�^����������
     * �����Fstring saveFileName, object data
     * �o�́F�Ȃ�
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
     * �^�Ɩ��O�FT LoadFromJson<T>
     * �d�l�F�Z�[�u�f�[�^��ǂݍ���
     * �����Fstring saveFileName
     * �o�́Fdata/default
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
     * �^�Ɩ��O�Fvoid DeleteSaveFile
     * �d�l�F�Z�[�u�f�[�^���폜����
     * �����Fstring saveFileName
     * �o�́F�Ȃ�
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
     * �^�Ɩ��O�Fbool SaveFileExists
     * �d�l�F�Z�[�u�f�[�^�����邩�ǂ����̊m�F
     * �����Fstring saveFileName
     * �o�́Ftrue/false
     */
    public static bool SaveFileExists(string saveFileName)
    {
        var path = Application.dataPath + Path.Combine(@"\dir1", @"\SaveData", saveFileName);

        return File.Exists(path);
    }
}

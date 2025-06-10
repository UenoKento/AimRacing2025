// �Ԃ̈ʒu�Ǝ��Ԃ�ۑ����ċL�^����v���O����
// �쐬�ҁF23CU0325_�y�c�C��
// �쐬���F2025/06/01
// �ǉ����F2025/06/02 

using System.IO;
using UnityEngine;

public class LogToFile : TransformLogData
{
    private string filePath;        // �����o����̃t�@�C��

    [SerializeField]
    private const int MaxLogSize = 100000; // ���O�̍ő�T�C�Y

    void Start()
    {
        // �����o������w��
        filePath = Path.Combine(Application.persistentDataPath, "debug_log.txt");
        Debug.Log($"Log file path: {Application.persistentDataPath}/debug_log.txt");

        // �t�@�C���̏�����
        using (StreamWriter writer = new StreamWriter(filePath, false))
        {
            writer.WriteLine("=== Debug Log Start ===");
        }
    }

    void FixedUpdate()
    {

    }

    void SaveLogText()
    {
        using (StreamWriter writer = new StreamWriter(filePath, true))
        {
            for (int i = 0; i < _index; i++)
            {
                // �e�����t�@�C���ɏ����o��
                writer.WriteLine(savePosAndTime._transform);
                writer.WriteLine(savePosAndTime._rotation);
                writer.WriteLine(savePosAndTime._timer);
            }
        }
    }

    private void OnApplicationQuit()
    {
        // �A�v���P�[�V�����I�����Ƀ��O��ۑ�
        //SaveLogText();
        Debug.Log("Debug log saved to " + filePath);
    }
}
// ���͂��ꂽ�Ԃ̈ʒu(Pos)�E��](Rot)�E����(Time)���L�^���ă��O�܂��̓e�L�X�g�ɏo�͂���v���O����
// �쐬�ҁF23CU0325_�y�c�C��
// �쐬���F2025/05/29
// �ǋL���F2025/06/01 �ʒu�E��]�E���Ԃ�z��ɕۑ�����悤�ɕύX
// �ǋL���F2025/06/05 �X�N���v�g�Đ���
// �ǋL���F2025/06/06 Excel������PC�ł������ɓǂ߂�悤��CSV�ł͂Ȃ��A.txt�ŕۑ�����悤�ɕύX�B
//      �@.txt�ɐ������o�͂���Ȃ����̏C���B

using UnityEngine;
using System.IO;

public class SavePosRotTime : MonoBehaviour
{
    // �ʒu�E��]�E���Ԃ��L�^����\���̔z��
    public struct PosRotTime_Save
    {
        public Vector3 Save_pos;    // �ʒu
        public Vector3 Save_rot;   // ��]
        public float Save_time;    // ����

        public PosRotTime_Save(Vector3 pos, Vector3 rot, float time)
        {
            Save_pos = pos;
            Save_rot = rot;
            Save_time = time;
        }
    }

    // �\���̔z��錾(1�ňʒu�E��]�E���Ԃ��L�^����)
    public PosRotTime_Save[] _posRotTime;
    // �ۑ�����ő�l
    private const int MaxIndex = 100000;
    // �J��Ԃ��̒��ŏ��ԂɃJ�E���g���ĕۑ����邽�߂̕ϐ�(�ۑ�����z��̏ꏊ��1�����₷)
    private int CurrentIndex = 0;
    // �{�^���L�^�p�t���O
    private bool isRecord = false;

    // ���t���[���ۑ�����Ɨe�ʂ��f�J���Ȃ�̂Ő��t���[���Ɉ�x�����ۑ�����悤�ɂ���ϐ�
    private int frameLimiter = 0;
    // ���t���[���Ɉ�x�����ۑ����邩�̕ϐ�(20�t���[����1��ۑ�����)
    private const int SaveFrameInterval = 20;

    // �ۑ�����t�@�C���̃p�X
    private string filepath = Application.dataPath + "PosRotTimeLog.csv";

    // �t�@�C���p�X���擾����֐�
    public string GetFilePath() { return filepath; }
    // �ő�l���擾����֐�
    public int GetMaxIndex() { return MaxIndex; }
    public int GetCurrentIndex() { return CurrentIndex; }

    void Start()
    {
        // �\���̔z�񏉊���
        _posRotTime = new PosRotTime_Save[MaxIndex];     
    }

    void Update()
    {
        // ������Ă��Ȃ��ꍇreturn����
        if (!Input.GetKeyDown(KeyCode.Space)) { return; }
        // �t���O�؂�ւ�(�������тɐ؂�ւ���)
        isRecord = !isRecord;

        // �L�^�𒆒f�E�I�������Ƃ���CSV�t�@�C���ɕۑ�����
        if (!isRecord)
        {
            string filepath = Application.dataPath + "/PosRotTimeLog.txt";
            SaveToFile(filepath);
        }
    }

    void FixedUpdate()
    {
        // �L�[�������ꂽ��L�^���s
        if(isRecord) { Recording(); }
    }

    // �Ԃ̈ʒu�E��]�E���Ԃ��L�^���ă��O�ɕ\������v���O����
    void Recording()
    {
        // �z��̍ő�l�𒴂�����L�^���I������
        if (CurrentIndex >= MaxIndex)
        {
            Debug.Log("�L�^�I��: �z��̍ő�l�ɒB���܂����B");
            isRecord = false;
            return;
        }
        // 0����J�n���Ȃ��悤�ɂ���
        ++frameLimiter;

        // ���t���[���Ɉ�x�����z��ɋL�^����
        if (frameLimiter % SaveFrameInterval == 0)
        {
            _posRotTime[CurrentIndex] = new PosRotTime_Save(transform.localPosition, transform.localEulerAngles, Time.time);
            Debug.Log("pos" + _posRotTime[CurrentIndex].Save_pos);
            Debug.Log("rot" + _posRotTime[CurrentIndex].Save_rot);
            Debug.Log("time" + _posRotTime[CurrentIndex].Save_time);
            ++CurrentIndex;
        }
    }

    protected void SaveToFile(string filename)
    {
        using(StreamWriter writer = new StreamWriter(filename))
        {
            for(int i = 0; i < CurrentIndex; i++)
            {
                // �z��̈ʒu�E��]�E���Ԃ�.txt�`���ŕۑ�
                writer.WriteLine(_posRotTime[i].Save_pos.ToString());
                writer.WriteLine(_posRotTime[i].Save_rot.ToString());
                writer.WriteLine(_posRotTime[i].Save_time.ToString());
                writer.WriteLine(); // ��s�ŋ�؂�
            }
        }
        Debug.Log("�ۑ�����!" + filename);
    }

    // txt�`���̕�������\���̂ɕϊ�����ÓI���\�b�h
    public static PosRotTime_Save FromCSV(string line)
    {
        var values = line.Split(',');
        if (values.Length < 7)
        {
            Debug.LogWarning("txt�̗񐔂��s�����Ă��܂�");
            return new PosRotTime_Save();
        }

        return new PosRotTime_Save(
            new Vector3(
                float.Parse(values[0]),
                float.Parse(values[1]),
                float.Parse(values[2])
            ),
            new Vector3(
                float.Parse(values[3]),
                float.Parse(values[4]),
                float.Parse(values[5])
            ),
            float.Parse(values[6])
        );
    }
}

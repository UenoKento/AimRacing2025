// �Ԃ̈ʒu�Ǝ��Ԃ�ۑ����āA�L�^�����Ƃ���ɍĐ�����v���O����
// �쐬�ҁF23CU0325_�y�c�C��
// �쐬���F2025/05/29
// �ǋL���F2025/06/01 �ʒu�E��]�E���Ԃ�z��ɕۑ�����悤�ɕύX

using UnityEngine;

public class TransformLogData : MonoBehaviour
{
    [SerializeField]
    private GameObject _car2025;
    public int _index = 0;		  // �z��̃C���f�b�N�X

	private bool isRecording = false;   // ���v���C�̋L�^�����ǂ���

	// �ۑ������\���̔z��
	protected struct SavePosAndTime
	{
		public Vector3[] _transform; // �ʒu
		public Vector3[] _rotation;  // ��]
		public float[] _timer;       // ����
	}
	// �\���̐錾
	protected SavePosAndTime savePosAndTime;

    // ���t���[���Ɉ��L�^����悤�ɂ���
    private int frameCounter = 1;

	void Start()
	{
		savePosAndTime._transform = new Vector3[100000];
		savePosAndTime._rotation = new Vector3[100000];
		savePosAndTime._timer = new float[100000];
	}

	void FixedUpdate()
    {
		// �X�y�[�X�L�[�ŋL�^�J�n����
		if (Input.GetKeyDown(KeyCode.Space)) { isRecording = !isRecording; }

		// �L�^���ă��O�ɕ\��
        if(isRecording)
		{
			FrameCounter();
		}

	}

	// �t���[���̈ʒu�E��]�E���Ԃ�z��ɕۑ����ă��O�ɕ\������֐�
    void FrameCounter()
    {
		// ���t���[���̓f�[�^�������Ȃ�̂ŋL�^�񐔂����炷
		if (frameCounter % 20 == 0)
		{
			savePosAndTime._transform[_index] = transform.position; // �ʒu
			Debug.Log("Transform: " + savePosAndTime._transform[_index]);

			savePosAndTime._rotation[_index] = transform.rotation.eulerAngles; // ��]
			Debug.Log("Rotation: " + savePosAndTime._rotation[_index]);

			savePosAndTime._timer[_index] += Time.time;�@// ����
			Debug.Log("Time:" + savePosAndTime._timer[_index]);
		}
		frameCounter++;
		_index++;
	}
}
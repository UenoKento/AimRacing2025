using UnityEngine;

/// <summary>
/// 概要　：スタート/ゴールを通過したときにアクションを起こすプログラム
/// 担当者：22CU0235 諸星大和
/// 変更日：2024/05/17 作成
/// 　　　：2024/09/04 名称変更
/// </summary>
public class Line_StartFinish : MonoBehaviour
{
    [System.Serializable]
    enum LineMode
    {
        START,
        FINISH
    }
    [SerializeField] LineMode _lineMode;

    private BoxCollider _boxCollider;

    [SerializeField]
    private TimeKeeper _timeKeeper;

    //[SerializeField]
    //private GameObject _goalText;

    [SerializeField]
    private bool _isChecked = false;

    [SerializeField]
    private KeyCode _debugKeyCode = KeyCode.None;

    #region
    public bool IsChecked => _isChecked;
	#endregion

	private void Start()
    {
        _boxCollider = GetComponent<BoxCollider>();

        _isChecked = false;

		//if (_goalText != null)
		//{
		//	_goalText.SetActive(false);
		//}
	}

    private void Update()
    {
        if (Input.GetKeyDown(_debugKeyCode))
        {
            if (_isChecked == false)
            {
                switch (_lineMode)
                {
                    case LineMode.START:
                        _timeKeeper?.ControlActiveFlag(true);

                        break;

                    case LineMode.FINISH:
                        _timeKeeper?.ControlActiveFlag(false);
                        GameManager.Instance.ResultTime = _timeKeeper.RetrieveSavedTotalTime();
                        break;
                }

                _isChecked = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isChecked == false)
        {
            switch (_lineMode)
            {
                case LineMode.START:
                    _timeKeeper?.ControlActiveFlag(true);

                    break;

                case LineMode.FINISH:
                    _timeKeeper?.ControlActiveFlag(false);
					GameManager.Instance.ResultTime = _timeKeeper.RetrieveSavedTotalTime();
					//_goalText?.SetActive(true);
					break;
            }

            _isChecked = true;
        }
    }

   
}

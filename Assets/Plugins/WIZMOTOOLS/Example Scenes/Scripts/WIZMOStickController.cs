using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WIZMOStickController : MonoBehaviour
{
    //Status
    public enum WIZMOControllerStatus
    {
        Natural = 0,
        Up = 1,
        Left = 2,
        Down = 3,
        Right = 4
    }

    //WIZMO System
    public WIZMOController WIZMOSystemObject = null;

    [SerializeField]
    private WIZMOControllerStatus WIZMOCtrlStatus;

    // Start is called before the first frame update
    void Start()
    {
        if (WIZMOSystemObject == null)
            WIZMOSystemObject = this.gameObject.GetComponent<WIZMOController>();
        if (WIZMOSystemObject == null)
            Debug.LogError("WIZMO System object not specified.");

        WIZMOCtrlStatus = WIZMOControllerStatus.Natural;
    }

    // Update is called once per frame
    void Update()
    {
        if(WIZMOSystemObject == null)
            return;

        WIZMOCtrlStatus = (WIZMOControllerStatus)WIZMOSystemObject.GetStatusEXT4();
    }

    public WIZMOControllerStatus GetSIMVRCtrlStatus()
    {
        return WIZMOCtrlStatus;
    }
}

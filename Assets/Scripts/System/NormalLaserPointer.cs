using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 通常のレーザーポインター
/// </summary>
public class NormalLaserPointer : MonoBehaviour
{
    #region variable

    private Transform _rightHandAnchor = null;
    private Transform _leftHandAnchor = null;

    #endregion

    #region method

    /// <summary>
    /// レーザを表示するコントローラを設定
    /// </summary>
    private void SetActiveController(OVRInput.Controller controller)
    {/*
        Transform transform;
        if (controller == OVRInput.Controller.LTrackedRemote)
        {
            transform = _leftHandAnchor;
        }
        else
        {
            transform = _rightHandAnchor;
        }*/
        //GameObject.Find("EventSystem").GetComponent<OVRInputModule>().rayTransform = transform;
       // GameObject.Find("OculusLaserPointer").GetComponent<OVRGazePointer>().rayTransform = transform;
    }

    #endregion

    #region unity_script

    private void Start()
    {
       // _rightHandAnchor = GameObject.Find("RightHandAnchor").transform;
       // _leftHandAnchor = GameObject.Find("LeftHandAnchor").transform;

       // GameObject.Find("EventSystem").GetComponent<OVRInputModule>().rayTransform = _rightHandAnchor;
       // GameObject.Find("OculusLaserPointer").GetComponent<OVRGazePointer>().rayTransform = _rightHandAnchor;
    }
    /*
    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            SetActiveController(OVRInput.Controller.LTrackedRemote);
        }
        else if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
        {
            SetActiveController(OVRInput.Controller.RTrackedRemote);
        }
    }*/

    #endregion
}

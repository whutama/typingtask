using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UniRx;

public class EventRayTransformChanger : MonoBehaviour
{
    private Transform rightHandAnchor = null;
    private Transform leftHandAnchor = null;

    private bool isLaserMode = false;

    //public Text debugText;
    public StartUI startUi;

    void Start()
    {
        rightHandAnchor = GameObject.Find("RightHandAnchor").transform;
        leftHandAnchor = GameObject.Find("LeftHandAnchor").transform;
        //debugText.text = "itisdebu";

        startUi.OnLaser
            .Subscribe((_ =>
            {
                isLaserMode = true;
                GameObject.Find("EventSystem").GetComponent<OVRInputModule>().rayTransform = rightHandAnchor;
                GameObject.Find("OculusLaserPointer").GetComponent<OVRGazePointer>().rayTransform = rightHandAnchor;
            })).AddTo(this);
    }

    void Update()
    {
        if (!isLaserMode) return;

        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger)) {
            SetActiveController(OVRInput.Controller.LTrackedRemote);
            //debugText.text = "L";
        }
        else if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger)) {
            SetActiveController(OVRInput.Controller.RTrackedRemote);
            //debugText.text = "R";
        }

    }

    void SetActiveController(OVRInput.Controller c)
    {
        Transform t;
        if (c == OVRInput.Controller.LTrackedRemote) {
            t = leftHandAnchor;
        }
        else {
            t = rightHandAnchor;
        }
        GameObject.Find("EventSystem").GetComponent<OVRInputModule>().rayTransform = t;
        GameObject.Find("OculusLaserPointer").GetComponent<OVRGazePointer>().rayTransform = t;
        //debugText.text = "changed";
    }
}

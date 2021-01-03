using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using System;

public class SecondaryStartUi : MonoBehaviour
{
    #region variable

    private Subject<Unit> isAcctive = new Subject<Unit>();
    public IObservable<Unit> IsAcctive => isAcctive;

    [SerializeField]
    private Button StartButton;

    #endregion

    #region method

    public void Acctivate()
    {
        StartButton.gameObject.SetActive(false);
        isAcctive.OnNext(Unit.Default);
    }

    #endregion

    #region unity_script

    #endregion
}

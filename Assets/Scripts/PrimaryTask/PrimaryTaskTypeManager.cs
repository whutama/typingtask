using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using UnityEngine.UI;
using System.Text;

public class PrimaryTaskTypeManager : MonoBehaviour
{
    #region variable

    [SerializeField]
    private Text _typeText;
    [SerializeField]
    private Text _phraseText;
    [SerializeField]
    private Text _typedCountText;
    [SerializeField]
    private Text _firstTypedTimeText;
    [SerializeField]
    private Text _wpmText;
    [SerializeField]
    private Text _fixErrorRateText;
    [SerializeField]
    private StartUI _startUI;
    

    private string _typeString = "";
    private List<string> _phraseSet;
    private int _typeNumber = 0;
    private int _totalTypedAmount = 0; //総入力文字数
    private bool _isFirstInput = false;
    private double _totalLevenDistance = 0;
    private double _totalPhraseTextAmount = 0; //タスクの総文字数

    private Subject<Unit> _isEndTask = new Subject<Unit>();
    public IObservable<Unit> IsEndTask => _isEndTask;

    System.Diagnostics.Stopwatch _stopwatch = new System.Diagnostics.Stopwatch();

    #endregion

    #region method

    /// <summary>
    /// 入力を確定
    /// </summary>
    private void ConfirmType()
    {
        CheckError(_typeString);

        _typeString = "";
        _typeText.text = "";

        if(_typeNumber < _phraseSet.Count)
        {
            SetPhrase();
        }
        else
        {
            var wpm = ((_totalTypedAmount / 5) * 60) / ((double)_stopwatch.ElapsedMilliseconds / 1000);
            _wpmText.text = "WPM:" + string.Format("{0:f5", wpm);

            _fixErrorRateText.text = "fER:" + _totalLevenDistance + ". PA:" + _totalPhraseTextAmount;

            var sPresenter = GameObject.Find("ScoreCanvas").GetComponent<ScorePresenter>();
            sPresenter.AddScoreText(_firstTypedTimeText.text + "\r\n" + _wpmText.text + "\r\n" + _fixErrorRateText.text + "\r\n");

            _stopwatch.Stop();
            _typedCountText.text = "Finish";
            _isEndTask.OnNext(Unit.Default);
        }
    }

    /// <summary>
    /// 正誤判定
    /// </summary>
    private void CheckError(string str)
    {
        _totalPhraseTextAmount += _phraseSet[_typeNumber].Length;

        string st = "";
        StringBuilder stringbuilder = new StringBuilder(str.Length);

        foreach (var c in str)
        {
            if (!char.IsControl(c)) stringbuilder.Append(c);
        }
        st = stringbuilder.ToString();

        if (!str.Equals(_phraseSet[_typeNumber].ToLower()))
        {
            _totalLevenDistance += CalculateLevenshteinDistance(str, _phraseSet[_typeNumber]);
        }

        _fixErrorRateText.text = "Error:" + _totalLevenDistance;

        _typeNumber++;
    }

    /// <summary>
    /// フレーズを設定
    /// </summary>
    private void SetPhrase()
    {
        _phraseText.text = _phraseSet[_typeNumber].ToLower();
        _typedCountText.text = _typeNumber.ToString();
    }

    /// <summary>
    /// 一文字削除
    /// </summary>
    private void ExecuteBackSpace()
    {
        _typeString = _typeString.Substring(0, _typeString.Length - 1);
        _typeText.text = _typeString;
    }

    /// <summary>
    /// レーベンシュタイン距離で誤差率求める
    /// </summary>
    private static int CalculateLevenshteinDistance(string strX, string strY)
    {
        if (strX == null)
            throw new ArgumentNullException("strX");
        if (strY == null)
            throw new ArgumentNullException("strY");

        if (strX.Length == 0)
            return strY.Length;
        if (strY.Length == 0)
            return strX.Length;

        var d = new int[strX.Length + 1, strY.Length + 1];
        for (var i = 0; i <= strX.Length; i++)
        {
            d[i, 0] = i;
        }

        for (var j = 0; j <= strY.Length; j++)
        {
            d[0, j] = j;
        }

        for (var j = 1; j <= strY.Length; j++)
        {
            for (var i = 1; i <= strX.Length; i++)
            {
                if (strX[i - 1] == strY[j - 1])
                    d[i, j] = d[i - 1, j - 1];
                else
                    d[i, j] = Math.Min(Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1), d[i - 1, j - 1] + 1);
            }
        }

        return d[strX.Length, strY.Length];
    }

    #endregion

    #region unity_script

    void Start()
    {
        var phraseManager = GameObject.Find("Manager").GetComponent<PhraseManager>();
        _phraseSet = phraseManager.LoadPhrases();

        _startUI.IsAcctive
            .Subscribe(_ =>
            {
                _typedCountText.text = "Start";
                SetPhrase();
                _stopwatch.Start();
            })
            .AddTo(this);
    }

    void Update()
    {
        if (_typeNumber >= _phraseSet.Count) return;

        string input = Input.inputString;
        if (input.Equals("")) return;

        char c = input[0];
        if (c == "\b"[0])
        {
            ExecuteBackSpace();
        }else if(c == "\n"[0] || c == "\r"[0])
        {
            ConfirmType();
        }
        else
        {
            _typeString += c;
            _typeText.text = _typeString;
            _totalTypedAmount++;

            if (!_isFirstInput)
            {
                if (c.Equals(_phraseSet[0].ToCharArray()[0]))
                {
                    _isFirstInput = true;
                    _firstTypedTimeText.text = "FirstTime:" + ((double)_stopwatch.ElapsedMilliseconds / 1000);
                }
            }
        }
    }

    #endregion
}

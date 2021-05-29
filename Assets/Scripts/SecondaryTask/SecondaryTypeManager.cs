using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;
using System.Linq;
using UnityEngine.UI;
using System;

public class SecondaryTypeManager : MonoBehaviour
{
    #region variable

    [SerializeField]
    private SecondaryStartUi _startUI;
    [SerializeField]
    private int _targetKeyCount = 5;
    [SerializeField]
    private int _countDownTime = 5;
    [SerializeField]
    private Text _typeText;
    [SerializeField]
    private Text _phraseText;
    [SerializeField]
    private Text _countText;
    [SerializeField]
    private Text _erText;
    [SerializeField]
    private Text _typeTimeText;
    [SerializeField]
    private Text _countDownText;
    [SerializeField]
    private Button _menuButton;
    [SerializeField]
    private GameObject _phraseBox;

    private string _alphabets = "abcdefghijklmnopqrstuvwxyz";
    private bool _isActive = false;
    private bool _isCountDown = true;
    private char _currentTargetKey;
    private int _typeNumber = 0;
    private int[] _missTypes = new int[26];
    private List<double> _typeTimes = new List<double>();

    System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

    #endregion

    #region method

    private bool CheckCollectKey(char key)
    {
        return key == _currentTargetKey ? true : false;
    }

    private void SetTargetKey()
    {
        var num = UnityEngine.Random.RandomRange(0, _alphabets.Length);
        _currentTargetKey = _alphabets[num];
        _phraseText.text = _currentTargetKey.ToString();
    }

    public void LoadTitleScene()
    {
        SceneManager.LoadScene("Menu");
    }

    private void ExecuteCountDown()
    {
        _isCountDown = true;
        _phraseText.gameObject.SetActive(false);
        _typeText.text = "";
        Observable
            .Timer(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(1))
            .Select(x => (int)(_countDownTime - x))
            .TakeWhile(x => x >= 0)
            .Subscribe(time =>
            {
                _countDownText.text = time.ToString();
                if (time <= 0)
                {
                    _countDownText.text = "";
                    _phraseText.gameObject.SetActive(true);
                    SetTargetKey();
                    _isCountDown = false;
                    sw.Reset();
                    sw.Start();
                }
            }).AddTo(this);
    }

    #endregion

    #region unity_Script

    private void Start()
    {
        _menuButton.gameObject.SetActive(false);
        _startUI.IsAcctive
            .Subscribe(_ => {
                _isActive = true;
                ExecuteCountDown();
                if (_phraseBox != null)
                {
                    _phraseBox.SetActive(true);
                }
            })
            .AddTo(this);
    }

    private void Update()
    {
        if (!_isActive) return;
        if (_isCountDown) return;
        if (_typeNumber >= _targetKeyCount) return;

        if (Input.anyKeyDown)
        {
            var key = Input.inputString.FirstOrDefault();
            _typeText.text = key.ToString();
            if (CheckCollectKey(key))
            {
                _erText.text = _erText.text + ", " + _missTypes[_typeNumber];

                _typeNumber++;
                _countText.text = _typeNumber.ToString();
                sw.Stop();
                _typeTimeText.text = _typeTimeText.text + ", " + (String.Format("{0:f2}", ((double)sw.ElapsedMilliseconds / 1000)));
                if (_typeNumber >= _targetKeyCount)
                {
                    _countText.text = "Finish";
                    _typeText.text = "";
                    _menuButton.gameObject.SetActive(true);
                    var sPresenter = GameObject.Find("ScoreCanvas").GetComponent<ScorePresenter>();
                    sPresenter.AddScoreText(_typeTimeText.text + "\r\n" + _erText.text + "\r\n");
                }
                else
                {
                    SetTargetKey();
                    ExecuteCountDown();
                }

            }
            else
            {
                _missTypes[_typeNumber]++;
            }
        }
    }

    #endregion
}

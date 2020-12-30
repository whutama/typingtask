using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TypeManager : MonoBehaviour
{
    private string typeString = "";
    private List<string> phraseSet;
    private int typeNumber = 0;
    private int totalLength = 30;
    private double totalLevenDistance = 0;
    private int totalInputAmount = 0;
    private double totalPhraseTextAmount = 0;
    private bool isFirstInput = false;

    public Text typeText;
    public Text phraseText;
    public Text countText;
    public Text wpmText;
    public Text erText;
    public Text firstText;
    public StartUI startUi;
    public Button titleButton;

    public GameObject phraseBox;

    System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

    private void Start()
    {
        titleButton.gameObject.SetActive(false);

        //phraseSet = RandomSort(basePhrases);
        var pManager = GameObject.Find("Manager").GetComponent<PhraseManager>();
        phraseSet = pManager.LoadPhrases();

        startUi.IsAcctive
            .Subscribe(_ =>
            {
                countText.text = "start";
                if (phraseBox != null)
                {
                    phraseBox.SetActive(true);
                }
                SetPhrase();
                sw.Start();
            }).AddTo(this);

        //SetPhrase();
        //sw.Start();
    }

    void Update()
    {
        if (typeNumber >= phraseSet.Count) return;

        string input = Input.inputString;

        if (input.Equals("")) return; // if we are not typing stops this function

        char c = input[0];

        if (c == "\b"[0]) {
            BackSpace();
        }
        else if (c == "\n"[0] || c == "\r"[0]) {
            Confirm();
        }
        else {
            typeString += c;
            typeText.text = typeString;
            totalInputAmount++;

            if (!isFirstInput) {
                if (c.Equals(phraseSet[0].ToCharArray()[0])) {
                    isFirstInput = true;
                    firstText.text = "FirstTime:" + ((double)sw.ElapsedMilliseconds / 1000);
                }
            }
        }
    }

    //入力を確定
    public void Confirm()
    {
        Debug.Log(typeString);
        CheckError(typeString);

        typeString = "";
        typeText.text = "";

        if (typeNumber < phraseSet.Count) {
            SetPhrase();
        }
        else {
            sw.Stop();
            countText.text = "Finish";

            var wpm = ((totalInputAmount / 5) * 60) / ((double)sw.ElapsedMilliseconds / 1000);
            wpmText.text = "WPM" + string.Format("{0:f5}", wpm);
            //修正エラー率求める
            erText.text = "ER:" + totalLevenDistance + ", PA:" + totalPhraseTextAmount;

            titleButton.gameObject.SetActive(true);

            var sPresenter = GameObject.Find("ScoreCanvas").GetComponent<ScorePresenter>();
            sPresenter.AddScoreText(firstText.text + "\r\n" + wpmText.text + "\r\n" + erText.text + "\r\n");
        }
    }

    //string型リストをランダムに並び替え
    public List<string> RandomSort(List<string> list)
    {
        return list.OrderBy(a => Guid.NewGuid()).ToList();
    }

    //正誤判定
    public void CheckError(string str)
    {
        //フレーズの文字数足してく
        totalPhraseTextAmount += phraseSet[typeNumber].Length;

        //--------
        string st = "";
        StringBuilder buf = new StringBuilder(str.Length);

        foreach (char c in str) {
            if (!char.IsControl(c)) buf.Append(c);
        }
        st = buf.ToString();

        Debug.Log(str.Equals(phraseSet[typeNumber]) + "-" + str + "-" + phraseSet[typeNumber] + "-");
        //-------

        if (str.Equals(phraseSet[typeNumber].ToLower())) {
            Debug.Log("Correct");
        }
        else {
            totalLevenDistance += CalculateLevenshteinDistance(str, phraseSet[typeNumber]);
            Debug.Log("Not Correct");
        }
        typeNumber++;

        //レーベンシュタイン距離のみ
        erText.text = "Error:" + totalLevenDistance;
    }

    //フレーズを変更する
    public void SetPhrase()
    {
        phraseText.text = phraseSet[typeNumber].ToLower();
        countText.text = typeNumber.ToString();
    }

    //一文字削除する
    public void BackSpace()
    {
        typeString = typeString.Substring(0, typeString.Length - 1);
        typeText.text = typeString;
    }


    //-----
    private static int Min(int x, int y, int z)
    {
        return Math.Min(Math.Min(x, y), z);
    }
    //レーベンシュタイン距離で誤差率求める
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

        for (var i = 0; i <= strX.Length; i++) {
            d[i, 0] = i;
        }

        for (var j = 0; j <= strY.Length; j++) {
            d[0, j] = j;
        }

        for (var j = 1; j <= strY.Length; j++) {
            for (var i = 1; i <= strX.Length; i++) {
                if (strX[i - 1] == strY[j - 1])
                    d[i, j] = d[i - 1, j - 1];
                else
                    d[i, j] = Min(d[i - 1, j] + 1,
                                  d[i, j - 1] + 1,
                                  d[i - 1, j - 1] + 1);
            }
        }

        return d[strX.Length, strY.Length];
    }
    //---

    public void OnKeyDown(GameObject kb)
    {
        if (typeNumber >= phraseSet.Count) return;

        char c = kb.name.ToCharArray()[0];

        if (kb.name == "BACKSPACE") {
            BackSpace();
        }
        else if (kb.name == "ENTER") {
            Confirm();
        }
        else {
            typeString += c;
            typeText.text = typeString;
            totalInputAmount++;

            if (!isFirstInput) {
                if (c.Equals(phraseSet[0].ToCharArray()[0])) {
                    isFirstInput = true;
                    firstText.text = "FirstTime:" + ((double)sw.ElapsedMilliseconds / 1000);
                }
            }
        }
    }

    public void LoadTitleScene()
    {
        //Scene loadScene = SceneManager.GetActiveScene();
        //SceneManager.LoadScene(loadScene.name);
        SceneManager.LoadScene("Menu");
    }
}

﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class PhraseManager : MonoBehaviour
{
    private string resultString = "";

    //string型リストをランダムに並び替え
    public List<string> RandomSort(string[] array)
    {
        return array.OrderBy(a => Guid.NewGuid()).ToList();
    }

    //TextAssetをString配列に変換
    public string[] ChangeStringArray(TextAsset textAsset)
    {
        return textAsset.text.Replace("\r\n", "\n").Split(new[] { '\n', '\r' });
    }

    public List<string> LoadPhrases()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "phrases2.txt");

        var androidPath = Application.persistentDataPath + "/phrases2.txt";

        //StartCoroutine(PathManage(path));

        string[] phrases = File.ReadAllLines(androidPath);

        //return RandomSort(resultString.Replace("\r\n", "\n").Split(new[] { '\n', '\r' }).ToArray()).Take(5).ToList();
        return RandomSort(phrases).Take(5).ToList();
    }

    /*
    public IEnumerator PathManage(string filePath)
    {
        if (filePath.Contains("://")) {
            UnityWebRequest www = UnityWebRequest.Get(filePath);
            yield return www.SendWebRequest();
            resultString = www.downloadHandler.text;
        }
        else
            resultString = System.IO.File.ReadAllText(filePath);
    }*/
}
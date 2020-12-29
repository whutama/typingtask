using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using DG.Tweening;

public class KeyObserver : MonoBehaviour
{
    [SerializeField]
    private List<Image> keyImages;

    [SerializeField]
    private float duration = 0f;

    private void Update()
    {
        if (Input.anyKeyDown) {
            var key = Input.inputString;
            foreach (KeyCode code in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(code))
                {
                    keyImages.Where(k => k.name.Equals(key.ToUpper())).FirstOrDefault().DOColor(Color.red, duration);
                    keyImages.Where(k => k.name.Equals(key.ToUpper())).FirstOrDefault().DOColor(Color.white, duration).SetDelay(duration);
                }
            }
        }

    }
}

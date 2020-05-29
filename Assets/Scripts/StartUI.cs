using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class StartUI : MonoBehaviour
{
    private Subject<Unit> isAcctive = new Subject<Unit>();
    public IObservable<Unit> IsAcctive => isAcctive;

    public int CountTime = 5;
    public Text CountDownText;
    public Button StartButton;

    public void ExecuteTask()
    {
        StartButton.gameObject.SetActive(false);

        Observable
            .Timer(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(1))
            .Select(x => (int)(CountTime - x))
            .TakeWhile(x => x >= 0)
            .Subscribe(time =>
            {
                CountDownText.text = time.ToString();
                if (time <= 0) Acctivate();
            }).AddTo(this);
    }

    public void Acctivate()
    {
        isAcctive.OnNext(Unit.Default);
        CountDownText.enabled = false;
    }
}

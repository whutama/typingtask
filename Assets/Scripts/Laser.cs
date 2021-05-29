using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class Laser : MonoBehaviour
{
    LineRenderer lineRenderer;
    [SerializeField] private GameObject startPointObj;
    [SerializeField] protected float defaultLength = 3.0f;

    public StartUI StartUi;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = this.gameObject.GetComponent<LineRenderer>();

        StartUi.OnLaser
            .Subscribe(_ =>
            {
                lineRenderer.enabled = true;
            }).AddTo(this);
    }

    // Update is called once per frame
    void Update()
    {
        lineRenderer.SetPosition(0, startPointObj.transform.position);
        lineRenderer.SetPosition(1, GetEnd());
    }

    protected virtual Vector3 GetEnd()
    {
        return CalculateEnd(defaultLength);
    }

    protected Vector3 CalculateEnd(float length)
    {
        return transform.position + (transform.forward * length);
    }
}

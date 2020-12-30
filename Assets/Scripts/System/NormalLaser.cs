using UnityEngine;

/// <summary>
/// レーザ処理
/// </summary>
public class NormalLaser : MonoBehaviour
{
    #region variable

    private LineRenderer _lineRenderer;
    [SerializeField]
    private GameObject _startPointObj;
    [SerializeField]
    protected float _defaultLength = 3.0f;

    #endregion

    #region method

    /// <summary>
    /// 先端を指定
    /// </summary>
    protected virtual Vector3 GetEnd()
    {
        return CalculateEnd(_defaultLength);
    }

    /// <summary>
    /// レーザの長さを計算
    /// </summary>
    protected Vector3 CalculateEnd(float length)
    {
        return transform.position + (transform.forward * length);
    }

    #endregion

    #region unty_script

    void Start()
    {
      //  _lineRenderer = this.gameObject.GetComponent<LineRenderer>();
      //  _lineRenderer.enabled = true;
    }

    void Update()
    {
     //   _lineRenderer.SetPosition(0, _startPointObj.transform.position);
     //   _lineRenderer.SetPosition(1, GetEnd());
    }

    #endregion
}

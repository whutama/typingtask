using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UniRx;

/// <summary>
/// 実験1用シーンローダ
/// </summary>
public class PrimaryTaskSceneLoader : MonoBehaviour
{
    #region variable

    [SerializeField]
    private Button _menuButton;
    [SerializeField]
    private PrimaryTaskTypeManager _primaryTaskTypeManager;

    #endregion

    #region method
    
    /// <summary>
    /// Menuシーンをロード
    /// </summary>
    private void LoadMenuScene()
    {
        SceneManager.LoadScene("Menu");
    }

    #endregion

    #region unity_script

    private void Start()
    {
        _menuButton.onClick.AddListener(LoadMenuScene);
        _menuButton.gameObject.SetActive(false);

        _primaryTaskTypeManager.IsEndTask
            .Subscribe(_ => _menuButton.gameObject.SetActive(true))
            .AddTo(this);
    }

    #endregion
}

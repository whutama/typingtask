using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Menuシーンのマネージャ
/// </summary>
public class MenuManager : MonoBehaviour
{
    #region variable

    [SerializeField]
    private Button _practiceButton;
    [SerializeField]
    private Button _primaryTaskButton;
    [SerializeField]
    private Button _secondaryTaskButton;

    /// <summary>
    /// タスクの種類
    /// </summary>
    private enum TaskType
    {
        Practice,
        PrimaryTask,
        SecondaryTask,
    }

    #endregion

    #region method

    /// <summary>
    /// タスクに応じてシーンロード
    /// </summary>
    private void LoadScene(TaskType type)
    {
        switch (type)
        {
            case TaskType.Practice:
                SceneManager.LoadScene("Test2");
                break;
            case TaskType.PrimaryTask:
                SceneManager.LoadScene("SampleScene");
                break;
            case TaskType.SecondaryTask:
                SceneManager.LoadScene("Test4");
                break;
            default:
                break;
        }
    }

    #endregion

    #region unity_script

    private void Start()
    {
        _practiceButton.onClick.AddListener(() => LoadScene(TaskType.Practice));
        _primaryTaskButton.onClick.AddListener(() => LoadScene(TaskType.PrimaryTask));
        _secondaryTaskButton.onClick.AddListener(() => LoadScene(TaskType.SecondaryTask));
    }

    #endregion

  }  

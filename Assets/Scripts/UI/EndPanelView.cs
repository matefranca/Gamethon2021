using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Clear.Managers;

public class EndPanelView : MonoBehaviour
{
    [Header("Buttons.")]
    [SerializeField]
    private Button menuButton;
    [SerializeField]
    private Button retryButton;
    [SerializeField]
    private Button closeButton;

    [Header("Texts.")]
    [SerializeField]
    private TMP_Text pointsText;
    [SerializeField]
    private TMP_Text levelText;

    private void Start()
    {
        menuButton.onClick.RemoveAllListeners();
        menuButton.onClick.AddListener(GoToMenu);

        retryButton.onClick.RemoveAllListeners();
        retryButton.onClick.AddListener(RestartGame);
    }

    public void Init(string points, string level)
    {
        pointsText.SetText(points);
        levelText.SetText(level);
    }

    private void GoToMenu()
    {
        GameManager.GetInstance().GoToMenu();
    }

    private void RestartGame()
    {
        GameManager.GetInstance().RestartGame();
    }
}

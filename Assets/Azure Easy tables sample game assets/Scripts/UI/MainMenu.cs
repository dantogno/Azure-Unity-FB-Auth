using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour 
{
    [SerializeField]
    private Button[] buttonsThatRequireLogin;

    private void Start()
    {
        foreach (var item in buttonsThatRequireLogin)
        {
            item.interactable = false;
        }
    }

    public void LogInButtonClicked()
    {
        FacebookLogin.Instance.LogInUser();
    }
    public void RaceButtonClicked()
    {
        SceneManager.LoadScene("RaceScene");
    }

    public void HeatmapButtonClicked()
    {
        SceneManager.LoadScene("HeatmapScene");
    }

    public void LeaderboardButtonClicked()
    {
        SceneManager.LoadScene("LeaderboardScene");
    }

    public void ExitButtonClicked()
    {
        Application.Quit();
    }

    #region LoggedIn event handler / subscribe / unsubscribe
    private void OnLoggedIn()
    {
        foreach (var item in buttonsThatRequireLogin)
        {
            item.interactable = true;
        }
    }
    private void OnEnable()
    {
        FacebookLogin.LoggedIn += OnLoggedIn;
    }
    private void OnDisable()
    {
        FacebookLogin.LoggedIn -= OnLoggedIn;
    }
    #endregion
}

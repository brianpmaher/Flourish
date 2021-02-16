using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject[] menuObjects;

    private bool _menuOpen;
    
    private void Start()
    {
        OpenMenu();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_menuOpen)
            {
                CloseMenu();
            }
            else
            {
                OpenMenu();
            }
        }
    }

    public void HandleMenuOpenClick()
    {
        OpenMenu();
    }

    public void HandleMenuCloseClick()
    {
        CloseMenu();
    }
    
    public void HandleExitClick()
    {
        Application.Quit();
    }

    public void HandleRestartClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    private void OpenMenu()
    {
        foreach (var menuObject in menuObjects)
        {
            menuObject.SetActive(true);
        }

        _menuOpen = true;
        PauseGame();
    }

    private void CloseMenu()
    {
        foreach (var menuObject in menuObjects)
        {
            menuObject.SetActive(false);
        }

        _menuOpen = false;
        ResumeGame();
    }

    private static void PauseGame()
    {
        Time.timeScale = 0;
    }

    private static void ResumeGame()
    {
        Time.timeScale = 1;
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class PostGameScreen : MonoBehaviour
    {
        public void Open()
        {
            PauseGame();

            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
        }
    
        public void HandleExitClick()
        {
            Application.Quit();
        }

        public void HandleRestartClick()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    
        private static void PauseGame()
        {
            Time.timeScale = 0;
        }
    }
}
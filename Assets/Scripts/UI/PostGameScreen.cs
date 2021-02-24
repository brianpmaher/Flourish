using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class PostGameScreen : MonoBehaviour
    {
        public bool isOpen;
        
        public void Open()
        {
            PauseGame();

            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }

            isOpen = true;
        }

        public void Close()
        {
            ResumeGame();

            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }

            isOpen = false;
        }

        public void HandleCloseClick()
        {
            ResumeGame();
            
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
                
                isOpen = false;
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

        private static void ResumeGame()
        {
            Time.timeScale = 1;
        }
    }
}
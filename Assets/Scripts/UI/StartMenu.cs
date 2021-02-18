using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class StartMenu : MonoBehaviour
    {
        /// <summary>
        /// Handles starting the game.
        /// </summary>
        public void HandleStartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        /// <summary>
        /// Handles exiting the game.
        /// </summary>
        public void HandleExitGame()
        {
            Application.Quit();
        }
    }
}
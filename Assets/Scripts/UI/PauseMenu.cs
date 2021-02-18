using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private GameObject[] menuObjects;
        [SerializeField] private GameObject startButton;
        [SerializeField] private GameObject exitButton;
        [SerializeField] private GameObject restartButton;

        private bool _firstTimeMenu = true;
        private bool _menuOpen;

        private List<GameObject> MenuObjects
        {
            get
            {
                var combinedMenuObjects = menuObjects.ToList();

                if (_firstTimeMenu)
                {
                    combinedMenuObjects.Add(startButton);
                }
                else
                {
                    combinedMenuObjects.Add(exitButton);
                    combinedMenuObjects.Add(restartButton);
                }

                return combinedMenuObjects;
            }
        }
    
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
            foreach (var menuObject in MenuObjects)
            {
                menuObject.SetActive(true);
            }

            _menuOpen = true;
            PauseGame();
        }

        private void CloseMenu()
        {
            foreach (var menuObject in MenuObjects)
            {
                menuObject.SetActive(false);
            }

            _menuOpen = false;
            _firstTimeMenu = false;
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
}
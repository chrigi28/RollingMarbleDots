using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Assets.Common.Scripts;
using Assets.Common.Scripts.Components;
using Assets.Scripts;
using Assets.Scripts.GameData;
using TMPro;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace Assets
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        private EGameState gameState = EGameState.Paused;

        public GameObject Canvas;
        private GameObject currentLevel;
        private CanvasScript canvasScript;
        private TimerScript timerScript;
        private PlayerData playerdata;

        private EntityManager manager;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);

                this.InitGameVariables();
                EventCenter.GameStateChangeEvent.AddListener(m => this.GameStateChanged(m));
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        private void InitGameVariables()
        {
            GameDataManager.LoadPlayerData();
            this.playerdata = PlayerData.Instance;
        }

        void Start()
        {
            this.canvasScript = this.Canvas.GetComponent<CanvasScript>();
            this.timerScript = this.Canvas.GetComponent<TimerScript>();
            manager = World.DefaultGameObjectInjectionWorld.EntityManager;
            this.DisableMenus();
        }

        public void RestartLevel()
        {
            ////SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            EventCenter.PlayerPositionChangedEvent.Invoke(new SetPlayerPositionMessage());
            this.StartCountDown();
        }

        public void GoToLevelSelection()
        {
            SceneManager.LoadScene(0);
        }

        public void SelectLevel(TextMeshProUGUI t)
        {
            //load LevelSelectionScene
            this.LoadLevel(int.Parse(t.text));
            this.DisableMenus();
            this.StartCountDown();
        }

        private void StartCountDown()
        {
            this.DisableMenus();
            gameState = EGameState.Countdown;
            Time.timeScale = 1;
            timerScript.Restart();
        }

        private void LoadLevel(int level)
        {
            PlayerData.Instance.CurrentLevel = level;
            var lg = GameObject.FindObjectOfType<LevelGenerator>();
            lg.SetUpLevel(level, 125, 750);
        }

        public void Quit()
        {
            Application.Quit();
        }

        public void PauseGame()
        {
            Time.timeScale = 0;
            gameState = EGameState.Paused;
            this.canvasScript.TogglePause();
        }

        public bool IsRunning()
        {
            return gameState == EGameState.Running;
        }

        public bool IsCountDown => gameState == EGameState.Countdown;

        public bool IsPaused()
        {
            return gameState == EGameState.Paused;
        }

        public void DisableMenus()
        {
            this.canvasScript.DisableMenus();
        }

        public void ContinueGame()
        {
            gameState = EGameState.Running;
            this.DisableMenus();
            Time.timeScale = 1;
        }

        public void ShowSettings()
        {
            Debug.Log("Settings Requested");
        }

        public void ShowHome()
        {
            SceneManager.LoadScene(0);
            Debug.Log("Settings Requested");
        }

        public void FinishLevel()
        {
            Debug.Log("GameFinished");
            Time.timeScale = 0;
            var time = this.timerScript.GetTime();
             var bestTime = this.playerdata.GetBestTimeOfCurrentLevel();
            if (time < bestTime)
            {
                bestTime = time;
            }

            this.canvasScript.ShowFinish(time, bestTime);
            gameState = EGameState.Paused;

            this.playerdata.SetLevelData(this.playerdata.CurrentLevel, time, 1);

            // show time
            // show Score (nr * )
            // show next start level
            // show level select 
            // show go home
        }

        public void SetGameState(EGameState state)
        {
            this.gameState = state;
        }

        public void GameOver()
        {
            Time.timeScale = 0;
            gameState = EGameState.Paused;
            this.canvasScript.SetGameOver(true);
        }

        public void GameStateChanged(IGameStateChangeMessage m)
        {
            if (m.Finish)
            {
                this.FinishLevel();
            }

            if (m.GameOver)
            {
                this.GameOver();
            }

            if (m.Pause)
            {
                this.PauseGame();
            }
        }
    }
}

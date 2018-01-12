using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager instance = null;

    #region GameState
    public enum GameState
    {
        MENU, LOADING, PAUSE, PLAY, END
    }
    private static GameState gameState;
    #endregion

    #region Event
    #region Game 
    public delegate void onGameInit();
    public event onGameInit OnGameInit;
    public delegate void onGameStart();
    public event onGameStart OnGameStart;
    public delegate void onGamePause();
    public event onGamePause OnGamePause;
    public delegate void onGameResume();
    public event onGameResume OnGameResume;
    public delegate void onGameEnd();
    public event onGameEnd OnGameEnd;
    #endregion

    #region Scene Event
    public delegate void onSceneLoad(string name);
    public event onSceneLoad OnSceneLoad;
    public delegate void onSceneReload(string name);
    public event onSceneReload OnSceneReload;
    public delegate void onSceneLoaded(Scene scene, LoadSceneMode sceneMode);
    public event onSceneLoaded OnSceneLoaded;
    #endregion
    #endregion

    #region Singleton Initialization 
    void Awake()
    {

        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(transform.gameObject);
    }
    #endregion

    #region Dispatchers

    #region Game State
    void d_InitGame()
    {
        if (OnGameInit != null)
            OnGameInit();
    }

    void d_StartGame()
    {
        if (OnGameStart != null)
            OnGameStart();
    }

    void d_PauseGame()
    {
        if (OnGamePause != null)
            OnGamePause();
    }

    void d_ResumeGame()
    {
        if (OnGameResume != null)
            OnGameResume();
    }

    void d_EndGame()
    {
        if (OnGameEnd != null)
            OnGameEnd();
    }
    #endregion

    #region Scene Loading Dispatchers
    void d_LoadedScene(Scene scene, LoadSceneMode sceneMode)
    {
        SceneManager.sceneLoaded -= d_LoadedScene;
        if (OnSceneLoaded != null)
            OnSceneLoaded(scene, sceneMode);
    }

    void d_LoadScene(string LevelName)
    {
        if (OnSceneLoad != null)
            OnSceneLoad(LevelName);
    }
    #endregion
    #endregion

    #region Methods
    #region Game State
    public void InitGame()
    {
        d_InitGame();
    }

    public void StartGame()
    {
        d_StartGame();
    }

    public void PauseGame()
    {
        d_PauseGame();
    }

    public void ResumeGame()
    {
        d_ResumeGame();
    }

    public void EndGame()
    {
        d_EndGame();
    }
    #endregion

    #region Scene
    public void ReloadScene()
    {
        string SceneName = SceneManager.GetActiveScene().name;
        if (OnSceneReload != null)
            OnSceneReload(SceneName);
        LoadScene(SceneName);
    }

    public void LoadScene(string LevelName)
    {
        SceneManager.LoadScene(LevelName);
        SceneManager.sceneLoaded += d_LoadedScene;
        d_LoadScene(LevelName);
    }
    #endregion

    #region GameState
    public static GameState getState()
    {
        return gameState;
    }

    public static void setState(GameState state)
    {
        gameState = state;
    }
    #endregion
    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{

    public static TimeManager instance = null;

    #region Variables
    public float time
    {
        get
        {
            return _time;
        }
    }
    private float _time;
    private float _modifier;
    #endregion

    #region Singleton Initialization 
    void Awake()
    {

        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);

        GameManager.instance.OnPauseGame += OnPause;
        GameManager.instance.OnResumeGame += OnResume;

        DontDestroyOnLoad(transform.gameObject);
    }
    #endregion

    #region Native Methods
    void Start()
    {
        _time = 0;
        _modifier = 1;
    }

    void Update()
    {
        _time = Time.deltaTime * _modifier;
    }
    #endregion

    private void OnDestroy()
    {
        GameManager.instance.OnPauseGame -= OnPause;
        GameManager.instance.OnResumeGame -= OnResume;
    }
    #region Methods

    private void OnPause()
    {
        Time.timeScale = 0;
        _modifier = 0;
    }

    private void OnResume()
    {
        Time.timeScale = 1;
        _modifier = 1;
    }
    #endregion
}

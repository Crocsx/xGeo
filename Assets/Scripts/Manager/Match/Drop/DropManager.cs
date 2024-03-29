﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropManager : MonoBehaviour {

    public GameObject DroppablePrefab;

    public GameObject spawned;

    [Header("Drops Timers")]
    public float MAX_DROP_COOLDOWN_DELTA = 3;
    public float DROP_RATE_COOLDOWN = 5f;
    float dropCurrentCooldown = 0;

    bool _active = false;
    public bool isActive { get { return _active; } }

    private void Awake()
    {
        GameManager.instance.OnStartGame += GameStart;
        GameManager.instance.OnFinishGame += GameFinish;
        GameManager.instance.OnEndGame += GameEnd;
    }

    void Start ()
    {
        _active = false;
    }

    void GameStart()
    {
        Activate();
    }

    void GameFinish()
    {
        Deactivate();
    }

    void GameEnd()
    {
        Deactivate();
        GameManager.instance.OnStartGame -= GameStart;
        GameManager.instance.OnFinishGame -= GameFinish;
        GameManager.instance.OnEndGame -= GameEnd;
    }

    float GetRandomTime()
    {
        return Random.Range(DROP_RATE_COOLDOWN - MAX_DROP_COOLDOWN_DELTA, DROP_RATE_COOLDOWN + MAX_DROP_COOLDOWN_DELTA);
    }

    void Update () {
        if (_active && spawned == null) CheckDrop();
    }

    void CheckDrop()
    {
        dropCurrentCooldown -= TimeManager.instance.time;
        if (dropCurrentCooldown < 0)
        {
            Dropitem();
        }
    }

    void Dropitem()
    {
        spawned = Instantiate(DroppablePrefab, transform.position, Quaternion.identity);
        spawned.GetComponent<Drop>().OnItemDroped += FreeDrop;
    }

    private void FreeDrop()
    {
        if(spawned != null)
        {
            spawned.GetComponent<Drop>().OnItemDroped -= FreeDrop;
            spawned = null;
        }

        dropCurrentCooldown = GetRandomTime();
    }
    public void Activate()
    {
        _active = true;
        FreeDrop();
    }

    public void Deactivate()
    {
        if (spawned != null)
            spawned.GetComponent<Drop>().OnItemDroped -= FreeDrop;

        _active = false;
    }


}

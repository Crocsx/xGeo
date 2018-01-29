using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropManager : MonoBehaviour {

    public GameObject DroppablePrefab;
    public GameObject DroppableArea;

    [Header("Drops Timers")]
    public float MAX_DROPS_ON_MAP = 6;
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
        GameManager.instance.OnStartGame -= GameStart;
        GameManager.instance.OnFinishGame -= GameFinish;
        GameManager.instance.OnEndGame -= GameEnd;
    }

    void Update () {
        if (_active) CheckDrop();
    }

    void CheckDrop()
    {
        dropCurrentCooldown += TimeManager.instance.time;
        if (dropCurrentCooldown > DROP_RATE_COOLDOWN)
        {
            Dropitem();
            dropCurrentCooldown = 0;
        }
    }

    void Dropitem()
    {
        SpriteRenderer spriteRenderer = DroppableArea.GetComponent<SpriteRenderer>();
        Vector3 SpawnLocation = GetPointOnDroppableArea(spriteRenderer);
        Instantiate(DroppablePrefab, SpawnLocation, Quaternion.identity);
    }

    Vector3[] GetSpriteSize(SpriteRenderer sp)
    {
        Vector3 pos = transform.position;
        Vector3[] array = new Vector3[2];
        array[0] = pos + sp.bounds.min;
        array[1] = pos + sp.bounds.max;
        return array;
    }

    Vector3 GetPointOnDroppableArea(SpriteRenderer sp)
    {
        Vector3[] array = GetSpriteSize(sp);
        return new Vector3(Random.Range(array[0].x, array[1].x), Random.Range(array[0].y, array[1].y), 1);
    }

    public void Activate()
    {
        _active = true;
    }

    public void Deactivate()
    {
        _active = false;
    }
}

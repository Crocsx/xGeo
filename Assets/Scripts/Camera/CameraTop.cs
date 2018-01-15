using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTop : MonoBehaviour {


    #region Parameters
    public List<Player> players;

    [Header("Speed")]
    public float cameraSpeed = 3;

    [Header("Altitude")]
    public float MINIMUM_ALTITUDE = 5;
    public float MAXIMUM_ALTITUDE = 100;

    [Header("Smooths")]
    public float MAX_DISTANCE_BORDER_OFFSET = 5;

    private Camera _camera;
    #endregion

    #region Native Methods
    void Start()
    {
        _camera = transform.gameObject.GetComponent<Camera>();
        MatchManager.instance.OnPlayerSpawned += AddPlayer;
        MatchManager.instance.OnPlayerKilled += RemovePlayer;
    }

    void Setup()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (players.Count <= 0)
            return;

        Vector2 center = GetPlayersCentroid();
        float maxDistance = GetPlayersMaxDistance(center) + MAX_DISTANCE_BORDER_OFFSET;

        // Debug.Log(maxDistance);
        Vector2 newPosition = Vector2.Lerp(transform.position, center, TimeManager.instance.time * cameraSpeed);
        transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);

        _camera.orthographicSize = Mathf.Clamp(maxDistance, MINIMUM_ALTITUDE, MAXIMUM_ALTITUDE);
        //Mathf.Lerp(MINIMUM_ALTITUDE, MAXIMUM_ALTITUDE, maxDistance / (MINIMUM_ALTITUDE - MAXIMUM_ALTITUDE));
    }
    #endregion

    #region Methods
    Vector2 GetPlayersCentroid()
    {
        Vector2 centroid = Vector2.zero;
        for (int i = 0; i < players.Count; i ++)
        {
            centroid.x += players[i].transform.position.x;
            centroid.y += players[i].transform.position.y;
        }
        return centroid / players.Count;
    }

    float GetPlayersMaxDistance(Vector2 center)
    {
        float maxDistance = 0;
        for (int i = 0; i < players.Count; i++)
        {
            float distance = Mathf.Abs(Vector2.Distance(center, players[i].transform.position));
            if (maxDistance < distance)
                maxDistance = distance;
        }
        return maxDistance;
    }

    void AddPlayer(Player player)
    {
        if (players.Contains(player))
            return;

        players.Add(player);
    }

    void RemovePlayer(Player player)
    {
        if (players.Contains(player))
            players.Remove(player);
    }
    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBarycenter : MonoBehaviour
{
    [Header("----------- CAMERA MOVEMENT -----------")]
    public bool hasMovement = true;
    public float smoothTime = 0.5f;
    [Header("----------- CAMERA ZOOM    -----------")]
    public bool hasZoom = true;
    public float minZoom = -100.0f;
    public float maxZoom = -70.0f;
    public float zoomLimiter = 50.0f;
    [Header("----------- CAMERA BOUNDS ------------")]
    public bool hasCameraBounds = true;
    public Collider2D boundsIntern;
    public Collider2D boundsExtern;
    private MultiplayerManager multiplayerManager;
    [Header("----------- DEBUG --------------------")]
    [SerializeField] private Vector3 velocity = new Vector3(0.0f, 0.0f, 0.0f);
    [SerializeField] private Vector3 centerPoint = new Vector3(0.0f, 0.0f, 0.0f);
    [SerializeField] float bounds_X = 0.0f;
    [SerializeField] float bounds_Y = 0.0f;
    private void Awake()
    {
        velocity = new Vector3(0.0f, 0.0f, 0.0f);
        bounds_X = boundsIntern.bounds.extents.x / 2;
        bounds_Y = boundsIntern.bounds.extents.y / 2;
    }
    private void Start()
    {
        multiplayerManager = MultiplayerManager.instance;
    }

    private void LateUpdate()
    {
        if (multiplayerManager.players.Count <= 0) { return; }

        if (hasMovement) { FollowPlayers(); }
        if (hasZoom) { Zoom(); }
    }

    private void FollowPlayers()
    {
        centerPoint = GetCentroid();
        if (hasCameraBounds)
        {
            centerPoint.x = Mathf.Clamp(centerPoint.x, -bounds_X, bounds_X);
            centerPoint.y = Mathf.Clamp(centerPoint.y, -bounds_Y, bounds_Y);
        }
        Vector3 newPos = new Vector3(
            centerPoint.x,
            centerPoint.y,
            transform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, newPos, ref velocity, smoothTime);
    }

    private void Zoom()
    {
        float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance() / zoomLimiter);
        transform.position = new Vector3(
            transform.position.x, 
            transform.position.y, 
            Mathf.Lerp(transform.position.z, newZoom, Time.deltaTime));
    }

    private Vector3 GetCentroid()
    {
        Vector3 center = new Vector3(0, 0, 0);
        for (int i = 0; i < multiplayerManager.players.Count; i++)
        {
            center += multiplayerManager.players[i].transform.position;
        }
        center /= multiplayerManager.players.Count;

        //Debug
        float val = 0;
        List<GameObject> players = multiplayerManager.players;
        foreach(GameObject player in players)
        {
            Debug.DrawLine(player.transform.position, center, Color.red + new Color(-val, val, 0));
            val += 0.25f;
        }

        return center;
    }

    private float GetGreatestDistance()
    {
        List<GameObject> players = multiplayerManager.players;
        var bounds = new Bounds(players[0].transform.position, Vector3.zero);
        for (int i = 0; i < players.Count; i++)
        {
            bounds.Encapsulate(players[i].transform.position);
        }

        return bounds.size.x;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class CameraState : MonoBehaviour
{
    [Header("------- Data -------")]
    public CameraType type = CameraType.BARYCENTER;
    public Collider bounds;

    [Header("------ Animation ------")]
    public float animTime = 0.5f;

    [Header("------- Move -------")]
    public bool canMove = true;
    public bool freezeX = false;
    public bool freezeY = false;

    [Header("------- Zoom -------")]
    public bool canZoom = true;
    public float maxOut_Z = -110.0f;
    public float maxIn_Z = -70.0f;
    public float zoomOutMargin = -5.0f;
    public float zoomInMargin = 10.0f;
    public float zoomOutValue = -20.0f;
    public float zoomInValue = -10.0f;
    public float zoomOut_Margin = 5.0f;
    public float zoomIn_Margin = 10.0f;


    [Header("----- End Animation -----")]
    public bool hasZoomOutAtEnd = true;
    public bool hasCenterCamera = true;
    //public bool hasFreeRoaming = false;
    //public int randomRadius = 5;
    //public float roamingTime = 3.0f;

    [Header("----- Prototype -----")]
    public List<CameraData> datas = new List<CameraData>();

    [Header("----- Debug -----")]
    [SerializeField] protected Camera cam = null;
    [SerializeField] protected MapManager mapManager = null;
    [SerializeField] protected MultiplayerManager multiplayerManager = null;
    [SerializeField] protected List<Player> playerList = new List<Player>();
    [SerializeField] protected Vector3 positionToGoTo = new Vector3(0.0f, 0.0f, 0.0f);
    [SerializeField] protected Vector3 velocity = new Vector3(0.0f, 0.0f, 0.0f);
    [SerializeField] protected Vector3 barycenter = new Vector3(0.0f, 0.0f, 0.0f);
    [SerializeField] protected float bounds_X = 0.0f;
    [SerializeField] protected float bounds_Y = 0.0f;
    [SerializeField] protected float min_bounds_X = 0.0f;
    [SerializeField] protected float max_bounds_X = 0.0f;
    [SerializeField] protected float min_bounds_Y = 0.0f;
    [SerializeField] protected float max_bounds_Y = 0.0f;
    [SerializeField] protected float frustumHeight = 0.0f;
    [SerializeField] protected float frustumWidth = 0.0f;
    [SerializeField] protected float min_viewport_X = 0.0f;
    [SerializeField] protected float max_viewport_X = 0.0f;
    [SerializeField] protected float min_viewport_Y = 0.0f;
    [SerializeField] protected float max_viewport_Y = 0.0f;
    [SerializeField] protected float playersBounds_X = 0.0f;
    [SerializeField] protected float playersBounds_Y = 0.0f;
    [SerializeField] protected float min_playersBounds_X = 0.0f;
    [SerializeField] protected float max_playersBounds_X = 0.0f;

    protected virtual void Start()
    {
        cam = Camera.main;
        mapManager = MapManager.instance;
        multiplayerManager = MultiplayerManager.instance;
        if(multiplayerManager != null)
        {
            playerList = multiplayerManager.players;
        }

        UpdateFrustrum();

        if (bounds == null)
        {
            Debug.Log("Please add camera bounds to CameraBarycenter");
        }
        else
        {
            bounds_X = bounds.bounds.size.x;
            bounds_Y = bounds.bounds.size.y;
            UpdateBounds();
        }
    }

    protected virtual void Update()
    {
        // Protections
        if (SceneManager.GetActiveScene().name == "0_MenuSelection" || SceneManager.GetActiveScene().buildIndex == 0) { return; }
        if (multiplayerManager == null) { return; }
        if (mapManager == null) { return; }
        if (cam == null) { return; }
        if (mapManager.isBusy) { return; }
        if (playerList.Count == 0) { return; }

        UpdateFrustrum();
        UpdateBounds();
        UpdatePlayersBounds();
    }

    protected virtual void LateUpdate()
    {
        // Protections
        if (SceneManager.GetActiveScene().name == "0_MenuSelection" || SceneManager.GetActiveScene().buildIndex == 0) { return; }
        if (multiplayerManager == null) { return; }
        if (mapManager == null) { return; }
        if (cam == null) { return; }
        if (mapManager.isBusy) { return; }
        if (playerList.Count == 0) { return; }

        if (canZoom) { UpdateZoom(); }
        UpdateCamera();
    }

    protected virtual void UpdateCamera()
    {
        transform.parent.position = Vector3.SmoothDamp(transform.parent.position, positionToGoTo, ref velocity, animTime);
    }

    private void UpdateZoom()
    {
        // Zoom Out
        float min_zoomOut_X = min_playersBounds_X - zoomOut_Margin;
        float max_zoomOut_X = max_playersBounds_X + zoomOut_Margin;
        bool canZoomOut = (min_viewport_X >= min_zoomOut_X && max_viewport_X <= max_zoomOut_X)
                          && transform.parent.position.z >= maxOut_Z;

        Debug.Log("Zoom out : " + canZoomOut);
        // if viewport is too small compare to greatest distance between players
        if (canZoomOut)
        {
            positionToGoTo.z = Mathf.Clamp(transform.position.z + zoomOutValue, maxOut_Z, maxIn_Z);
            return;
        }

        // Zoom In
        float min_zoomIn_X = min_playersBounds_X - zoomIn_Margin;
        float max_zoomIn_X = max_playersBounds_X + zoomIn_Margin;

        // if viewport is too big compare to greatest distance between players
        bool canZoomIn = (min_viewport_X <= min_zoomIn_X || max_viewport_X >= max_zoomIn_X)
                         && transform.parent.position.z <= maxIn_Z;

        if (canZoomIn)
        {
            positionToGoTo.z = Mathf.Clamp(transform.position.z + zoomInValue, maxOut_Z, maxIn_Z);
        }
        Debug.Log("Zoom In : " + canZoomIn);

    }


    protected void UpdateFrustrum()
    {
        float distance = -transform.parent.position.z;
        frustumHeight = 2.0f * distance * Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad);
        frustumWidth = frustumHeight * cam.aspect;

        float offsetX = frustumWidth / 2.0f;
        float offsetY = frustumHeight / 2.0f;
        min_viewport_X = transform.parent.position.x - offsetX;
        max_viewport_X = transform.parent.position.x + offsetX;
        min_viewport_Y = transform.parent.position.y - offsetY;
        max_viewport_Y = transform.parent.position.y + offsetY;
    }
    
    protected void UpdateBounds()
    {
        float offsetX = (bounds_X - frustumWidth) / 2.0f;
        float offsetY = (bounds_Y - frustumHeight) / 2.0f;
        min_bounds_X = bounds.transform.position.x - offsetX;
        max_bounds_X = bounds.transform.position.x + offsetX;
        min_bounds_Y = bounds.transform.position.y - offsetY;
        max_bounds_Y = bounds.transform.position.y + offsetY;
    }

    protected void UpdatePlayersBounds()
    {
        Bounds playersBounds = new Bounds(playerList[0].transform.position, Vector3.zero);

        foreach (Player player in playerList)
        {
            playersBounds.Encapsulate(player.transform.position);
        }

        playersBounds_X = playersBounds.size.x;

        float offsetX = playersBounds_X;
        min_playersBounds_X = barycenter.x - offsetX;
        max_playersBounds_X = barycenter.x + offsetX;
    }

    #region Public Method
    public CameraType GetCameraType()
    {
        return type;
    }
    #endregion

    #region Debug
    protected virtual void OnDrawGizmosSelected()
    {
        // Draw Camera Viewport
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.parent.position, new Vector3(frustumWidth, frustumHeight, 1));

        // Draw Camera Zoom Boxes
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.parent.position, new Vector3(frustumWidth + zoomOutMargin, frustumHeight + zoomOutMargin, 1));
        Gizmos.DrawWireCube(transform.parent.position, new Vector3(frustumWidth + zoomInMargin, frustumHeight + zoomInMargin, 1));

        // Draw Camera Bounds
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(bounds.transform.position, new Vector3(bounds_X, bounds_Y, 1));

        // Draw Camera Movement Clamp
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(bounds.transform.position, new Vector3(bounds_X - frustumWidth, bounds_Y - frustumHeight, 1));

        // Draw Players Bounds
        Gizmos.color = Color.grey;
        Gizmos.DrawWireCube(barycenter, new Vector3(playersBounds_X, playersBounds_Y, 1));

        // Draw Zoom Out Margin from PlayerBounds
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(barycenter, new Vector3(playersBounds_X + zoomOut_Margin, playersBounds_Y + zoomOut_Margin, 1));

        // Draw Zoom In Margin from PlayerBounds
        Gizmos.DrawWireCube(barycenter, new Vector3(playersBounds_X + zoomIn_Margin, playersBounds_Y + zoomIn_Margin, 1));
    }
    #endregion
}

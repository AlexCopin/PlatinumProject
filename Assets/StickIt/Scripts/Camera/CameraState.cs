using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class CameraState : MonoBehaviour
{
    [Header("------- Data -------")]
    public CameraType type = CameraType.BARYCENTER;
    public bool autoBounds = true;
    public Collider camBounds;

    [Header("------- Move -------")]
    public bool canMove = true;
    public float moveTime = 0.5f;
    public bool freezeX = false;
    public bool freezeY = false;

    [Header("------- Zoom -------")]
    public bool canZoom = true;
    public float zoomTime = 1.0f;
    public bool autoMaxOut_Z = false;
    public float maxOut_Z = -110.0f;
    public float maxIn_Z = -70.0f;
    public float zoomOutValue = -20.0f;
    public float zoomInValue = 10.0f;
    public float zoomOutMargin = 2.0f;
    public float zoomInMargin = 5.0f;
    public bool canClampZoom = false;

    [Header("----- Prototype -----")]
    public List<CameraData> datas = new List<CameraData>();

    [Header("----- Debug -----")]
    [SerializeField] protected Camera cam = null;
    [SerializeField] protected MapManager mapManager = null;
    [SerializeField] protected MultiplayerManager multiplayerManager = null;
    [SerializeField] protected List<Player> playerList = new List<Player>();
    [SerializeField] protected Bounds bounds;
    [SerializeField] protected Vector3 boundsPos = new Vector3(0.0f, 0.0f, 0.0f);
    [SerializeField] protected Vector3 positionToGoTo = new Vector3(0.0f, 0.0f, 0.0f);
    [SerializeField] protected Vector3 moveVelocity = new Vector3(0.0f, 0.0f, 0.0f);
    [SerializeField] protected Vector3 zoomVelocity = new Vector3(0.0f, 0.0f, 0.0f);
    [SerializeField] protected Vector3 barycenter = new Vector3(0.0f, 0.0f, 0.0f);
    [SerializeField] protected float bounds_width = 0.0f;
    [SerializeField] protected float bounds_height = 0.0f;
    [SerializeField] protected float min_bounds_X = 0.0f;
    [SerializeField] protected float max_bounds_X = 0.0f;
    [SerializeField] protected float min_bounds_Y = 0.0f;
    [SerializeField] protected float max_bounds_Y = 0.0f;
    [SerializeField] protected float min_moveBounds_X = 0.0f;
    [SerializeField] protected float max_moveBounds_X = 0.0f;
    [SerializeField] protected float min_moveBounds_Y = 0.0f;
    [SerializeField] protected float max_moveBounds_Y = 0.0f;
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

    protected virtual void Awake()
    {
        if (bounds == null)
        {
            bounds = CameraBounds.Instance.GetComponent<Collider>().bounds;
            //Debug.Log("Please add camera bounds to Camera State");
        }

        GameEvents.OnSceneUnloaded.AddListener(ResetCamera);
    }
    protected virtual void Start()
    {
        cam = Camera.main;
        mapManager = MapManager.instance;
        multiplayerManager = MultiplayerManager.instance;
        if(multiplayerManager != null) { playerList = multiplayerManager.players; }

        UpdateFrustum();

        if (bounds == null)
        {
            bounds = CameraBounds.Instance.GetComponent<Collider>().bounds;
            //Debug.Log("Please add camera bounds to Camera State");
        }
        else
        {
            UpdateBounds();
            UpdateMoveBounds();
        }

        SearchMaxOut_Z();
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

        UpdateFrustum();
        UpdateMoveBounds();
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

        UpdateCamera();
    }

    protected virtual void UpdateCamera()
    {
        // Move Camera
        Vector3 newPos = new Vector3(
            positionToGoTo.x,
            positionToGoTo.y,
            transform.parent.position.z);
        transform.parent.position = Vector3.SmoothDamp(transform.parent.position, newPos, ref moveVelocity, moveTime);

        // if zooming out and is touching border > return and wait to move first
        bool isTouchingBorder = 
            min_viewport_X <= min_bounds_X || max_viewport_X >= max_bounds_X;
        if (canClampZoom && positionToGoTo.z < transform.parent.position.z && isTouchingBorder) { return; }
        
        // Zoom Camera
        Vector3 newZoom = new Vector3(
            transform.parent.position.x,
            transform.parent.position.y,
            positionToGoTo.z);
        transform.parent.position = Vector3.SmoothDamp(transform.parent.position, newZoom, ref zoomVelocity, zoomTime);
    }

    protected void UpdateFrustum()
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

    protected virtual void UpdateBounds()
    {

        boundsPos = Camera.main.ScreenToWorldPoint(bounds.center);
        //Debug.Log(boundsPos);
        boundsPos.z = 0;

        //bounds_width = newBounds.size.x;
        //bounds_height = newBounds.size.y;

        float offsetX = bounds_width / 2.0f;
        float offsetY = bounds_height / 2.0f;
        min_bounds_X = boundsPos.x - offsetX;
        max_bounds_X = boundsPos.x + offsetX;
        min_bounds_Y = boundsPos.y - offsetY;
        max_bounds_Y = boundsPos.y + offsetY;
    }

    protected void UpdateMoveBounds()
    {
        float offsetX = (bounds_width - frustumWidth) / 2.0f;
        float offsetY = (bounds_height - frustumHeight) / 2.0f;
        min_moveBounds_X = boundsPos.x - offsetX;
        max_moveBounds_X = boundsPos.x + offsetX;
        min_moveBounds_Y = boundsPos.y - offsetY;
        max_moveBounds_Y = boundsPos.y + offsetY;
    }

    protected void UpdatePlayersBounds()
    {
        Bounds playersBounds = new Bounds(playerList[0].transform.position, Vector3.zero);

        foreach (Player player in playerList)
        {
            playersBounds.Encapsulate(player.transform.position);
        }

        playersBounds_X = playersBounds.size.x;
        playersBounds_Y = playersBounds.size.y;

        float offsetX = playersBounds_X;
        min_playersBounds_X = barycenter.x - offsetX;
        max_playersBounds_X = barycenter.x + offsetX;
    }

    protected virtual void UpdateZoom() {
        // Zoom Out
        float min_zoomOut_X = min_playersBounds_X - zoomOutMargin;
        float max_zoomOut_X = max_playersBounds_X + zoomOutMargin;
        bool canZoomOut = (min_viewport_X >= min_zoomOut_X && max_viewport_X <= max_zoomOut_X)
                          && -Mathf.Floor(-transform.parent.position.z) > maxOut_Z;
        // If Viewport is too small compare to bounds players
        if (canZoomOut)
        {
            positionToGoTo.z = Mathf.Clamp(transform.position.z + zoomOutValue, maxOut_Z, maxIn_Z);
            return;
        }

        // Zoom In
        float min_zoomIn_X = min_playersBounds_X - zoomInMargin;
        float max_zoomIn_X = max_playersBounds_X + zoomInMargin;

        // If Viewport is too big compare to bounds players
        bool canZoomIn = (min_viewport_X <= min_zoomIn_X || max_viewport_X >= max_zoomIn_X)
                         && -Mathf.Floor(-transform.parent.position.z) < maxIn_Z;
        if (canZoomIn)
        {
            positionToGoTo.z = Mathf.Clamp(transform.position.z + zoomInValue, maxOut_Z, maxIn_Z);
        }
    }

    private void SearchMaxOut_Z()
    {
        // Search Max Zoom Out
        if (autoMaxOut_Z)
        {
            float maxFrustumHeight = 0.0f;
            if (bounds_width < bounds_height)
            {
                float maxFrustumWidth = bounds_width;
                maxFrustumHeight = maxFrustumWidth / cam.aspect;
            }
            else
            {
                maxFrustumHeight = bounds_height;
            }

            float distance = -maxFrustumHeight * 0.5f / Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad);
            maxOut_Z = distance;
        }
    }

    #region Public Method
    public CameraType GetCameraType()
    {
        return type;
    }

    public void ResetCamera()
    {
        UpdateFrustum();
        UpdateBounds();
        UpdateMoveBounds();
        UpdatePlayersBounds();
        SearchMaxOut_Z();
    }
    #endregion

    #region Debug
    protected virtual void OnDrawGizmosSelected()
    {
        if(bounds == null) { return; }
        // Draw Camera Viewport
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.parent.position, new Vector3(frustumWidth, frustumHeight, 1));

        // Draw Camera Bounds
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(boundsPos, new Vector3(bounds_width, bounds_height, 1));

        // Draw Camera Movement Clamp
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(boundsPos, new Vector3(bounds_width - frustumWidth, bounds_height - frustumHeight, 1));

        // Draw Players Bounds
        Gizmos.color = Color.grey;
        Gizmos.DrawWireCube(barycenter, new Vector3(playersBounds_X, playersBounds_Y, 1));

        // Draw Zoom Out Margin from PlayerBoundsj
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(barycenter, new Vector3(playersBounds_X + zoomOutMargin, playersBounds_Y + zoomOutMargin, 1));

        // Draw Zoom In Margin from PlayerBounds
        Gizmos.DrawWireCube(barycenter, new Vector3(playersBounds_X + zoomInMargin, playersBounds_Y + zoomInMargin, 1));
    }
    #endregion

/* PROTOTYPE
    public void LoadCameraData()
    {
        CameraData current = datas[dataIndex];

        moveTime = current.moveTime;
        distanceBeforeBorder = current.distanceBeforeBorder;
        freezeX = current.freezeX;
        freezeY = current.freezeY;
        hasFollowLastPlayer = current.hasFollowOnlyPlayer;
        hasFreeRoaming = current.hasFreeRoaming;
        randomRadius = current.randomRadius;
        roamingTime = current.roamingTime;
        maxOut_Z = current.maxOut_Z;
        maxIn_Z = current.maxIn_Z;
        zoomOutMargin = current.zoomOutMargin;
        zoomInMargin = current.zoomInMargin;
        zoomOutValue = current.zoomOutSpeed;
        zoomInValue = current.zoomInSpeed;
        zoomTime = current.zoomTime;
        hasZoomOutAtEnd = current.hasZoomOutAtEnd;
        deathMargin = current.deathMargin;
        timeBeforeDeath = current.timeBeforeDeath;

        dataIndex++;
    }
*/
}

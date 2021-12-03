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
    public Vector3 offset = new Vector3(0.0f, 0.0f, 0.0f);

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
    [SerializeField] protected BlocksScript blocksScript = null;
    [SerializeField] protected MapManager mapManager = null;
    [SerializeField] protected MultiplayerManager multiplayerManager = null;
    [SerializeField] protected List<Player> playerList = new List<Player>();
    [SerializeField] protected Vector3 positionToGoTo = new Vector3(0.0f, 0.0f, 0.0f);
    [SerializeField] protected Vector3 moveVelocity = new Vector3(0.0f, 0.0f, 0.0f);
    [SerializeField] protected Vector3 zoomVelocity = new Vector3(0.0f, 0.0f, 0.0f);
    [SerializeField] protected Vector3 barycenter = new Vector3(0.0f, 0.0f, 0.0f);
    [SerializeField] protected Vector2 boundsPos = new Vector3(0.0f, 0.0f);
    [SerializeField] protected Vector2 bounds_dimension = new Vector2(0.0f, 0.0f);
    [SerializeField] protected Vector2 min_bounds = new Vector2(0.0f, 0.0f);
    [SerializeField] protected Vector2 max_bounds = new Vector2(0.0f, 0.0f);
    [SerializeField] protected Vector2 min_moveBounds = new Vector2(0.0f, 0.0f);
    [SerializeField] protected Vector2 max_moveBounds = new Vector2(0.0f, 0.0f);
    [SerializeField] protected Vector2 frustum_dimension = new Vector2(0.0f, 0.0f);
    [SerializeField] protected Vector2 min_viewport = new Vector2(0.0f, 0.0f);
    [SerializeField] protected Vector2 max_viewport = new Vector2(0.0f, 0.0f);
    [SerializeField] protected float playersBounds_X = 0.0f;
    [SerializeField] protected float playersBounds_Y = 0.0f;
    [SerializeField] protected Vector2 min_playerBounds = new Vector2(0.0f, 0.0f);
    [SerializeField] protected Vector2 max_playerBounds = new Vector2(0.0f, 0.0f);

    protected virtual void Awake()
    {
        GameEvents.OnSceneUnloaded.AddListener(ResetCamera);
        if (SceneManager.GetActiveScene().name == "1_MenuSelection" || SceneManager.GetActiveScene().buildIndex == 0) { return; }
        TakeNewBounds();
    }
    protected virtual void Start()
    {
        cam = Camera.main;
        mapManager = MapManager.instance;
        multiplayerManager = MultiplayerManager.instance;
        if(multiplayerManager != null) { playerList = multiplayerManager.players; }

        UpdateFrustum();

        blocksScript = BlocksScript.Instance;
   
        if(blocksScript != null)
        {
            Debug.Log("Block script ");
            TakeNewBounds();
            UpdateMoveBounds();
        }

        SearchMaxOut_Z();
    }

    protected virtual void Update()
    {
        // Protections
        if (SceneManager.GetActiveScene().name == "1_MenuSelection" || SceneManager.GetActiveScene().buildIndex == 0) { return; }
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
        if (SceneManager.GetActiveScene().name == "1_MenuSelection" || SceneManager.GetActiveScene().buildIndex == 0) { return; }
        if (multiplayerManager == null) { return; }
        if (mapManager == null) { return; }
        if (cam == null) { return; }
        if (mapManager.isBusy) { return; }
        if (playerList.Count == 0) { return; }

        UpdateCamera();
    }

    protected virtual void UpdateCamera()
    {
        /*
        // Move Camera
        Vector3 newPos = new Vector3(
            positionToGoTo.x,
            positionToGoTo.y,
            transform.parent.position.z);
        transform.parent.position = Vector3.SmoothDamp(transform.parent.position, newPos, ref moveVelocity, moveTime);
        */
        // if zooming out and is touching border > return and wait to move first
        bool isTouchingBorder = 
            min_viewport.x <= min_bounds.x || max_viewport.x >= max_bounds.x;
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
        frustum_dimension.y = 2.0f * distance * Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad);
        frustum_dimension.x = frustum_dimension.y * cam.aspect;

        float offsetX = frustum_dimension.x / 2.0f;
        float offsetY = frustum_dimension.y / 2.0f;
        min_viewport.x = transform.parent.position.x - offsetX;
        max_viewport.x = transform.parent.position.x + offsetX;
        min_viewport.y = transform.parent.position.y - offsetY;
        max_viewport.y = transform.parent.position.y + offsetY;
    }

    protected virtual void TakeNewBounds()
    {
        if (blocksScript == null) return;
        boundsPos = blocksScript.boundsPos;
        bounds_dimension = blocksScript.dimension;
        float offsetX = bounds_dimension.x/ 2.0f;
        float offsetY = bounds_dimension.y / 2.0f;
        min_bounds.x = boundsPos.x - offsetX;
        max_bounds.x = boundsPos.x + offsetX;
        min_bounds.y = boundsPos.y - offsetY;
        max_bounds.y = boundsPos.y + offsetY;
    }

    protected void UpdateMoveBounds()
    {
        float offsetX = (bounds_dimension.x - frustum_dimension.x) / 2.0f;
        float offsetY = (bounds_dimension.y - frustum_dimension.y) / 2.0f;
        max_moveBounds.x = boundsPos.x + offsetX;
        min_moveBounds.x = boundsPos.x - offsetX;
        min_moveBounds.y = boundsPos.y - offsetY;
        max_moveBounds.y = boundsPos.y + offsetY;
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
        min_playerBounds.x = barycenter.x - offsetX;
        max_playerBounds.x = barycenter.x + offsetX;
    }

    protected virtual void UpdateZoom() {
        // Zoom Out
        float min_zoomOut_X = min_playerBounds.x - zoomOutMargin;
        float max_zoomOut_X = max_playerBounds.x + zoomOutMargin;
        bool canZoomOut = (min_viewport.x >= min_zoomOut_X && max_viewport.x <= max_zoomOut_X)
                          && -Mathf.Floor(-transform.parent.position.z) > maxOut_Z;
        // If Viewport is too small compare to bounds players
        if (canZoomOut)
        {
            positionToGoTo.z = Mathf.Clamp(transform.position.z + zoomOutValue, maxOut_Z, maxIn_Z);
            return;
        }

        // Zoom In
        float min_zoomIn_X = min_playerBounds.x - zoomInMargin;
        float max_zoomIn_X = max_playerBounds.x + zoomInMargin;

        // If Viewport is too big compare to bounds players
        bool canZoomIn = (min_viewport.x <= min_zoomIn_X || max_viewport.x >= max_zoomIn_X)
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
            if (bounds_dimension.x < bounds_dimension.y)
            {
                float maxFrustumWidth = bounds_dimension.x;
                maxFrustumHeight = maxFrustumWidth / cam.aspect;
            }
            else
            {
                maxFrustumHeight = bounds_dimension.y;
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
        blocksScript = BlocksScript.Instance;
        UpdateFrustum();
        TakeNewBounds();
        UpdateMoveBounds();
        UpdatePlayersBounds();
        SearchMaxOut_Z();
    }
    #endregion

    #region Debug
    protected virtual void OnDrawGizmosSelected()
    {
        // Draw Camera Viewport
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.parent.position, new Vector3(frustum_dimension.x, frustum_dimension.y, 1));

        // Draw Camera Bounds
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(boundsPos, new Vector3(bounds_dimension.x, bounds_dimension.y, 1));

        // Draw Camera Movement Clamp
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(boundsPos, new Vector3(bounds_dimension.x - frustum_dimension.x, bounds_dimension.y - frustum_dimension.y, 1));

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

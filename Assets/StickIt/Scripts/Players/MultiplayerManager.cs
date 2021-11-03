using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
public class MultiplayerManager : MonoBehaviour
{

    public struct PlayerData
    {
        public string name;
        public int id;
        public int deviceID;
        public Material material;
        public int mass;

        public uint score;
        public uint nbrDeath;
        public uint nbrVictories;
        public PlayerData(string _name, int _id, int _deviceID, Material _material)
        {
            name = _name;
            id = _id;
            deviceID = _deviceID;
            material = _material;
            mass = 100;
            score = 0;
            nbrDeath = 0;
            nbrVictories = 0;
        }
    }
    

    public static MultiplayerManager instance;
    public List<Material> materialsTemp = new List<Material>();
    public int nbrOfPlayer;
    [SerializeField] private Transform _prefabPlayer;
    [SerializeField] AnimationCurve curve_ChangeMap_PosX;
    [SerializeField] AnimationCurve curve_ChangeMap_PosY;

    [Header("------------DEBUG------------")]
    public List<Player> players = new List<Player>();
    public List<Player> alivePlayers = new List<Player>();
    public List<Player> deadPlayers = new List<Player>();

    public List<Material> materials = new List<Material>();

    private List<PlayerData> datas = new List<PlayerData>();
    private Transform playersStartingPos;
  //  private int nbrDevicesLastFrame = 0;
    [HideInInspector] public float speedChangeMap = 1;
    private float t = 0f;
    private float y = 0f;
    private float[] initPosX;
    private float[] initPosY;
    private bool isChangingMap = false;


    int nbrDevicesLastFrame = 0;

    

#if UNITY_EDITOR
    [SerializeField] public bool isMenuSelection = false; // should be private
#endif

    private void Awake()
    {

      // Initialization();
        if (instance == null) instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(this);
#if UNITY_EDITOR
        // Is Menu Selection ?
        if(SceneManager.GetActiveScene().name == "0_MenuSelection")
        {
            isMenuSelection = true;
        }
        else
        {
            isMenuSelection = false;
        }
#endif
    }

    private void Start()
    {
        playersStartingPos = FindObjectOfType<PlayerStartingPos>().transform;
#if UNITY_EDITOR
        if(!isMenuSelection)
        InitializePlayersWithoutMenuSelector(nbrOfPlayer);
#endif
    }


    private void Update()
    {
        if (isChangingMap)
        {
            LerpDuringChangeMap();
        }
    }


    public void SaveDatas(PlayerData playerData)
    {
        datas.Add(playerData);
    }

    public void InitializePlayersWithoutMenuSelector(int numberOfPlayer)
    {
        for(int i = 0; i < numberOfPlayer; i++)
        {
            PlayerData newData = new PlayerData("Player" + i.ToString(), i, -1, materials[i]);
            datas.Add(newData);
        }

        for (int i = 0; i < datas.Count; i++)
        {
            PlayerInput newPlayer = null;
            Gamepad gamepad = null;
            foreach (Gamepad pad in Gamepad.all)
            {
                if (pad.deviceId == datas[i].deviceID)
                {
                    gamepad = pad;
                    break;
                }
            }
            newPlayer = PlayerInput.Instantiate(_prefabPlayer.gameObject, datas[i].id, "Gamepad", -1, gamepad);
            Player scriptPlayer = newPlayer.transform.GetComponent<Player>();
            scriptPlayer.myDatas = datas[i];
            scriptPlayer.transform.GetComponentInChildren<SkinnedMeshRenderer>().materials[0] = scriptPlayer.myDatas.material;
            players.Add(scriptPlayer);
            alivePlayers.Add(scriptPlayer);

            newPlayer.transform.position = playersStartingPos.GetChild(i).position;

        }

    }



    public void StartChangeMap()
    {
        // Disable the players
        foreach(Player player in players)
        {
            player.PrepareToChangeLevel();
        }

        // Prepare to lerp Players

        foreach(Transform child in MapManager.instance.nextMapRoot.transform)
        {
            if (child.GetComponent<PlayerStartingPos>())
            {
                playersStartingPos = child;
                break;
            }
        }
        initPosX = new float[players.Count];
        initPosY = new float[players.Count];
        for(int i = 0; i < players.Count; i++)
        {
            initPosX[i] = players[i].transform.position.x;
            initPosY[i] = players[i].transform.position.y;
        }
        t = 0f;
        isChangingMap = true;
    }

    private void LerpDuringChangeMap()
    {
        for(int i = 0; i < players.Count; i++)
        {
            t += Time.unscaledDeltaTime * speedChangeMap;
            y = t;
            y = curve_ChangeMap_PosX.Evaluate(y);
            float currentPosX = Mathf.Lerp(initPosX[i], playersStartingPos.GetChild(i).transform.position.x, y);
            y = t;
            y = curve_ChangeMap_PosY.Evaluate(y);
            float currentPosY = Mathf.Lerp(playersStartingPos.GetChild(i).transform.position.y, initPosY[i] , 1-y);
            players[i].transform.position = new Vector3(currentPosX, currentPosY);
            if(y >= 1)
            {
                EndChangeMap();
            }
        }
    }

    public void EndChangeMap()
    {
        isChangingMap = false;

        // Reset the lists and re-enable the players
        alivePlayers = players;
        deadPlayers.Clear();
        RespawnPlayers();
    }

    public void RespawnPlayers()
    {
        for(int i = 0; i < players.Count; i++)
        {
            players[i].Respawn();
        }
    }





}

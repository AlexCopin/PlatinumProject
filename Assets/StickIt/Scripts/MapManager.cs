using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapManager : Unique<MapManager>
{
    [Header("TEST______________________")]
    public bool startACertainMap = true;
    public string mapToStart = "7_Deathmatch";
    public bool playAllMapInOrder = false;
    public bool playAllMapInRandom = false;
    [Header("DATAS____________________")]
    public ModsData modsData;
    public float numberOfRounds = 2.0f;
    [Range(0, 1)]
    public float smoothTime;
    public float slowTime;
    public float smoothMOE;
    public int mapOffset;
    public float timeScale;
    public string endMapName = "100_EndScene";

    //[Header("PREFABS___________________")]
    //public VisualEffect onDeathVFX;                                             // Take Player death Animation to know how long to wait before transition

    [Header("DEBUG____________________")]
    public bool isBusy;
    [SerializeField] private GameObject curMapRoot;
    [SerializeField] private GameObject nextMapRoot;
    [SerializeField] private string prevModName;
    [SerializeField] private string prevMap;
    [SerializeField] private Mod prevMod;
    [SerializeField] private string curModName;
    [SerializeField] private string curMap = "";
    [SerializeField] private Mod curMod;
    [SerializeField] private Coroutine _coroutine;
    [SerializeField] private CameraStateDriven camManager;
    [SerializeField] private bool levelEnded = false;
    [SerializeField] private FadeShader shaderScript;
    [SerializeField] private int modCount = 0;
    [SerializeField] private int mapCount = 0;
    [SerializeField] private uint roundCount = 0;
    [SerializeField] private List<Mod> modsRemaining = new List<Mod>();
    [SerializeField] private ModsData cloneModsData;

    #region Property
    public string CurModName { get => curModName; }
    #endregion
    //void OnGUI()
    //{
    //    if (GUI.Button(new Rect(0, 0, 200, 100), "NextMap")) NextMap(nextMapManual, true);
    //}
    protected override void Awake()
    {
        base.Awake();
        camManager = Camera.main.GetComponent<CameraStateDriven>();
        shaderScript = GetComponent<FadeShader>();
        modCount = 0;
        mapCount = 0;
        roundCount = 0;

        cloneModsData = Object.Instantiate(modsData);
        foreach (Mod aMod in cloneModsData.mods)
        {
            modsRemaining.Add(aMod);
        }
    }
    private void Start()
    {
        if(AudioManager.instance != null) {
            AudioManager.instance.PlayAmbiantSounds(gameObject);
        }

        if (playAllMapInOrder || playAllMapInRandom)
        {
            numberOfRounds = 0;
            foreach (Mod aMod in modsData.mods)
            {
                numberOfRounds += aMod.maps.Count;
            }
        }
    }
    public bool EndLevel()
    {
        if (!levelEnded && MultiplayerManager.instance.alivePlayers.Count <= 1)
        {
            shaderScript.AllObjectsDisappear();
            NextMap();
            levelEnded = true;
            return true;
        }
        return false;
    }
    public bool NextMap(string nextMap = "", bool fromMenu = false)
    {
        //if (fromMenu)
        //{
        //    nextMap = SelectNextMap();
        //    SceneManager.LoadScene(nextMap);
        //    camManager.SwitchStates(Utils.GetCameraType(curMod));
        //    return true;
        //}
        foreach (Player player in MultiplayerManager.instance.players) player.PrepareToChangeLevel();
        if (_coroutine == null) _coroutine = StartCoroutine(BeginTransition(nextMap, fromMenu));
        else return false;
        return true;
    }
    private void UpdateData(Mod mod, string map)
    {
        prevMod = curMod;
        prevModName = curModName;
        prevMap = curMap;
        curMod = mod;
        curModName = mod.name;
        curMap = map;

        roundCount++;
    }

    private string SelectNextMap()
    {
        // End Game
        if (roundCount == numberOfRounds)
        {
            curModName = "End";
            curMap = endMapName;
            return endMapName;
        }

        // Next Map
        Mod mod = new Mod();
        string map = SceneManager.GetActiveScene().name;

        // Protection | if No Mod > return current scene
        if (modsData.mods.Count == 0) return map;

        // Play All Map In Order
        if (playAllMapInOrder)
        {
            // Get Next Mod
            if (mapCount >= modsData.mods[modCount].maps.Count)
            {
                modCount++;
                mapCount = 0;
            }
            mod = modsData.mods[modCount];

            // Get Next Map
            map = mod.maps[mapCount];
            mapCount++;
            if (roundCount != 0)
            {
                modsRemaining.Add(curMod);
            }
            modsRemaining[modCount].maps.Remove(map);
            UpdateData(mod, map);
            return map;
        }

        // Play All Map Without repeating one
        if (playAllMapInRandom)
        {
            // Choose Mod
            int modIndex = 0;
            if(modsRemaining.Count == 1)
            {
                mod = modsRemaining[0];
            }
            // Randomly
            else
            {
                if(curMod != null)
                {
                    do
                    {
                        modIndex = Random.Range(0, modsRemaining.Count);
                        mod = modsRemaining[modIndex];
                    } while (mod == curMod);
                }
            }

            // Choose Map
            if(mod.maps.Count == 1)
            {
                map = mod.maps[0];
            }
            // Randomly
            else
            {
                do
                {
                    map = mod.maps[Random.Range(0, mod.maps.Count)];
                } while (map == curMap);
            }

            modsRemaining[modIndex].maps.Remove(map);
            if (modsRemaining[modIndex].maps.Count <= 0)
            {
                modsRemaining.Remove(mod);
            }
            UpdateData(mod, map);
            return map;
        }

        // Choose Mod
        if (modsData.mods.Count > 1)
        {
            // Test | Play a Certain Map
            if (startACertainMap)
            {
                bool hasFoundMap = false;
                foreach (Mod aMod in modsData.mods)
                {
                    foreach (string aMap in aMod.maps)
                    {
                        if (aMap == mapToStart)
                        {
                            mod = aMod;
                            hasFoundMap = true;
                            break;
                        }
                    }

                    if (hasFoundMap)
                    {
                        break;
                    }
                }
            }
            // Choose Randomly a mod
            else
            {
                if(roundCount != 0)
                {
                    modsRemaining.Remove(curMod);
                }
                mod = modsRemaining[Random.Range(0, modsRemaining.Count)];
            }
        }
        // Choose the only mod available
        else
        {
            mod = modsData.mods[0];
        }

        // Protection | If Mod contains No Map > return current scene
        if (mod.maps.Count == 0) { return map; }

        // Choose map
        if (mod.maps.Count > 1)
        {
            // Test | Take map given
            if (startACertainMap)
            {
                map = mapToStart;
            }
            // Random between all maps in the chosen mod
            else
            {
                do map = mod.maps[Random.Range(0, mod.maps.Count)];
                while (map == curMap);
            }
        }
        else
        {
            map = mod.maps[0];
        }

        UpdateData(mod, map);
        return map;
    }
    private IEnumerator BeginTransition(string nextMap, bool fromMenu)
    {
        isBusy = true;
        if (nextMap == "") nextMap = SelectNextMap();

        // Wait for animation death player to end
        foreach (Player player in MultiplayerManager.instance.players)
        {
            PlayerAnimations snowAnim = player.GetComponent<PlayerAnimations>();
            if (snowAnim != null)
            {
                snowAnim.VFXSnow.Stop();
                snowAnim.ChangeBoolSnowToFalse();
            }
        }
        yield return new WaitForSecondsRealtime(2.0f);
        //--------

        Time.timeScale = .5f;
        timeScale = Time.timeScale;
        if (curMapRoot == null) curMapRoot = GameObject.Find("MapRoot");
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(nextMap, LoadSceneMode.Additive);
        asyncOp.allowSceneActivation = false;
        float timeToLoad = 0;
        while (!asyncOp.isDone)
        {
            timeToLoad += Time.unscaledDeltaTime;
            if (asyncOp.progress >= .9f)
            {
                if (!fromMenu && timeToLoad < slowTime) yield return new WaitForSecondsRealtime(slowTime - timeToLoad);
                asyncOp.allowSceneActivation = true;
            }
            yield return null;
        }
        Time.timeScale = 0;
        timeScale = Time.timeScale;
        GameObject[] objs = GameObject.FindGameObjectsWithTag("MapRoot");
        nextMapRoot = objs[objs.Length - 1];
        nextMapRoot.transform.position = new Vector3(mapOffset, 0);
        // CAMERA EVENT
        GameEvents.OnSceneUnloaded.Invoke();
        // ----------
        GameObject[] nextStartPos = GameObject.FindGameObjectsWithTag("StartPos");
        MultiplayerManager.instance.speedChangeMap = 1 / slowTime;
        MultiplayerManager.instance.StartChangeMap(nextStartPos[nextStartPos.Length - 1].transform);
        while (MultiplayerManager.instance.isChangingMap) yield return null;
        StartCoroutine(EndTransition());
    }
    public IEnumerator EndTransition()
    {
      
        //Vector3 v0 = Vector3.zero, v1 = Vector3.zero, d0 = Vector3.one, d1 = Vector3.one;
   
        //while (d0.sqrMagnitude > smoothMOE && d1.sqrMagnitude > smoothMOE)
        //{
        //    curMapRoot.transform.position = Vector3.SmoothDamp(curMapRoot.transform.position, new Vector3(-mapOffset, 0), ref v0, smoothTime, Mathf.Infinity, Time.unscaledDeltaTime);
        //    nextMapRoot.transform.position = Vector3.SmoothDamp(nextMapRoot.transform.position, Vector3.zero, ref v1, smoothTime, Mathf.Infinity, Time.unscaledDeltaTime);
        //    d0 = curMapRoot.transform.position - new Vector3(-mapOffset, 0);
        //    d1 = nextMapRoot.transform.position - Vector3.zero;
        //    yield return null;
        //}
       
        AsyncOperation asyncOp = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

        while (!asyncOp.isDone)
        {        
             yield return null;
        }
        shaderScript.SetShaders(true);
        nextMapRoot.transform.position = Vector3.zero;
        // Switch camera state depending of map mode
        camManager.SwitchStates(Utils.GetCameraType(curModName));
        //-------
        Time.timeScale = 1;
        timeScale = Time.timeScale;
        curMapRoot = nextMapRoot;
        nextMapRoot = null;
        isBusy = false;
        _coroutine = null;
        shaderScript.AllObjectsAppear();
        var lvl = FindObjectOfType<Level>();
        if (lvl != null) StartCoroutine(lvl.Init());
        levelEnded = false;
        yield return null;
    }
}
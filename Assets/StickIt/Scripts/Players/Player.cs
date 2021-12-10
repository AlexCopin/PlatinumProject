using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
public class Player : MonoBehaviour
{
    private MultiplayerManager _multiplayerManager;
    public PlayerMouvement myMouvementScript;
    public MultiplayerManager.PlayerData myDatas;
    public MoreMountains.Feedbacks.MMFeedbacks deathAnim;
    public GameObject deathPart;
    public bool isDead;
    [SerializeField] private int minMass = 100;
    [SerializeField] private int maxMass = 250;
    private void Awake()
    {
        _multiplayerManager = MultiplayerManager.instance;
        if (TryGetComponent<PlayerMouvement>(out PlayerMouvement pm))
        {
            myMouvementScript = pm;
            myMouvementScript.myPlayer = this;
        }
        DontDestroyOnLoad(this);
    }
    public void Death(bool intensityAnim = false)
    {
        isDead = true;
        myMouvementScript.enabled = false;
        _multiplayerManager.alivePlayers.Remove(this);
        _multiplayerManager.deadPlayers.Add(this);
        // Play Death Animation
        StartCoroutine(OnDeath(intensityAnim));
    }
    private IEnumerator OnDeath(bool intensityAnim)
    {
        deathAnim.PlayFeedbacks();
        if (intensityAnim)
        {
            //AudioManager.instance.PlayDeathSounds(this.gameObject);
            yield return new WaitForSeconds(deathAnim.TotalDuration / deathAnim.DurationMultiplier);
        }
        myMouvementScript.Death();
        GameObject obj = Instantiate(deathPart, new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z), Quaternion.identity);
        obj.GetComponent<ParticleSystemRenderer>().material = myDatas.material;
        GameEvents.CameraShake_CEvent?.Invoke(2.0f, 1.0f);
        //
        // ParticleSystem ps = obj.GetComponent<ParticleSystem>();
        // ParticleSystem.Particle[] particles = new ParticleSystem.Particle[ps.particleCount];
        // int length = ps.GetParticles(particles);
        // for (int i = 0; i < length; i++)
        // {
        //     particles[i].position = Vector3.zero;
        // }
        //
        MapManager.instance.EndLevel();
    }
    public void PrepareToChangeLevel() // When the player is still alive
    {
        if (!isDead)
        {
            myMouvementScript.enabled = false;
            foreach (Collider col in GetComponentsInChildren<Collider>())
            {
                col.enabled = false;
            }
            foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>())
            {
                rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
                rb.isKinematic = true;
            }
        }
    }
    public void Respawn()
    {
        myMouvementScript.enabled = true;
        myMouvementScript.Respawn();
        isDead = false;
    }
    public void SetScoreAndMass(bool isWin, uint score, int mass)
    {
        myDatas.score += score;
        if (isWin)
        {
            myDatas.mass += mass;
        }
        else
        {
            myDatas.mass -= mass;
        }
        myDatas.mass = Mathf.Clamp(myDatas.mass, minMass, maxMass);
    }
    // INPUT TOOLS TO REMOVE LATER
    public void InputTestMassP25(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (context.started)
        {
            SetScoreAndMass(true, 0, 25);
            myMouvementScript.RescaleMeshWithMass();
        }
    }
    public void InputTestMassP5(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (context.started)
        {
            SetScoreAndMass(true, 0, 5);
            myMouvementScript.RescaleMeshWithMass();
        }
    }
    public void InputTestMassM25(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (context.started)
        {
            SetScoreAndMass(false, 0, 25);
            myMouvementScript.RescaleMeshWithMass();
        }
    }
    public void InputTestMassM5(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (context.started)
        {
            SetScoreAndMass(false, 0, 5);
            myMouvementScript.RescaleMeshWithMass();
        }
    }
    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed && !isDead && !MapManager.instance.isBusy)
        {
            AkSoundEngine.PostEvent("Play_SFX_UI_Return", gameObject);
            if (MapManager.instance.curMod == "MusicalChairs" && FindObjectOfType<MusicalChairManager>().inTransition)
                for (int i = 0; i < Gamepad.all.Count; i++)
                    Gamepad.all[i].PauseHaptics();
            foreach (var item in FindObjectsOfType<PlayerInput>()) item.enabled = false;
            Pause.instance.GetComponent<PlayerInput>().enabled = true;
            Pause.instance.mainLayerSwitch.ChangeLayer("Layer_Main");
            Pause.instance.PauseGame();
        }
    }
    public void SoundReturn(InputAction.CallbackContext context)
    { if (context.performed && !isDead && !MapManager.instance.isBusy) AkSoundEngine.PostEvent("Play_SFX_UI_Return", gameObject); }
}
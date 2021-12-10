using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Chair : MonoBehaviour
{
    private MusicalChairManager musicalChairManager;
    [Header("Player")]
    public List<Player> playersInChair;
    public Player chosenOne;
    [Header("Settings")]
    [SerializeField]
    public float offsetSpawn;
    private float duration;
    private bool isSpawnAnimation;
    public AnimationCurve animCurve;
    [HideInInspector]
    public bool isActive;
    [HideInInspector]
    public bool isTaken;
    private Color activatedColor;
    private Color deactivatedColor;
    private Vector3 spawnPosition;
    private Vector3 originalPos;
    [SerializeField]
    public GameObject shield;
    [Header("ChosenOne")]
    private LineRenderer lr;
    [SerializeField]
    private Transform lrBeginPos;
    private MeshRenderer myMeshRenderer;
    // Start is called before the first frame update
    private void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.SetPosition(0, lrBeginPos.position + transform.forward * offsetSpawn);
        lr.enabled = false;
        myMeshRenderer = GetComponent<MeshRenderer>();
        musicalChairManager = FindObjectOfType<MusicalChairManager>();
        duration = musicalChairManager.durationSpawn;
        spawnPosition = transform.position + transform.forward * offsetSpawn;
        originalPos = transform.position;
        shield.SetActive(false);
    }
    // Update is called once per frame
    private void Update()
    {
        TestPlayer();
    }
    private void TestPlayer()
    {
        if (isTaken)
        {
            for (int i = 1; i < playersInChair.Count; i++)
            {
                if (playersInChair.Count > 1 && Vector3.Distance(chosenOne.transform.position, transform.position) > Vector3.Distance(playersInChair[i].transform.position, transform.position))
                {
                    chosenOne = playersInChair[i];
                }
            }
            if (chosenOne)
            {
                shield.transform.SetParent(chosenOne.transform);
                shield.transform.localPosition = new Vector3(0, 0, 0);
                lr.SetPosition(1, chosenOne.transform.position);
            }
        }
    }
    public void ActivateChair(float duration)
    {
        StartCoroutine(SpawnChairCor(duration));
        isActive = true;
    }
    public void DeactivateChair(float duration)
    {
        //FAIRE TREMBLER LA CHAISE A L'INVERSE
        if (isTaken)
            musicalChairManager.winners.Add(chosenOne);
        isActive = false;
        lr.enabled = false;
        chosenOne = null;
        playersInChair.Clear();
        StartCoroutine(DespawnChairCor(duration));
    }
    private IEnumerator SpawnChairCor(float duration)
    {
        isSpawnAnimation = true;
        float elapsed = 0;
        float ratio = 0;
        myMeshRenderer.material = musicalChairManager.chairTaken;
        while (elapsed < duration)
        {
            ratio = elapsed / duration;
            ratio = animCurve.Evaluate(ratio);
            transform.position = Vector3.Lerp(originalPos, spawnPosition, ratio);
            elapsed += Time.deltaTime;
            yield return null;
        }
        isSpawnAnimation = false;
        if (!isTaken)
            myMeshRenderer.material = musicalChairManager.chairNotTaken;
    }
    private IEnumerator DespawnChairCor(float duration)
    {
        float elapsed = 0;
        float ratio = 0;
        isSpawnAnimation = true;
        while (elapsed < duration)
        {
            ratio = elapsed / duration;
            ratio = animCurve.Evaluate(ratio);
            transform.position = Vector3.Lerp(spawnPosition, originalPos, ratio);
            elapsed += Time.deltaTime;
            yield return null;
        }
        isSpawnAnimation = false;
        DeactivateShield();
    }
    public void DeactivateShield()
    {
        shield.transform.SetParent(transform);
        shield.SetActive(false);
    }
    private void OnTriggerEnter(Collider c)
    {
        if (isActive)
        {
            if (c.tag == "Player")
            {
                playersInChair.Add(c.gameObject.GetComponentInParent<Player>());
                if (playersInChair.Count == 1)
                {
                    isTaken = true;
                    lr.enabled = true;
                    if (!isSpawnAnimation)
                        myMeshRenderer.material = musicalChairManager.chairTaken;
                    chosenOne = playersInChair[0];
                    shield.SetActive(true);
                }
            }
        }
    }
    private void OnTriggerExit(Collider c)
    {
        if (isActive)
        {
            if (c.tag == "Player")
            {
                playersInChair.Remove(playersInChair.Find(x => c.gameObject.GetComponentInParent<Player>()));
                if (playersInChair.Count < 1)
                {
                    isTaken = false;
                    lr.enabled = false;
                    if (!isSpawnAnimation)
                        myMeshRenderer.material = musicalChairManager.chairNotTaken;
                    shield.transform.SetParent(transform);
                    shield.SetActive(false);
                }
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawMesh(GetComponent<MeshFilter>().sharedMesh, transform.position + transform.forward * offsetSpawn, transform.rotation, transform.localScale);
        Gizmos.DrawCube(lrBeginPos.position, new Vector3(0.5f, 0.5f, 0.5f));
    }
}
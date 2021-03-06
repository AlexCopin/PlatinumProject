using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class RunnerEndCheckpoint : MonoBehaviour
{
    public RunnerManager runnerManager;
    public int scoreBonus = 100;
    [Header("-------- DEBUG ---------")]
    [SerializeField] private float timer;
    [SerializeField] private BoxCollider boxCollider;
    private void Awake()
    {
        if(runnerManager == null)
        {
            runnerManager = FindObjectOfType<RunnerManager>();
        }

        boxCollider = GetComponent<BoxCollider>();
        boxCollider.isTrigger = true;
    }

    private void Update()
    {
        timer += Time.deltaTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        Player player = other.gameObject.GetComponentInParent<Player>();
        if (player != null)
        {
            if (!runnerManager.GetOrder().Contains(player))
            {
                runnerManager.AddArriveTime(timer);
                runnerManager.AddOrder(player);
                runnerManager.AddBonus(scoreBonus);
                runnerManager.TriggerEndCheckpoint(true);
            }
        }
    }
}

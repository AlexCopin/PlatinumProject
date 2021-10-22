using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chair : MonoBehaviour
{
    MusicalChairManager musicalChairManager;
    public bool isActive;
    public bool isTaken;
    public List<Player> playersInChair;
    public Player chosenOne;
    // Start is called before the first frame update
    void Start()
    {
        musicalChairManager = FindObjectOfType<MusicalChairManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            if (playersInChair.Count > 0)
            {
                isTaken = true;
                chosenOne = playersInChair[0];
                for (int i = 0; i < playersInChair.Count; i++)
                {
                    if(Vector3.Distance(chosenOne.transform.position, transform.position) > Vector3.Distance(playersInChair[i].transform.position, transform.position))
                    {
                        chosenOne = playersInChair[i];
                    }
                }
            }
        }
    }

    public void ActivateChair(Color c)
    {
        //FAIRE TREMBLER LA CHAISE
        isActive = true;
        gameObject.GetComponent<MeshRenderer>().material.color = c;
    }
    public void DeactivateChair(Color c)
    {
        //FAIRE TREMBLER LA CHAISE A L'INVERSE
        if (isActive)
        {
            gameObject.GetComponent<MeshRenderer>().material.color = c;
            if (isTaken)
                musicalChairManager.chosenOnes.Add(chosenOne);
            isActive = false;
        }
    }
    private void OnTriggerEnter(Collider c)
    {
        Debug.Log(c.gameObject.name);

        if (isActive)
        {
            if (c.gameObject.transform.parent.parent.CompareTag("Player"))
            {
                playersInChair.Add(c.gameObject.GetComponentInParent<Player>());
            }
        }
    }
    private void OnTriggerExit(Collider c)
    {
            if (c.gameObject.transform.parent.parent.CompareTag("Player"))
            {
               playersInChair.Remove(playersInChair.Find(x => c.gameObject.GetComponentInParent<Player>()));
            }
        
    }
}

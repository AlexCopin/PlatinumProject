using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EndScore : MonoBehaviour
{
    public GameObject[] panelPlayers;
    public TMP_Text[] textP;
    public TMP_Text[] textScores;
    public RawImage[] rawImages;

    [Header("Debug____________________")]
    private Player[] ranking;

    private void Start()
    {
        ranking = new Player[MultiplayerManager.instance.players.Count];
        MultiplayerManager.instance.players.CopyTo(ranking);

        // Debug
        ranking[1].myDatas.score = 10;

        bool hasPermute = false;
        do
        {
            hasPermute = false;
            for (int i = 0; i < ranking.Length - 1; i++)
            {
                if (ranking[i].myDatas.score < ranking[i + 1].myDatas.score)
                {
                    Swap(i, i + 1);
                    hasPermute = true;
                }
            }
        } while (hasPermute);

        foreach(GameObject panel in panelPlayers)
        {
            panel.SetActive(false);
        }
        for(int i = 0; i < ranking.Length; i++)
        {
            panelPlayers[i].SetActive(true);
            textP[i].text = "P" + ranking[i].myDatas.id.ToString();
            textScores[i].text = ranking[i].myDatas.score.ToString();
            rawImages[i].texture = ranking[i].myDatas.renderTexture;
        }
    }

    private void Swap(int i, int j)
    {
        Player temp = ranking[i];
        ranking[i] = ranking[j];
        ranking[j] = temp;
    }
}

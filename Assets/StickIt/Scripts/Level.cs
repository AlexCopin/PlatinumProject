using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Level : MonoBehaviour
{
    // Start is called before the first frame update

    protected GameManager _gameManager;
    protected MultiplayerManager _multiplayerManager;
    public List<Player> winners;
    public Transform startingPos;

    void Start()
    {
        StartMap();
    }
    protected virtual void StartMap()
    {
        _multiplayerManager.InstantiatePlayers();
        _multiplayerManager = MultiplayerManager.instance;
        _multiplayerManager.playersStartingPos = startingPos;
        _multiplayerManager.ChangeMap();

        _gameManager = GameManager.instance;
    }
}

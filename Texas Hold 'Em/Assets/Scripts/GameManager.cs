﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public int startingChips = 1000;
    public static GameManager instance;

    public static Dictionary<int, PlayerManager> players = new Dictionary<int, PlayerManager>();


    public Canvas WorldSpaceCanvas;
    //public static Dictionary<int, ItemSpawner> itemSpawners = new Dictionary<int, ItemSpawner>();

    public GameObject localPlayerPrefab;
    public GameObject playerPrefab;
    //public GameObject itemSpawnerPrefab;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Initialize();
            
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    public void Initialize()
    {
    }

    /// <summary>Spawns a player.</summary>
    /// <param name="_id">The player's ID.</param>
    /// <param name="_name">The player's name.</param>

    public void SpawnPlayer(int _id, string _username)
    {
        GameObject _player;
        if (_id == Client.instance.myId)
        {
            _player = Instantiate(localPlayerPrefab);
            WorldSpaceCanvas.worldCamera = _player.GetComponentInChildren<Camera>();
        }
        else
        {
            _player = Instantiate(playerPrefab);
        }

        _player.GetComponent<PlayerManager>().Initialize(_id, _username);
        _player.GetComponent<PlayerManager>().chipTotal = startingChips; // temp hard code for giving chip total
        players.Add(_id, _player.GetComponent<PlayerManager>());
    }

    public static void ResetTable()
    {

    }

    //public void CreateItemSpawner(int _spawnerId, Vector3 _position, bool _hasItem)
    //{
    //    GameObject _spawner = Instantiate(itemSpawnerPrefab, _position, itemSpawnerPrefab.transform.rotation);
    //    _spawner.GetComponent<ItemSpawner>().Initialize(_spawnerId, _hasItem);
    //    itemSpawners.Add(_spawnerId, _spawner.GetComponent<ItemSpawner>());
    //}
}
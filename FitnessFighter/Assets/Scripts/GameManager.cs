﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public string stringSeed = "seed string";
    public bool useStringSeed = true;
    public int seed;
    public bool randomizeSeed;
    public static GameManager SharedInstance = null;

    public int roomsPassed = 0;
    public int amountOFRoomsInDungeon = 10;
    public RoomsCollections rooms;

    public List<GameObject> roomsEntered;

    private void Awake()
    {
        if (SharedInstance != null && SharedInstance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            SharedInstance = this;
        }

        if (useStringSeed)
        {
            seed = stringSeed.GetHashCode();
        }

        if (randomizeSeed)
        {
            seed = Random.Range(0, 99999);
        }

        Random.InitState(seed);

    }

    public void DeactivatePrevRoom()
    {
        roomsEntered[roomsPassed - 1].SetActive(false);
    }

}
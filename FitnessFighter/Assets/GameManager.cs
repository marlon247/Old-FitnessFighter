using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager SharedInstance = null;

    public int roomsPassed = 0;
    public int amountOFRoomsInDungeon = 10;
    public Rooms rooms;

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
    }

    public void DeactivatePrevRoom()
    {
        roomsEntered[roomsPassed - 1].SetActive(false);
    }

}

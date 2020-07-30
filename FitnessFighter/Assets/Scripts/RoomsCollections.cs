using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomsCollections : MonoBehaviour {

    public List<GameObject> bossRooms;
    public List<GameObject> startRooms;
    public List<GameObject> rooms;
    public List<GameObject> door1;
    public List<GameObject> door2;
    public List<GameObject> door3;
    public List<GameObject> door4;
    public List<GameObject> leftRooms;
    public List<GameObject> rightRooms;
    public List<GameObject> topRooms;
    public List<GameObject> bottomRooms;
    public List<List<GameObject>> dirRooms { get { return new List<List<GameObject>> { topRooms, rightRooms, bottomRooms, leftRooms }; } }

}

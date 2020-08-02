using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomObject
{
    public GameObject room;
    public Vector2 coord;
    public bool isStartRoom;
    public bool isBossRoom;

    public RoomObject() 
    {
        isStartRoom = false;
        isBossRoom = false;
    }
    public RoomObject(GameObject r, Vector2 c)
    {
        room = r;
        coord = c;
        isStartRoom = false;
        isBossRoom = false;
    }
}

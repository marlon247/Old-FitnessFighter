using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;

public class CreateMap : MonoBehaviour
{
    private class RoomObject
    {
        public GameObject room;
        public Vector2 coord;

        public RoomObject() { }
        public RoomObject(GameObject r, Vector2 c)
        {
            room = r;
            coord = c;
        }
    }

    private Dictionary<Vector2, RoomObject> map;
    private GameManager gameManager;
    private Queue<RoomObject> bfsQueue;
    private RoomObject startRoom = new RoomObject();
    private RoomObject endRoom;
    private Dictionary<Vector4, GameObject> doorsToRoomMap;

    

    private void Awake()
    {
        doorsToRoomMap = new Dictionary<Vector4, GameObject>();
        foreach(GameObject room in GameManager.SharedInstance.rooms.rooms)
        {
            doorsToRoomMap.Add(room.GetComponent<RoomProperties>().doors, room);
        }
    }

    public void GenerateMap()
    {
        bool validMap = false;
        while ( !validMap ) 
        {
            validMap = bfsMapCreation();
        };
        DrawMap();
    }

    private bool bfsMapCreation()
    {
        bool validMap = true;

        map = new Dictionary<Vector2, RoomObject>();
        bfsQueue = new Queue<RoomObject>();
        RoomsCollections rooms = GameManager.SharedInstance.rooms;
        startRoom.room = rooms.startRooms[Random.Range(0, rooms.startRooms.Count)];
        startRoom.coord = new Vector2(0, 0);
        bfsQueue.Enqueue(startRoom);

        while (bfsQueue.Count != 0 && map.Count < 15)
        {
            
            RoomObject currRoom = bfsQueue.Dequeue();

            if (!map.ContainsKey(currRoom.coord))
            {
                map.Add(currRoom.coord, currRoom);
            }
            else
            {
                continue;
            }

            if (currRoom.room.GetComponent<RoomProperties>().Top)
            {
                Vector2 tempCoord = currRoom.coord + Vector2.up;
                if (!map.ContainsKey(tempCoord))
                {
                    GameObject tempRoom = rooms.bottomRooms[Random.Range(0, rooms.bottomRooms.Count)];
                    bfsQueue.Enqueue(new RoomObject(tempRoom, tempCoord));
                }
                else
                {
                    RoomObject adjRoom = map[tempCoord];
                    if (!adjRoom.room.GetComponent<RoomProperties>().Bottom)
                    {
                        map[tempCoord] = changeRoomDoor(adjRoom, new Vector4(0, 0, 1, 0), 1);
                    }
                }
            }

            if (currRoom.room.GetComponent<RoomProperties>().Right)
            {
                Vector2 tempCoord = currRoom.coord + Vector2.right;
                if (!map.ContainsKey(tempCoord))
                {
                    GameObject tempRoom = rooms.leftRooms[Random.Range(0, rooms.leftRooms.Count)];
                    bfsQueue.Enqueue(new RoomObject(tempRoom, tempCoord));
                }
                else
                {
                    RoomObject adjRoom = map[tempCoord];
                    if (!adjRoom.room.GetComponent<RoomProperties>().Left)
                    {
                        map[tempCoord] = changeRoomDoor(adjRoom, new Vector4(0, 0, 0, 1), 1);
                    }
                }
            }

            if (currRoom.room.GetComponent<RoomProperties>().Bottom)
            {
                Vector2 tempCoord = currRoom.coord + Vector2.down;
                if (!map.ContainsKey(tempCoord))
                {
                    GameObject tempRoom = rooms.topRooms[Random.Range(0, rooms.topRooms.Count)];
                    bfsQueue.Enqueue(new RoomObject(tempRoom, tempCoord));
                }
                else
                {
                    RoomObject adjRoom = map[tempCoord];
                    if (!adjRoom.room.GetComponent<RoomProperties>().Top) 
                    {
                        map[tempCoord] = changeRoomDoor(adjRoom, new Vector4(1, 0, 0, 0), 1);
                    }
                }
            }

            if (currRoom.room.GetComponent<RoomProperties>().Left)
            {
                Vector2 tempCoord = currRoom.coord + Vector2.left;
                if (!map.ContainsKey(tempCoord))
                {
                    GameObject tempRoom = rooms.rightRooms[Random.Range(0, rooms.rightRooms.Count)];
                    bfsQueue.Enqueue(new RoomObject(tempRoom, tempCoord));
                }
                else
                {
                    RoomObject adjRoom = map[tempCoord];
                    if (!adjRoom.room.GetComponent<RoomProperties>().Right)
                    {
                        map[tempCoord] = changeRoomDoor(adjRoom, new Vector4(0, 1, 0, 0), 1);
                    }
                }
            }
        }

        //remove extra rooms from queue and add close doors to out of map.
        while(bfsQueue.Count != 0)
        {
            RoomObject extraRoom = bfsQueue.Dequeue();
            RoomProperties extraRoomProperties = extraRoom.room.GetComponent<RoomProperties>();

            if (extraRoomProperties.Top)
            {
                Vector2 adjRoomCoord = extraRoom.coord + Vector2.up;
                if (map.ContainsKey(adjRoomCoord))
                {
                    RoomObject adjRoom = map[adjRoomCoord];
                    if (adjRoom.room.GetComponent<RoomProperties>().Bottom)
                    {
                        map[adjRoomCoord] = changeRoomDoor(adjRoom, new Vector4(0, 0, 1, 0), -1);
                    }
                }
            }
            if (extraRoomProperties.Right)
            {
                Vector2 adjRoomCoord = extraRoom.coord + Vector2.right;
                if (map.ContainsKey(adjRoomCoord))
                {
                    RoomObject adjRoom = map[adjRoomCoord];
                    if (adjRoom.room.GetComponent<RoomProperties>().Left)
                    {
                        map[adjRoomCoord] = changeRoomDoor(adjRoom, new Vector4(0, 0, 0, 1), -1);
                    }
                }
            }
            if (extraRoomProperties.Bottom)
            {
                Vector2 adjRoomCoord = extraRoom.coord + Vector2.down;
                if (map.ContainsKey(adjRoomCoord))
                {
                    RoomObject adjRoom = map[adjRoomCoord];
                    if (adjRoom.room.GetComponent<RoomProperties>().Top)
                    {
                        map[adjRoomCoord] = changeRoomDoor(adjRoom, new Vector4(1, 0, 0, 0), -1);
                    }
                }
            }
            if (extraRoomProperties.Left)
            {
                Vector2 adjRoomCoord = extraRoom.coord + Vector2.left;
                if (map.ContainsKey(adjRoomCoord))
                {
                    RoomObject adjRoom = map[adjRoomCoord];
                    if (adjRoom.room.GetComponent<RoomProperties>().Right)
                    {
                        map[adjRoomCoord] = changeRoomDoor(adjRoom, new Vector4(0, 1, 0, 0), -1);
                    }
                }
            }

        }



        if(map.Count < 13)
        {
            validMap = false;
        }

        return validMap;
    }

    private RoomObject changeRoomDoor(RoomObject room, Vector4 door, int addRemove)
    {
        RoomProperties RoomProperties = room.room.GetComponent<RoomProperties>();
        Vector4 newDoor = RoomProperties.doors;

        for(int i = 0; i < 4; ++i)
        {
            if(addRemove == -1 && RoomProperties.doors[i] == 1 && door[i] == 1) {
                newDoor[i] = 0;
            }else if(addRemove == 1 && RoomProperties.doors[i] == 0 && door[i] == 1)
            {
                newDoor[i] = 1;
            }
        }

        GameObject toReturnRoom = doorsToRoomMap[newDoor];
        return new RoomObject(toReturnRoom, room.coord);
    }

    public void DrawMap()
    {
        foreach(KeyValuePair<Vector2, RoomObject> room in map)
        {
            GameObject go = room.Value.room;
            RoomProperties rt = go.GetComponent<RoomProperties>();
            Vector3 roomLocation = new Vector3(room.Key.x * rt.xScale, 0, room.Key.y * rt.yScale);
            Instantiate(go, roomLocation, Quaternion.identity);
        }
    }

}


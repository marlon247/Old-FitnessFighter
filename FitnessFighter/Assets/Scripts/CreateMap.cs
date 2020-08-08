using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;

public class CreateMap : MonoBehaviour
{

    public List<GameObject> rooms;
    
    private RoomsCollections roomsCollections;
    private Dictionary<Vector2, RoomObject> map;
    private GameManager gameManager;
    private Queue<RoomObject> bfsQueue;
    private RoomObject startRoom = new RoomObject();
    private RoomObject endRoom;
    private Dictionary<Vector4, GameObject> doorsToRoomMap;
    private List<Vector2> direction { get { return new List<Vector2> { Vector2.up, Vector2.right, Vector2.down, Vector2.left }; } }
    private List<Vector4> altDoors { get { return new List<Vector4> { new Vector4(1,0,0,0), new Vector4(0, 1, 0, 0), new Vector4(0, 0, 1, 0), new Vector4(0, 0, 0, 1) }; } }

    private void Awake()
    {
        doorsToRoomMap = new Dictionary<Vector4, GameObject>();
        foreach(GameObject room in GameManager.SharedInstance.rooms.rooms)
        {
            doorsToRoomMap.Add(room.GetComponent<RoomProperties>().doors, room);
        }

        roomsCollections = GameManager.SharedInstance.rooms;
    }

    public void GenerateMap()
    {
        bool validMap = false;
        while ( !validMap ) 
        {
            validMap = bfsMapCreation();
        };
        DrawMap();
        rooms[rooms.Count - 1].GetComponentInChildren<Renderer>().material.color = Color.red;
    }

    private bool bfsMapCreation()
    {
        bool validMap = true;

        map = new Dictionary<Vector2, RoomObject>();
        bfsQueue = new Queue<RoomObject>();
        RoomsCollections rooms = GameManager.SharedInstance.rooms;
        startRoom.room = rooms.startRooms[Random.Range(0, rooms.startRooms.Count)];
        startRoom.coord = new Vector2(0, 0);
        startRoom.isStartRoom = true;
        bfsQueue.Enqueue(startRoom);

        while (bfsQueue.Count != 0)
        {
            RoomObject currRoom = bfsQueue.Dequeue();

            if (!map.ContainsKey(currRoom.coord))
            {
                if (map.Count < 15)
                {
                    map.Add(currRoom.coord, currRoom);
                }
            }
            else
            {
                currRoom = map[currRoom.coord];
                for (int i = 0; i < 4; i++)
                {
                    if(currRoom.room.GetComponent<RoomProperties>().doors[i] == 0)
                    {
                        Vector2 tempCoord = currRoom.coord + direction[i];

                        if (map.ContainsKey(tempCoord))
                        {
                            RoomObject adjRoom = map[tempCoord];
                            
                            map[tempCoord] = changeRoomDoor(adjRoom, altDoors[(i+2)%4], -1);
                        }
                    }
                }
                continue;
            }

            for(int i = 0; i < 4; i++)
            {
                if (currRoom.room.GetComponent<RoomProperties>().doors[i] == 1)
                {
                    Vector2 tempCoord = currRoom.coord + direction[i];
                    if (!map.ContainsKey(tempCoord))
                    {
                        if (map.Count < 15)
                        {
                            GameObject tempRoom = rooms.dirRooms[(i + 2) % 4][Random.Range(0, rooms.dirRooms[(i + 2) % 4].Count)];
                            bfsQueue.Enqueue(new RoomObject(tempRoom, tempCoord));
                        }
                    }
                    else
                    {
                        RoomObject adjRoom = map[tempCoord];
                        if (adjRoom.room.GetComponent<RoomProperties>().doors[(i+2) % 4] == 0)
                        {
                            map[tempCoord] = changeRoomDoor(adjRoom, altDoors[(i+2) % 4], 1);
                        }

                    }
                }
            }
        }

        if (map.Count < 13)
        {
            return false;
        }

        List<Vector2> keys = new List<Vector2>(map.Keys );
        List<Vector2> pBossRooms = new List<Vector2>();

        foreach (Vector2 coord in keys)
        {
            for(int i = 0; i < 4; ++i)
            {
                RoomObject roomObj = map[coord];
                GameObject room = roomObj.room;
                RoomProperties roomProp = room.GetComponent<RoomProperties>();

                if (roomProp.doors[i] == 1)
                {
                    Vector2 newCoord = coord + direction[i];

                    if (!map.ContainsKey(newCoord))
                    {
                        map[coord] = changeRoomDoor(map[coord], altDoors[i], -1);

                        if (validBossRoom(newCoord)) { pBossRooms.Add(newCoord); }
                    }
                }
            }
        }

        Vector2 bossCoord = pBossRooms[Random.Range(0, pBossRooms.Count)];
        addBossRoom(bossCoord);

        return validMap;
    }

    private void addBossRoom(Vector2 coord)
    {
        for (int i = 0; i < 4; ++i)
        {
            Vector2 adjCoord = coord + direction[i];

            if (map.ContainsKey(adjCoord))
            {
                GameObject room = roomsCollections.door1[i];
                RoomObject bossRoom = new RoomObject(room, coord);
                bossRoom.isBossRoom = true;
                map.Add(coord, bossRoom);
                room.tag = "BossRoom";
                map[adjCoord] = changeRoomDoor(map[adjCoord], altDoors[(i + 2) % 4], 1);


            }
        }
    }

    private bool validBossRoom(Vector2 coord)
    {
        int adjCount = 0;
        for(int i = 0; i < 4; ++i)
        {
            Vector2 adjCoord = coord + direction[i];
            if (map.ContainsKey(adjCoord)) { adjCount++; }
        }
        return adjCount == 1;
    }

    /*
     * Params: room: initial room type, door: doors to be modified(1), addRemove: 1 to add door, -1 to remove door
     * Return: a new room with door modifications from params  
     */
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
            GameObject newRoom;
            GameObject go = room.Value.room;
            RoomProperties rt = go.GetComponent<RoomProperties>();
            Vector3 roomLocation = new Vector3(room.Key.x * rt.xScale, 0, room.Key.y * rt.yScale);
            newRoom = Instantiate(go, roomLocation, Quaternion.identity);
            rooms.Add(newRoom);
        }
    }
}

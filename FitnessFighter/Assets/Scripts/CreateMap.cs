using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.UIElements;

//using rooms = GameManager.SharedInstance.rooms;

public class CreateMap : MonoBehaviour
{
    private Dictionary<Vector2, GameObject> map;
    private GameManager gameManager;
    private Queue<GameObject> bfsQueue;
    private GameObject startRoom;
    private GameObject endRoom;
    private int size = 0;
    private Dictionary<Vector4, GameObject> doorsToRoomMap;

    private void Awake()
    {
        foreach(GameObject room in GameManager.SharedInstance.rooms.rooms)
        {
            doorsToRoomMap.Add(room.GetComponent<RoomType>().doors, room);
        }
    }

    void Start()
    {


        while (!bfsMapCreation()) ;
        //TODO: if queue still has stuff after reaching max size remove doors by checking the area around
        //      open doors if one side is open and not the other.
    }


    private bool bfsMapCreation()
    {
        bool validMap = true;
        
        map = new Dictionary<Vector2, GameObject>();
        bfsQueue = new Queue<GameObject>();
        RoomsCollections rooms = GameManager.SharedInstance.rooms;
        startRoom = rooms.startRooms[Random.Range(0, rooms.startRooms.Count)];
        startRoom.GetComponent<RoomType>().coord = new Vector2(0, 0);
        bfsQueue.Enqueue(startRoom);

        while (bfsQueue.Count != 0 && size < 15)
        {
            size++;
            GameObject currRoom = bfsQueue.Dequeue();

            if (!map.ContainsKey(currRoom.GetComponent<RoomType>().coord))
            {
                map.Add(currRoom.GetComponent<RoomType>().coord, currRoom);
            }
            else
            {
                continue;
            }

            if (currRoom.GetComponent<RoomType>().Top)
            {
                Vector2 tempCoord = currRoom.GetComponent<RoomType>().coord + Vector2.up;
                if (!map.ContainsKey(tempCoord))
                {
                    GameObject tempRoom = rooms.bottomRooms[Random.Range(0, rooms.bottomRooms.Count)];
                    tempRoom.GetComponent<RoomType>().coord = tempCoord;
                    bfsQueue.Enqueue(tempRoom);
                }
                else
                {
                    GameObject adjRoom = map[tempCoord];
                    RoomType adjRoomType = adjRoom.GetComponent<RoomType>();

                    if (!adjRoomType.Bottom)
                    {
                        adjRoom = doorsToRoomMap[adjRoomType.doors + new Vector4(0, 0, 1, 0)];
                        adjRoom.GetComponent<RoomType>().coord = adjRoomType.coord;
                    }
                }
            }

            if (currRoom.GetComponent<RoomType>().Right)
            {
                Vector2 tempCoord = currRoom.GetComponent<RoomType>().coord + Vector2.right;
                if (!map.ContainsKey(tempCoord))
                {
                    GameObject tempRoom = rooms.leftRooms[Random.Range(0, rooms.leftRooms.Count)];
                    tempRoom.GetComponent<RoomType>().coord = tempCoord;
                    bfsQueue.Enqueue(tempRoom);
                }
                else
                {
                    GameObject adjRoom = map[tempCoord];
                    RoomType adjRoomType = adjRoom.GetComponent<RoomType>();

                    if (!adjRoomType.Left)
                    {
                        adjRoom = doorsToRoomMap[adjRoomType.doors + new Vector4(0, 0, 0, 1)];
                        adjRoom.GetComponent<RoomType>().coord = adjRoomType.coord;
                    }
                }
            }

            if (currRoom.GetComponent<RoomType>().Bottom)
            {
                Vector2 tempCoord = currRoom.GetComponent<RoomType>().coord + Vector2.down;
                if (!map.ContainsKey(tempCoord))
                {
                    GameObject tempRoom = rooms.topRooms[Random.Range(0, rooms.topRooms.Count)];
                    tempRoom.GetComponent<RoomType>().coord = tempCoord;
                    bfsQueue.Enqueue(tempRoom);
                }
                else
                {
                    GameObject adjRoom = map[tempCoord];
                    RoomType adjRoomType = adjRoom.GetComponent<RoomType>();

                    if (!adjRoomType.Top)
                    {
                        adjRoom = doorsToRoomMap[adjRoomType.doors + new Vector4(1, 0, 0, 0)];
                        adjRoom.GetComponent<RoomType>().coord = adjRoomType.coord;
                    }
                }
            }

            if (currRoom.GetComponent<RoomType>().Left)
            {
                Vector2 tempCoord = currRoom.GetComponent<RoomType>().coord + Vector2.left;
                if (!map.ContainsKey(tempCoord))
                {
                    GameObject tempRoom = rooms.rightRooms[Random.Range(0, rooms.rightRooms.Count)];
                    tempRoom.GetComponent<RoomType>().coord = tempCoord;
                    bfsQueue.Enqueue(tempRoom);
                }
                else
                {
                    GameObject adjRoom = map[tempCoord];
                    RoomType adjRoomType = adjRoom.GetComponent<RoomType>();

                    if (!adjRoomType.Right)
                    {
                        adjRoom = doorsToRoomMap[adjRoomType.doors + new Vector4(0, 1, 0, 0)];
                        adjRoom.GetComponent<RoomType>().coord = adjRoomType.coord;
                    }
                }
            }
        }

        if(size != 15)
        {
            validMap = false;
        }

        return validMap;
    }


}


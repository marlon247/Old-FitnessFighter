using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

//using rooms = GameManager.SharedInstance.rooms;

public class CreateMap : MonoBehaviour
{
    private Dictionary<Vector2, GameObject> map;
    private GameManager gameManager;
    //private RoomsCollections rooms = GameManager.SharedInstance.rooms;
    private Queue<GameObject> bfsQueue;
    private GameObject startRoom;
    private GameObject endRoom;
    private int size = 0;

    void Start()
    {
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
                Vector2 tempCoord = currRoom.GetComponent<RoomType>().coord + new Vector2(0, 1);
                if (!map.ContainsKey(tempCoord))
                {
                    GameObject tempRoom = rooms.bottomRooms[Random.Range(0, rooms.bottomRooms.Count)];
                    tempRoom.GetComponent<RoomType>().coord = tempCoord;
                    bfsQueue.Enqueue(tempRoom);
                }
            }

            if (currRoom.GetComponent<RoomType>().Right)
            {
                Vector2 tempCoord = currRoom.GetComponent<RoomType>().coord + new Vector2(1, 0);
                if (!map.ContainsKey(tempCoord))
                {
                    GameObject tempRoom = rooms.leftRooms[Random.Range(0, rooms.leftRooms.Count)];
                    tempRoom.GetComponent<RoomType>().coord = tempCoord;
                    bfsQueue.Enqueue(tempRoom);
                }
            }

            if (currRoom.GetComponent<RoomType>().Bottom)
            {
                Vector2 tempCoord = currRoom.GetComponent<RoomType>().coord + new Vector2(0, -1);
                if (!map.ContainsKey(tempCoord))
                {
                    GameObject tempRoom = rooms.topRooms[Random.Range(0, rooms.topRooms.Count)];
                    tempRoom.GetComponent<RoomType>().coord = tempCoord;
                    bfsQueue.Enqueue(tempRoom);
                }
            }

            if (currRoom.GetComponent<RoomType>().Left)
            {
                Vector2 tempCoord = currRoom.GetComponent<RoomType>().coord + new Vector2(-1, 0);
                if (!map.ContainsKey(tempCoord))
                {
                    GameObject tempRoom = rooms.rightRooms[Random.Range(0, rooms.rightRooms.Count)];
                    tempRoom.GetComponent<RoomType>().coord = tempCoord;
                    bfsQueue.Enqueue(tempRoom);
                }
            }
        }

        //TODO: if queue still has stuff after reaching max size remove doors by checking the area around
        //      open doors if one side is open and not the other.
    }


}


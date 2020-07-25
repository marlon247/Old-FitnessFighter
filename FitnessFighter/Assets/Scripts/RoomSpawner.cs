using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour {

    [Tooltip("1 = Right, 2 = Left, 3 = Bottom, 4 = Top")]
    public int roomID;

    private Rooms rooms;
    private GameObject newRoom;
    private void Start()
    {
        rooms = GameManager.SharedInstance.rooms;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(GameManager.SharedInstance.roomsPassed >= GameManager.SharedInstance.amountOFRoomsInDungeon)
            {
                newRoom = FinalRoomToSpawn();
            }
            else if(roomID == 1)
            {//spawning a room with left door
                int rand = Random.Range(0, rooms.leftRooms.Count);
                newRoom= Instantiate(rooms.leftRooms[rand]);
            }
            else if(roomID == 2)
            {//spawning a room with right door
                int rand = Random.Range(0, rooms.rightRooms.Count);
                newRoom = Instantiate(rooms.rightRooms[rand]);
            }
            else if(roomID == 3)
            {//spawning a room with top door
                int rand = Random.Range(0, rooms.topRooms.Count);
                newRoom= Instantiate(rooms.topRooms[rand]);
            }
            else if(roomID == 4)
            {//spawning a room with bottom door
                int rand = Random.Range(0, rooms.bottomRooms.Count);
                newRoom = Instantiate(rooms.bottomRooms[rand]);
            }
            GameManager.SharedInstance.roomsPassed += 1;
            GameManager.SharedInstance.roomsEntered.Add(newRoom);
            GameManager.SharedInstance.DeactivatePrevRoom();
        }
    }

    GameObject FinalRoomToSpawn()
    {
        GameObject go = new GameObject();
        if (roomID == 1)
        {
            //spawning a room with left door
            go = Instantiate(rooms.leftRooms[0]);
        }
        else if (roomID == 2)
        {
            go = Instantiate(rooms.rightRooms[0]);
        }
        else if (roomID == 3)
        {
            go = Instantiate(rooms.topRooms[0]);
        }
        else if (roomID == 4)
        {
            go = Instantiate(rooms.bottomRooms[0]);
        }
        return go;
    }

}

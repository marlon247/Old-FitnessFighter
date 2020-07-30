using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnNewRoom : MonoBehaviour
{
    
    public CreateMap map;
    public void SpawnRoom()
    {
        foreach (GameObject go in map.rooms)
        {
            Destroy(go);
        }
        map.rooms.Clear();
        GameManager.SharedInstance.GenerateSeed();

        map.GenerateMap();
    }
}

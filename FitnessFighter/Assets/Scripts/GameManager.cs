using System.Collections;
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

    public Material[] roomMats;

    private CreateMap mapCreator;

    [SerializeField]
    GameObject player;

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

        GenerateSeed();

        mapCreator = GetComponent<CreateMap>();

    }


    public void GenerateSeed()
    {
        if (useStringSeed)
        {
            seed = stringSeed.GetHashCode();
        }

        if (randomizeSeed)
        {
            seed = Random.Range(0, 99999);
        }

        Random.InitState(seed);

        mapCreator = GetComponent<CreateMap>();

    }


    private void SpawnPlayer()
    {
        Instantiate(player, new Vector3(0, 0, 0), Quaternion.identity);
    }


    private void Start()
    {
        mapCreator.GenerateMap();
        SpawnPlayer();
    }

}

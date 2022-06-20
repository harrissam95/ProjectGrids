using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    //important for storing and accessing info
    public static MainManager Instance;

    public Vector3 playerPosOverworld, doorSpawnPos;
    public Quaternion playerRotOverworld, doorSpawnRot;
    public bool doorSpawnDecision;
    public int lastScene;

    private void Awake()
    {
        //if there's already an instance running, kill it
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void enhance()
    {
        Debug.Log(":)");
    }
}
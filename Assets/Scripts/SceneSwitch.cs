using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//necessary for scene switching
using UnityEngine.SceneManagement;

/*
 * Author: Sam Harris
 * Goal: Handle the switching of scenes and storing the OverWorld (Scene 0) position
 * as necessary when leaving with the intent of returning at the same place. The OW 
 * position is stored within the MainManager singleton. If returning to OW at a door
 * then store the location of the door's SpawnPoint child and set the player there.
 */

public class SceneSwitch : MonoBehaviour
{

    //start
    private void Start()
    {
        setPlayerLocation();
    }//end Start

    //switch scenes on trigger depending on what the collision is with
    private void OnTriggerEnter(Collider other)
    {
        //get an int value of current scene
        int sceneId = SceneManager.GetActiveScene().buildIndex;

        //switch based on tag of collided object
        switch ((other.gameObject.tag.ToLower(), sceneId))
        {
            case ("grass", 0):
                storeInfo(sceneId, false, other);
                SceneManager.LoadScene(1);
                break;
            case ("door", 0):
                storeInfo(sceneId, true, other);
                SceneManager.LoadScene(2);
                break;
            case ("door", 2):
                MainManager.Instance.doorSpawnDecision = true;
                SceneManager.LoadScene(0);
                break;
            default:
                Debug.Log("SceneSwitch switch case defaulted");
                break;
        }

    }//end onTriggerEnter

    //get and store info like current position, current rotation, and current scene before loading a new one
    private void storeInfo(int scene, bool isDoor, Collider collider)
    {
        //decide on whether or not to keep isDoor or replace with just if you pass
        //a collider or not to check if we should look for spawn point or not
        if (isDoor)
        {
            //get and store child SpawnPoint of object/door's position/rotation
            MainManager.Instance.doorSpawnPos = collider.transform.Find("SpawnPoint").gameObject.transform.position;
            MainManager.Instance.doorSpawnRot = collider.transform.Find("SpawnPoint").gameObject.transform.rotation;
        }
        else
        {
            //store all available info into singleton
            MainManager.Instance.playerPosOverworld = transform.localPosition;
            MainManager.Instance.playerRotOverworld = transform.localRotation;
            MainManager.Instance.lastScene = scene;
        }

    }//end storeInfo

    //sets the player's location based on what's needed
    private void setPlayerLocation()
    {
        if (MainManager.Instance != null)
        {
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                GetComponent<CharacterController>().enabled = false;

                if (MainManager.Instance.doorSpawnDecision)
                {
                    transform.position = MainManager.Instance.doorSpawnPos;
                    transform.rotation = MainManager.Instance.doorSpawnRot;
                    MainManager.Instance.doorSpawnDecision = false;
                }
                else
                {
                    transform.position = MainManager.Instance.playerPosOverworld;
                    transform.rotation = MainManager.Instance.playerRotOverworld;
                }

                GetComponent<CharacterController>().enabled = true;
            }
        }
    }//end setPlayerLocation

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//necessary for scene switching
using UnityEngine.SceneManagement;

/* Author: Sam Harris
 * Goal: Handle the switching of scenes and storing the OverWorld (Scene 0) position
 * as necessary when leaving with the intent of returning at the same place. The OW 
 * position is stored within the MainManager singleton. If returning to OW at a door
 * then store the location of the door's SpawnPoint child and set the player there.
 */

public class SceneSwitch : MonoBehaviour
{
    private int sceneId;
    private string colliderTag;
    
    private void Start()
    {
        //get index value of currently loaded scene
        sceneId = SceneManager.GetActiveScene().buildIndex;
        setPlayerLocation();
    }

    //switch scenes on trigger depending on what the collision is with
    private void OnTriggerEnter(Collider other)
    {
        colliderTag = other.gameObject.tag.ToLower();

        switch ((colliderTag, sceneId))
        {
            case ("grass", 0):
                storeInfo(other);
                SceneManager.LoadScene(1);
                break;
            case ("door", 0):
                storeInfo(other);
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

    }

    //get and store info like current position, current rotation, and current scene before loading a new one
    private void storeInfo(Collider collider)
    {
        if (colliderTag == "door")
        {
            //get and store child SpawnPoint of object/door's position/rotation
            MainManager.Instance.doorSpawnPos = collider.transform.Find("SpawnPoint").gameObject.transform.position;
            MainManager.Instance.doorSpawnRot = collider.transform.Find("SpawnPoint").gameObject.transform.rotation;
        }
        else
        {
            MainManager.Instance.playerPosOverworld = transform.localPosition;
            MainManager.Instance.playerRotOverworld = transform.localRotation;
        }

        MainManager.Instance.lastScene = sceneId;
    }

    //sets the player's location based on what's needed
    private void setPlayerLocation()
    {
        if (MainManager.Instance != null)
        {
            if (sceneId == 0)
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
    }

}
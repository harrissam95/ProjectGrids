using UnityEngine;
using System.Collections.Generic;

public class TurnController : MonoBehaviour
{
    public List<GameObject> turnOrder;
    public GameObject mainCharacter;

    private int refreshFlag = 0;

    private void Start()
    {
        InitializeTurnOrder();
        Debug.Log(turnOrder);
    }

    private void Update()
    {
        if(refreshFlag == 1)
        {
            RefreshTurnOrder();
        }
    }

    private void RefreshTurnOrder()
    {
        turnOrder.Clear();
        InitializeTurnOrder();
        refreshFlag = 0;
    }

    private void InitializeTurnOrder()
    {
        GameObject[] characters;
        characters = GameObject.FindGameObjectsWithTag("Character");
        
        foreach(GameObject character in characters)
        {
            turnOrder.Add(character);
            Debug.Log(character.GetComponent<CombatStats>().characterName);
        }

        SortList();
        //https://answers.unity.com/questions/677070/sorting-a-list-linq.html
        //https://answers.unity.com/questions/1620539/how-do-i-move-a-list-object-at-the-end-of-the-list.html
    }

    private void SortList()
    {
        turnOrder.Sort(SortBySpeed);
    }

    private int SortBySpeed(GameObject char1, GameObject char2)
    {
        return char1.GetComponent<CombatStats>().speed.CompareTo(char2.GetComponent<CombatStats>().speed);
    }
}
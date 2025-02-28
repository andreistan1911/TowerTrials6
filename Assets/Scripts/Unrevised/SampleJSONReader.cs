using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleJSONReader : MonoBehaviour
{
    /*
    SAMPLE READER
    
    USE ONLY AS AN EXAMPLE

    SAMPLE JSON:
    {
        "player": [
            {
                "name": "Warrior",
                "health": 100,
                "mana": 20
            },
            {
                "name": "Wizard",
                "health": 40,
                "mana": 80
            },
            {
                "name": "Cleric",
                "health": 80,
                "mana": 50
            },
        ]
    }
    */

    public TextAsset textJSON;

    [System.Serializable]
    public class Player
    {
        public string name;
        public int health;
        public int mana;
    }

    [System.Serializable]
    public class PlayerList
    {
        public Player[] player;
    }


    public PlayerList myPlayerList = new();

    void Start()
    {
        myPlayerList = JsonUtility.FromJson<PlayerList>(textJSON.text);

    }

    void Update()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JSONReader : MonoBehaviour
{
    private TextAsset enemyJSON;
    private TextAsset reactionJSON;

    [System.Serializable]
    public class EnemyParsed
    {
        public string type;
        public float health;
        public float speed;
    }

    [System.Serializable]
    public class EnemyParsedList
    {
        public EnemyParsed[] enemy;
    }

    [System.Serializable]
    public class ReactionParsed
    {
        public string name;
        public string displayName;
        public float damage;
        public float slowValue; // Must be in [0, 1]
        public float slowDuration;
        public string buff; // must be handled
    }

    [System.Serializable]
    public class ReactionParsedList
    {
        public ReactionParsed[] reaction;
    }

    [HideInInspector]
    public EnemyParsedList enemyParsedList = new();
    [HideInInspector]
    public ReactionParsedList reactionParsedList = new();

    private void Awake()
    {
        enemyJSON = Resources.Load<TextAsset>("JSON/Enemies");
        reactionJSON = Resources.Load<TextAsset>("JSON/Reactions");

        ReadEnemies();
        ReadReactions();
    }

    private Global.EnemyType ParseType(string type)
    {
        if (type == "Slime")
            return Global.EnemyType.Slime;
        if (type == "Goblin")
            return Global.EnemyType.Goblin;
        if (type == "Wolf")
            return Global.EnemyType.Wolf;
        if (type == "Dragon")
            return Global.EnemyType.Dragon;
        if (type == "Skeleton")
            return Global.EnemyType.Skeleton;
        if (type == "Viking")
            return Global.EnemyType.Viking;
        if (type == "Giant")
            return Global.EnemyType.Giant;
        if (type == "Demon")
            return Global.EnemyType.Demon;

        return Global.EnemyType.Slime; // default value
    }

    private void ReadEnemies()
    {
        enemyParsedList = JsonUtility.FromJson<EnemyParsedList>(enemyJSON.text);

        for (int i = 0; i < enemyParsedList.enemy.Length; ++i)
        {
            EnemyParsed currentEnemy = enemyParsedList.enemy[i];
            EnemyStats enemyStats = new(currentEnemy.health, currentEnemy.speed);

            Global.enemyValues.Add(ParseType(currentEnemy.type), enemyStats);
        }
    }

    private void ReadReactions()
    {
        reactionParsedList = JsonUtility.FromJson<ReactionParsedList>(reactionJSON.text);

        for (int i = 0; i < reactionParsedList.reaction.Length; ++i)
        {
            ReactionParsed currentReaction = reactionParsedList.reaction[i];
            Global.Element firstElement = GetElementByLetter(currentReaction.name[0]);
            Global.Element secondElement = GetElementByLetter(currentReaction.name[1]);

            ReactionStats reactionStats = new(currentReaction.displayName, currentReaction.damage, currentReaction.slowValue, currentReaction.slowDuration, currentReaction.buff);

            // Add entry to first element -> values[Fire][Water]
            if (Global.reactionValues.ContainsKey(firstElement))
                Global.reactionValues[firstElement].Add(secondElement, reactionStats);
            else
                Global.reactionValues.Add(firstElement, new Dictionary<Global.Element, ReactionStats>() { { secondElement, reactionStats } });

            // Add entry to second element -> values[Water][Fire]
            if (Global.reactionValues.ContainsKey(secondElement))
                Global.reactionValues[secondElement].Add(firstElement, reactionStats);
            else
                Global.reactionValues.Add(secondElement, new Dictionary<Global.Element, ReactionStats>() { { firstElement, reactionStats } });
        }
    }

    private Global.Element GetElementByLetter(char letter)
    {
        Global.Element element = Global.Element.None;

        switch (letter)
        {
            case 'F':
                element = Global.Element.Fire;
                break;
            case 'L':
                element = Global.Element.Lightning;
                break;
            case 'W':
                element = Global.Element.Water;
                break;
            default:
                Debug.LogError("Undefined element!");
                break;
        }

        return element;
    }
}


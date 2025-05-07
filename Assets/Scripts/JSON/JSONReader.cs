using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JSONReader : MonoBehaviour
{
    private TextAsset enemyJSON;
    private TextAsset reactionJSON;
    private TextAsset towerJSON;

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
    public class TowerParsed
    {
        public string type;
        public string element;
        public float damage;
        public float attackRate;
        public float range;
        public int cost;
    }

    [System.Serializable]
    public class TowerParsedList
    {
        public TowerParsed[] tower;
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
    [HideInInspector]
    public TowerParsedList towerParsedList = new();

    private void Awake()
    {
        if (Global.isDataLoaded)
            return;

        enemyJSON = Resources.Load<TextAsset>("JSON/Enemies");
        reactionJSON = Resources.Load<TextAsset>("JSON/Reactions");
        towerJSON = Resources.Load<TextAsset>("JSON/Towers");

        ReadEnemies();
        ReadReactions();
        ReadTowers();

        Global.isDataLoaded = true;
    }
    private void ReadEnemies()
    {
        enemyParsedList = JsonUtility.FromJson<EnemyParsedList>(enemyJSON.text);

        for (int i = 0; i < enemyParsedList.enemy.Length; ++i)
        {
            EnemyParsed currentEnemy = enemyParsedList.enemy[i];
            EnemyStats enemyStats = new(currentEnemy.health, currentEnemy.speed);

            Global.enemyValues.Add(ParseEnemyType(currentEnemy.type), enemyStats);
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

    private void ReadTowers()
    {
        towerParsedList = JsonUtility.FromJson<TowerParsedList>(towerJSON.text);

        float maxDmg = 0, maxAtkSpd = 0, maxRange = 0;

        for (int i = 0; i < towerParsedList.tower.Length; ++i)
        {
            TowerParsed currentTower = towerParsedList.tower[i];
            TowerStats towerStats = new(currentTower.damage, currentTower.attackRate, currentTower.range, currentTower.cost);

            maxDmg = currentTower.damage > maxDmg ? currentTower.damage : maxDmg;
            maxAtkSpd = (1.0f / currentTower.attackRate) > maxAtkSpd ? (1.0f / currentTower.attackRate) : maxAtkSpd;
            maxRange = currentTower.range > maxRange ? currentTower.range : maxRange;

            Global.Element element = Global.GetElementFromString(currentTower.element);
            Global.TowerType type = Global.GetTowerTypeFromString(currentTower.type);

            if (Global.towerValues.ContainsKey(element))
                Global.towerValues[element].Add(type, towerStats);
            else
                Global.towerValues.Add(element, new Dictionary<Global.TowerType, TowerStats>() { { type, towerStats } });
        }

        Global.ComputeTowerMaxValues(maxDmg, maxAtkSpd, maxRange);
    }

    private Global.EnemyType ParseEnemyType(string type)
    {
        return type switch 
        { 
            "Slime"      => Global.EnemyType.Slime,
            "Goblin"     => Global.EnemyType.Goblin,
            "Wolf"       => Global.EnemyType.Wolf,
            "Dragon"     => Global.EnemyType.Dragon,
            "Skeleton"   => Global.EnemyType.Skeleton,
            "Viking"     => Global.EnemyType.Viking,
            "Giant"      => Global.EnemyType.Giant,
            "Demon"      => Global.EnemyType.Demon,
            "DragonMama" => Global.EnemyType.DragonMama,
            "Wizard"     => Global.EnemyType.Wizard,
            _ => Global.EnemyType.Slime
        };
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


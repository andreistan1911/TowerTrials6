using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Global : ScriptableObject
{
    public static bool isDataLoaded = false;

    public static Dictionary<EnemyType, EnemyStats> enemyValues = new();
    public static Dictionary<Element, Dictionary<Element, ReactionStats>> reactionValues = new();
    public static Dictionary<Element, Dictionary<TowerType, TowerStats>> towerValues = new();

    public const float INFLICT_STATUS_DURATION = 1.0f;
    public const float REACTION_COOLDOWN = 1.0f;
    public const int DPS_TICKS = 4;
    public const int MAX_WAVES = 7;
    public static float dpsCooldown;

    public static float g = 9.81f;
    public static float eps = 0.001f;

    public const float VIEW_ANGLE = 10f;

    public static float maxTowerDamage = 0;
    public static float maxTowerAtkSpd = 0;
    public static float maxTowerRange  = 0;

    public enum WinCode
    {
        Nothing,
        FinishedTutorial
    }

    public enum Element
    {
        None,
        Fire,
        Lightning,
        Water,
    }

    public enum TowerType
    {
        Single,
        Laser,
        Bomb,
        Aoe
    }

    public enum EnemyType
    {
        Slime,
        Wolf,
        Goblin,
        Dragon,
        Skeleton,
        Viking,
        Demon,
        Giant,
        DragonMama,
        Wizard
    }

    public enum Level
    {
        Tutorial,
        Level_1
    }

    private void Awake()
    {
        dpsCooldown = ComputeDpsCooldown();
    }

    private static float ComputeDpsCooldown()
    {
        return 1.0f / DPS_TICKS;
    }

    internal static void HandleWin(WinCode code)
    {
        switch (code)
        {
            case WinCode.FinishedTutorial:
                PlayerPrefs.SetInt("EndlessUnlocked", 1);
                SceneManager.LoadScene("Win Tutorial");
                break;
            default:
                break;
        }
    }

    public static Element GetElementFromString(string element)
    {
        return element switch
        {
            "None"      => Element.None,
            "Fire"      => Element.Fire,
            "Lightning" => Element.Lightning,
            "Water"     => Element.Water,
            _ => Element.None
        };
    }

    public static TowerType GetTowerTypeFromString(string type)
    {
        return type switch
        {
            "Single" => TowerType.Single,
            "Bomb"   => TowerType.Bomb,
            "Aoe"    => TowerType.Aoe,
            "Laser"  => TowerType.Laser,
            _ => TowerType.Single
        };
    }

    public static void ComputeTowerMaxValues(float maxDamage, float maxAtkSpd, float maxRange)
    {
        maxTowerDamage = maxDamage;
        maxTowerAtkSpd = maxAtkSpd;
        maxTowerRange  = maxRange;
    }

    public static void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public static void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}

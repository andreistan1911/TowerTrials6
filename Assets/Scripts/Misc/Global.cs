using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : ScriptableObject
{
    public static Dictionary<EnemyType, EnemyStats> enemyValues = new();
    public static Dictionary<Element, Dictionary<Element, ReactionStats>> reactionValues = new();

    public const float INFLICT_STATUS_DURATION = 1.0f;
    public const float REACTION_COOLDOWN = 1.0f;
    public const int DPS_TICKS = 4;
    public const int MAX_WAVES = 7;
    public static float dpsCooldown;

    public static float g = 9.81f;
    public static float eps = 0.001f;

    public const float VIEW_ANGLE = 10f;

    public enum Element
    {
        None,
        Fire,
        Lightning,
        Water,
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
        Giant
    }

    private void Awake()
    {
        dpsCooldown = ComputeDpsCooldown();
    }

    private static float ComputeDpsCooldown()
    {
        return 1.0f / DPS_TICKS;
    }
}

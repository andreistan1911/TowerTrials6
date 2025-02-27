[System.Serializable]
public class ReactionStats
{
    public string displayName;
    public float damage;
    public float slowValue; // Must be in [0, 1]
    public float slowDuration;
    public string buff; // must be handled

    public ReactionStats(string displayName, float damage, float slowValue, float slowDuration, string buff)
    {
        this.displayName = displayName;
        this.damage = damage;
        this.slowValue = slowValue;
        this.slowDuration = slowDuration;
        this.buff = buff;
    }
}

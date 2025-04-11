using UnityEngine;

[System.Serializable]
public class EnemySpawnData
{
    public Global.EnemyType type;
    public Global.Element element;

    public EnemySpawnData(Global.EnemyType type, Global.Element element)
    {
        this.type = type;
        this.element = element;
    }

    public bool HasElement()
    {
        if (element == Global.Element.None)
            return false;

        return true;
    }

    public int GetCost()
    {
        return AbstractWaveManger.baseCosts[type] + (HasElement() ? 1 : 0);
    }
}
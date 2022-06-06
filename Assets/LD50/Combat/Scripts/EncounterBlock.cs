using UnityEngine;

[System.Serializable]
public class EncounterBlock
{
    [System.Serializable]
    public struct EncounterBlockItem
    {
        public Encounter encounter;
        [Range(1,5)] public int weight;
    }

    [SerializeField] private EncounterBlockItem[] items = new EncounterBlockItem[0];

    public Encounter GetEncounter()
    {
        int total = 0;
        foreach(var item in items)
            total += item.weight;

        int roll = Random.Range(0,total+1);

        int current = 0;
        foreach(var item in items)
        {
            current += item.weight;
            if(current < roll) continue;
            return item.encounter;
        }

        return items[0].encounter;
    }
}
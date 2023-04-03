using LGrid;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tile Search Profile", fileName = "TileSearchProfile")]
public class TileSearchProfile : SerializedScriptableObject
{
    public HashSet<string> blackList;
    public HashSet<string> whiteList;
    public bool Check(Position position, HashSet<string> wildcards = null)
    {
        var tags = GridManager.Instance[position];
        var w = wildcards != null && wildcards.Overlaps(tags);
        var b1 = blackList == null || blackList.Count == 0 || tags == null || !tags.Overlaps(blackList);
        var b2 = whiteList == null || whiteList.Count == 0 || tags != null && tags.IsSupersetOf(whiteList);
        return w || b1 && b2;
    }
}

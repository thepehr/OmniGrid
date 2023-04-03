using System.Collections;
using System.Collections.Generic;
using LGrid;
using Sirenix.OdinInspector;
using UnityEditor;
//using UnityEditor;
using UnityEngine;

public class FlowField : SerializedMonoBehaviour
{
    public Position position;
    public HashSet<string> blackList;
    public HashSet<string> whiteList;
    public HashSet<string> wildcard;
    public int maxDepth;

    //public HashSet<Position> seen = new HashSet<Position>();
    public List<Position> buffer = new List<Position>();

    public Dictionary<Position, float> height = new Dictionary<Position, float>();

    public void CheckTile(Position position, float depth)
    {
        if (depth > maxDepth)
            return;
        var tags = GridManager.Instance[position];
        var b1 = blackList == null || blackList.Count == 0 || tags == null || !tags.Overlaps(blackList);
        var b2 = whiteList == null || whiteList.Count == 0 || tags != null && tags.IsSupersetOf(whiteList);
        var w = tags != null && tags.Overlaps(wildcard);
        /*
        if (!height.ContainsKey(position) && (b1 && b2 || w))
        {
            height.Add(position, depth);
            buffer.Add(position);
        }
        */
        if ((b1 && b2 || w))
        {
            if (!height.ContainsKey(position))
            {
                height.Add(position, depth);
                buffer.Add(position);
            }
            else if (height[position] > depth)
            {   
                height[position] = depth;
                buffer.Add(position);
            }
        }
    }

    [Button("Refresh")]
    public void Refresh()
    {
        var f = Time.realtimeSinceStartup;
        height.Clear();
        buffer.Clear();
        CheckTile(position, 0);
        while (buffer.Count > 0)
        {
            var next = buffer[0];
            buffer.RemoveAt(0);
            var depth = height[next];
            foreach (var item in next.GetAllNeighbors())
            {
                CheckTile(item, depth + (next - item).GetWorldPosition().magnitude);
            }
            /*
            CheckTile(next + new Position(1, 0), depth + 1);
            CheckTile(next + new Position(-1, 0), depth + 1);
            CheckTile(next + new Position(0, 1), depth + 1);
            CheckTile(next + new Position(0, -1), depth + 1);
            */
        }
        Debug.Log("duration: " + (Time.realtimeSinceStartup - f).ToString());
    }

    public Position _GetNextPosition(Position currentPos){
        var n = _GetNextPosition(currentPos);
        if (height[n] < height[currentPos])
            return n;
        var n2 = _GetNextPosition(n);
        if (n2 != currentPos){
            return n;
        }
        return currentPos;
    }

    public Position GetNextPosition(Position currentPos, int radius = 0)
    {
        var minValue = float.MaxValue;
        var nextPos = currentPos;
        foreach (var item in currentPos.GetAllNeighbors())
        {
            var pos = item;
            var extra = GridManager.Instance.HasTag(pos, "oc") ? 100 : 0;
            
            if (pos != currentPos &&
                height.ContainsKey(pos) &&
                height[pos] + extra <= minValue && height[pos] + extra <= height[currentPos]
            )
            {
                minValue = height[pos];
                nextPos = pos;
            }
        }
        /*
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                var pos = new Position(i, j) + currentPos;
                var extra = GridManager.Instance.HasTag(pos, "oc") ? 100 : 0;
                
                if (Mathf.Abs(i) + Mathf.Abs(j) == 1 && pos != currentPos &&
                    height.ContainsKey(pos) &&
                    height[pos] + extra <= minValue && height[pos] + extra <= height[currentPos]
                )
                {
                    minValue = height[pos];
                    nextPos = pos;
                }
            }
        }
        */
        return nextPos;
    }
    public Dictionary<Position, Position> GetPath(Position source, bool reverse = false)
    {
        var d = new Dictionary<Position, Position>();
        var p = source;
        if (!height.ContainsKey(p))
            return null;
        while (p != position)
        {
            d.Add(p, GetNextPosition(p));
            p = GetNextPosition(p);
        }
        if (reverse)
        {
            var dd = new Dictionary<Position, Position>();
            foreach (var item in d.Keys)
            {
                dd.Add(d[item], item);
            }
            return dd;
        }
        return d;
    }

    public void Clear()
    {
        height.Clear();
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (height == null)
            return;
        foreach (var item in height.Keys)
        {
            Handles.Label(item.GetWorldPosition(), height[item].ToString());
        }
    }
#endif
}

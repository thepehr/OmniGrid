using System.Collections;
using System.Collections.Generic;
using LGrid;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class TagFinder : SerializedMonoBehaviour
{
    public Position position;
    public HashSet<string> blackList;
    public HashSet<string> whiteList;
    public HashSet<string> wildcard;
    public HashSet<string> walkableBlackList;
    //public string targetTag;
    public string targetTag;
    public Position targetPosition;
    public bool valid = false;
    public int searchDepth;

    //public HashSet<Position> seen = new HashSet<Position>();
    public List<Position> buffer = new List<Position>();

    public Dictionary<Position, float> height = new Dictionary<Position, float>();

    public void CheckTile(Position position, float depth)
    {
        if (depth > searchDepth)
            return;
        var tags = GridManager.Instance[position];
        var b1 = blackList == null || blackList.Count == 0 || tags == null || !tags.Overlaps(blackList);
        var b2 = whiteList == null || whiteList.Count == 0 || tags != null && tags.IsSupersetOf(whiteList);
        var w = tags != null && tags.Overlaps(wildcard);
        /*
        if (!height.ContainsKey(position) && (b1 && b2 || w || GridManager.Instance.HasTag(position, targetTag)))
        {
            if (GridManager.Instance.HasTag(position, targetTag) && !GridManager.Instance[position].Overlaps(walkableBlackList)){
                targetPosition = position;
                valid = true;
            }
            height.Add(position, depth);
            buffer.Add(position);
        }
        */
        if ((b1 && b2 || w || GridManager.Instance.HasTag(position, targetTag)))
        {
            if (GridManager.Instance.HasTag(position, targetTag) && !GridManager.Instance[position].Overlaps(walkableBlackList)){
                targetPosition = position;
                valid = true;
            }
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
        valid = false;
        height.Clear();
        buffer.Clear();
        //CheckTile(position, 0);
        height.Add(position, 0);
        buffer.Add(position);
        if (GridManager.Instance.HasTag(position, targetTag) && !GridManager.Instance[position].Overlaps(walkableBlackList)){
            targetPosition = position;
            valid = true;
        }
        while (buffer.Count > 0 && !valid)
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
        //Debug.Log("duration: " + (Time.realtimeSinceStartup - f).ToString());
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
        return nextPos;
    }

    public Dictionary<Position, Position> GetPath(){
        if (!valid){
            return null;
        }
        var p = new Dictionary<Position, Position>(){};
        var n = targetPosition;
        while (n != position)
        {
            //n = GetNextPosition(n);
            p.Add(GetNextPosition(n), n);
            n = GetNextPosition(n);
        }
        return p;
    }
    
    private void OnDrawGizmosSelected()
    {
        
    }
    
}

using System.Collections;
using System.Collections.Generic;
using LGrid;
using Sirenix.OdinInspector;
using UnityEditor;
using System.Linq;
using UnityEngine;

public class GuidedTagFinder : SerializedMonoBehaviour
{
    public TileSearchProfile profile;
    public TileSearchProfile pavingProfile;
    public HashSet<string> wildcards = new HashSet<string>();
    public Position origin;
    public string targetTag;
    public int maxIter;

    public Dictionary<Position, float> depths;

    [Button("Find")]
    public Position Find()
    {
        int iter = 0;
        var open = new PriorityQueue<Position>();
        var pavings = new PriorityQueue<Position>();
        depths.Clear();
        depths.Add(origin, 0);
        if (pavingProfile.Check(origin, wildcards))
        {
            pavings.Enqueue(origin, 0);
        }   
        else
            open.Enqueue(origin, 0);
        while (pavings.Count > 0 || open.Count > 0)
        {
            var next = new Position();
            bool isPaving = false;
            if (pavings.Count > 0)
            {
                next = pavings.Dequeue();
                isPaving = true;
                //pavings.Remove(next);
            }
            else
            {
                next = open.Dequeue();
                //open.Remove(next);
            }
            iter++;
            if (iter >= maxIter)
            {
                Debug.Log("Maximum number of iterations reached!");
                return origin;
            }
            bool terminal = true;
            foreach (var item in next.GetAllNeighbors())
            {
                var d = depths[next] + (next - item).GetWorldPosition().magnitude;
                if (!profile.Check(item, wildcards) || depths.ContainsKey(item))
                    continue;
                if (GridManager.Instance.HasTag(item, targetTag))
                {
                    return item;
                }
                if (pavingProfile.Check(item, wildcards))
                {
                    pavings.Enqueue(item, depths[next]);
                    depths.Add(item, depths[next]);
                    terminal = false;
                }
                else if (!isPaving)
                {
                    open.Enqueue(item, d);
                    depths.Add(item, d);
                    terminal = false;
                }
            }
            if (isPaving && terminal)
            {
                foreach (var item in next.GetAllNeighbors())
                {
                    if (profile.Check(item, wildcards) && !depths.ContainsKey(item))
                    {
                        depths.Add(item, depths[next] + (next - item).GetWorldPosition().magnitude);
                        open.Enqueue(item, depths[item]);
                    }
                }
            }
        }
        return origin;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected() {
        if (depths == null)
            return;
        foreach (var item in depths)
        {
            Handles.color = Color.white;
            Handles.DrawSolidDisc(item.Key.GetWorldPosition(), Vector3.forward, 0.2f);
        }
    }  
#endif
}


public class PositionGreedyComparer : IComparer<Position>
{
    public GuidedTagFinder finder;
    public PositionGreedyComparer(GuidedTagFinder finder)
    {
        this.finder = finder;
    }
    public int Compare(Position p1, Position p2)
    {
        if (!finder.depths.ContainsKey(p1) || !finder.depths.ContainsKey(p1))
            return 0;
        return finder.depths[p1].CompareTo(finder.depths[p2]);
    }
}
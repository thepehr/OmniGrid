using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEditor;
using System.Linq;

public class GridMapSkeleton : SerializedMonoBehaviour
{
    public TileSearchProfile profile;

    public Dictionary<Position, HashSet<Position>> regions = new Dictionary<Position, HashSet<Position>>();
    public HashSet<Position> pivots = new HashSet<Position>();
    public Dictionary<Position, HashSet<Position>> pivotsToRegions = new Dictionary<Position, HashSet<Position>>();
    public Dictionary<Position, HashSet<Position>> edges = new Dictionary<Position, HashSet<Position>>();
    public HashSet<Position> fringe = new HashSet<Position>();
    public HashSet<Position> nonFringe = new HashSet<Position>();

    public float depth;


    public void AddTileToRegion(Position tile, Position region)
    {
        if (regions.ContainsKey(tile))
        {
            regions[tile].Add(region);
        }
        else
        {
            regions.Add(tile, new HashSet<Position>(){region});
        }
        if (pivotsToRegions.ContainsKey(region))
        {
            pivotsToRegions[region].Add(tile);
        }
        else
        {
            pivotsToRegions.Add(region, new HashSet<Position>(){tile});
        }
    }

    
    public void AddRegion(Position pivot, float depth)
    {
        pivots.Add(pivot);
        var open = new List<Position>(){pivot};
        var depths = new Dictionary<Position, float>();
        depths.Add(pivot, 0);
        while (open.Count > 0)
        {
            var next = open[0];
            open.RemoveAt(0);
            bool isFringe = true;
            bool terminal = true;
            foreach (var item in next.GetAllNeighbors())
            {
                if (!profile.Check(item))
                    continue;
                var d = (item - next).GetWorldPosition().magnitude + depths[next];
                if (depths.ContainsKey(item))
                {
                    if (d < depths[item])
                    {
                        depths[item] = d;
                        nonFringe.Add(next);
                        fringe.Remove(next);
                        isFringe = false;
                        terminal = false;
                        AddTileToRegion(item, pivot);
                    }
                }
                else if (d <= depth)
                {
                    depths.Add(item, d);
                    open.Add(item);
                    isFringe = false;
                    nonFringe.Add(next);
                    fringe.Remove(next);
                    terminal = false;
                    AddTileToRegion(item, pivot);
                    if (pivots.Contains(item))
                    {
                        if (edges.ContainsKey(item))
                        {
                            edges[item].Add(pivot);
                        }
                        else
                        {
                            edges.Add(item, new HashSet<Position>(){pivot});
                        }
                        if (edges.ContainsKey(pivot))
                        {
                            edges[pivot].Add(item);
                        }
                        else
                        {
                            edges.Add(pivot, new HashSet<Position>(){item});
                        }
                    }
                }
                else
                {
                    terminal = false;
                }
            }
            if (terminal)
            {
                nonFringe.Add(next);
                fringe.Remove(next);
            }
            if (isFringe && !nonFringe.Contains(next) && !terminal)
            {
                fringe.Add(next);
            }
        }
    }

    [Button("Build")]
    public void Build()
    {   
        fringe.Clear();
        fringe.Add(new Position());
        nonFringe.Clear();
        pivots.Clear();
        edges.Clear();
        regions.Clear();
        while (fringe.Count > 0)
        {
            AddRegion(fringe.ElementAt(0), depth);
        }
    }
    

    public void Refresh(Position change)
    {
        if (regions.ContainsKey(change) && !profile.Check(change))
        {
            var pivs = new HashSet<Position>(regions[change]);
            foreach (var item in pivs)
            {
                pivotsToRegions[item].Remove(change);
            }
            regions.Remove(change);
            foreach (var pivot in pivs)
            {
                foreach (var item in pivotsToRegions[pivot])
                {
                    regions[item].Remove(pivot);
                    if (regions[item].Count == 0)
                    {
                        regions.Remove(item);
                        fringe.Add(item);
                        nonFringe.Remove(item);
                    }
                }
            }
            pivots.ExceptWith(pivs);
            foreach (var item in pivs)
            {
                var l = new HashSet<Position>(edges[item]);
                //edges.Remove(item);
                foreach (var item2 in l)
                {
                    edges[item2].Remove(item);
                    edges[item].Remove(item2);
                    //if (edges[item2].Count == 0)
                    //{
                    //    edges.Remove(item2);
                    //}
                }
            }
            foreach (var item in pivs)
            {
                if (regions.ContainsKey(item) && regions[item].Count == 0)
                {
                    fringe.Add(item);
                    nonFringe.Remove(item);
                }
            }
        }
        while (fringe.Count > 0)
        {
            AddRegion(fringe.ElementAt(0), depth);
        }
    }

    private void LateUpdate() {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Build();
        }
        if (Input.GetMouseButtonDown(0))
        {
            Refresh(Position.mouse);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected() {
        Handles.color = Color.white;
        foreach (var item in edges)
        {
            foreach (var item2 in item.Value)
            {
                Handles.DrawLine(item.Key.GetWorldPosition(), item2.GetWorldPosition());
                Handles.DrawSolidArc(item.Key.GetWorldPosition(), Vector3.forward, Vector3.left, 360, 0.2f);
            }
            
        }
        foreach (var item in fringe)
        {
            Handles.DrawSolidArc(item.GetWorldPosition(), Vector3.forward, Vector3.left, 360, 0.2f);
        }
        foreach (var item in regions.Keys)
        {
            Color cc = Color.white;
            cc.a = 0.1f;
            Handles.color = cc;
            Handles.DrawSolidArc(item.GetWorldPosition(), Vector3.forward, Vector3.left, 360, 0.1f);
            foreach (var item2 in regions[item])
            {
                Color c = Color.white;
                c.a = 0.1f;
                Handles.color = c;
                Handles.DrawLine(item.GetWorldPosition(), item2.GetWorldPosition());
            }
        }
    }
#endif
}
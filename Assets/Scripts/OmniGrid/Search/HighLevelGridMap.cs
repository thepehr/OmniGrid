using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Sirenix.OdinInspector;

public class HighLevelGridMap : SerializedMonoBehaviour
{
    public int size;
    public TileSearchProfile profile;
    public Dictionary<int, HashSet<int>> edges = new Dictionary<int, HashSet<int>>();
    public Dictionary<Position, int> regionID = new Dictionary<Position, int>();

    public Dictionary<Position, int> regions = new Dictionary<Position, int>();

    public Position GetRegion(Position position)
    {
        var res = new Position(position.x / size - (position.x < 0 && position.x % size != 0 ? 1 : 0), position.y / size - (position.y < 0 && position.y % size != 0 ? 1 : 0));
        //Debug.Log(position + " " + res);
        return res;
    }

    public void AddEdge(int p1, int p2)
    {
        if (p1 == p2)
            return;
        if (edges.ContainsKey(p1))
        {
            edges[p1].Add(p2);
        }
        else
        {
            edges.Add(p1, new HashSet<int>(){p2});
        }
        if (edges.ContainsKey(p2))
        {
            edges[p2].Add(p1);
        }
        else
        {
            edges.Add(p2, new HashSet<int>(){p1});
        }
    }

    public void Build(Position pivot)
    {
        var open = new List<Position>();
        var closed = new HashSet<Position>();
        var visited = new HashSet<Position>();
        open.Add(pivot);
        visited.Add(pivot);
        int counter = 0;
        while (open.Count > 0)
        {
            var next = open[0];
            open.RemoveAt(0);
            closed.Add(next);
            var currentRegion = GetRegion(next);
            regions.Add(next, -1);
            var adjacency = new HashSet<int>();
            foreach (var item in next.GetAllNeighbors())
            {
                var reg = GetRegion(item);
                if (profile.Check(item))
                {
                    if (!closed.Contains(item))
                    {
                        if (!visited.Contains(item))
                        {
                            open.Add(item);
                            visited.Add(item);
                        }
                    }
                    else
                    {
                        if (reg == currentRegion)
                        {
                            regions[next] = regions[item];
                        }
                        else
                        {
                            adjacency.Add(regions[item]);
                        }
                    }
                }
            }
            if (regions[next] == -1)
            {
                regions[next] = ++counter;
            }
            foreach (var item in adjacency)
            {
                if (item == regions[next])
                    continue;
                if (edges.ContainsKey(item))
                {
                    edges[item].Add(regions[next]);
                }
                else
                {
                    edges.Add(item, new HashSet<int>(){regions[next]});
                }
                if (edges.ContainsKey(regions[next]))
                {
                    edges[regions[next]].Add(item);
                }
                else
                {
                    edges.Add(regions[next], new HashSet<int>(){item});
                }
            }
        }
    }
    

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Build(new Position());
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected() {
        foreach (var item in regions)
        {
            Handles.Label(item.Key.GetWorldPosition(), item.Value.ToString());
        }
    }
#endif
}

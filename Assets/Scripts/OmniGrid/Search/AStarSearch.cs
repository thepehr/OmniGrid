using LGrid;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AStarSearch : SerializedMonoBehaviour, ISearch
{
    public float weight;
    private Position start;
    private Position dest;
    private int exp = 0;
    public int maxIter;
    private bool finished = false;

    public TileSearchProfile tileSearchProfile;

    private List<Position> open = new List<Position>();
    private Dictionary<Position, Position> track = new Dictionary<Position, Position>();

    private Dictionary<Position, float> g = new Dictionary<Position, float>();

    public Position GetClosestAvailable(Position start, Position dest)
    {
        if (IsWalkable(dest))
            return dest;
        var p = dest;
        var dist = Mathf.Infinity ;
        while (!IsWalkable(p))
        {
            foreach (var item in p.GetAllNeighbors())
            {
                if ((item - start).GetWorldPosition().magnitude < dist)
                {
                    p = item;
                    dist = (item - start).GetWorldPosition().magnitude;
                }
            }
        }
        return p;
    }

    public bool IsWalkable(Position p)
    {
        return tileSearchProfile.Check(p);// && WorldGenerator.Instance.tiles.ContainsKey(p);
    }

    public AStarSearch(float weight)
    {
        this.weight = weight;
        
    }

    public Dictionary<Position, Position> GetPath()
    {
        if (dest == start)
            return null;
        var res = new Dictionary<Position, Position>();
        var head = dest;
        while (true)
        {
            res.Add(track[head], head);
            if (track[head] == start)
                return res;
            head = track[head];
        }
    }

    public Dictionary<Position, Position> Find(Position start, Position dest)
    {
        if (!IsWalkable(dest))
            return null;
        var limit = maxIter;
        exp = 0;
        this.start = start;
        this.dest = dest;
        open = new List<Position>();
        track = new Dictionary<Position, Position>();
        g = new Dictionary<Position, float>();
        open.Add(start);
        g.Add(start, 0);
        while (limit-- > 0)
        {
            if (open.Count == 0)
            {
                return null;
            }
            open.Sort(new PositionComparer(this));
            var top = open[0];
            if (top == dest)
            {
                return GetPath();
            }
            open.RemoveAt(0);
            foreach (var item in top.GetAllNeighbors())
            {
                exp++;
                var gg = g[top] + (item - top).GetWorldPosition().magnitude;
                if (IsWalkable(item))
                {
                    if (gg < G(item))
                    {
                        if (g.ContainsKey(item))
                        {
                            g[item] = gg;
                        }
                        else
                        {
                            g.Add(item, gg);
                        }
                        if (!open.Contains(item))
                        {
                            open.Add(item);
                        }
                        if (track.ContainsKey(item))
                        {
                            track[item] = top;
                        }
                        else
                        {
                            track.Add(item, top);
                        }
                    }
                }
            }
        }
        return null;
    }

    public void Tick()
    {
        
        if (open.Count == 0)
        {
            return;
        }
        
        
        open.Sort(new PositionComparer(this));
        var top = open[0];
        if (top == dest)
        {
            if (!finished)
            {
                finished = true;
                Debug.Log(exp);
            }
            return;
        }
        open.RemoveAt(0);
        foreach (var item in top.GetNeighbors())
        {
            var gg = g[top] + 1f;
            if (IsWalkable(item))
            {
                if (gg < G(item))
                {
                    if (g.ContainsKey(item))
                    {
                        g[item] = gg;
                    }
                    else
                    {
                        g.Add(item, gg);
                    }
                    if (!open.Contains(item))
                    {
                        open.Add(item);
                    }
                }
            }
        }
    }

    public float F(Position position)
    {
        return G(position) + weight * H(position);
    }

    public float G(Position position)
    {
        return g.ContainsKey(position) ? g[position] : 999999f;
    }

    public float H(Position position)
    {
        return (position - dest).GetWorldPosition().magnitude;
    }
}

public class PositionComparer : IComparer<Position>
{
    private AStarSearch ass;
    public PositionComparer(AStarSearch ass)
    {
        this.ass = ass;
    }
    public int Compare(Position x, Position y)
    {
        return ass.F(x) - ass.F(y) < 0 ? -1 : 1;
    }
}
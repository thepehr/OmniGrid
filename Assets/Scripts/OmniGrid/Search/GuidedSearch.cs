using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class GuidedSearch : SerializedMonoBehaviour
{
    public TileSearchProfile profile;
    public TileSearchProfile pavingProfile;
    public Position dest;
    public Position start;
    public HashSet<string> wildCards = new HashSet<string>();
    public int pavingIncentive;
    public int maxIter;
    public Dictionary<Position, float> depths = new Dictionary<Position, float>();
    public Dictionary<Position, Position> path = new Dictionary<Position, Position>();

    public float OctileDistance(Position p1, Position p2)
    {
        var d1 = Mathf.Abs(p1.x - p2.x);
        var d2 = Mathf.Abs(p1.y - p2.y);
        return Mathf.Min(d1, d2) * Mathf.Sqrt(2) + Mathf.Max(d1, d2) - Mathf.Min(d1, d2);
    }


    [Button("Find")]
    public Dictionary<Position, Position> Find()
    {
        var open = new PriorityQueue<Position>();
        var parents = new Dictionary<Position, Position>();
        //var depths = new Dictionary<Position, float>();
        depths = new Dictionary<Position, float>();
        path = new Dictionary<Position, Position>();
        var iter = 0;

        open.Enqueue(start, OctileDistance(start, dest));
        depths.Add(start, 0);
        while (open.Count > 0 && iter++ < maxIter)
        {
            var next = open.Dequeue();
            if (next == dest)
            {
                //path.Clear();
                var current = dest;
                while (parents.ContainsKey(current))
                {
                    path.Add(parents[current], current);
                    current = parents[current];
                }
                return null;
            }
            foreach (var item in next.GetAllNeighbors())
            {
                if (!profile.Check(item, wildCards))
                    continue;
                var d = depths[next] + (next - item).GetWorldPosition().magnitude;
                if (!depths.ContainsKey(item))
                {
                    var incentive = 0;
                    if (pavingProfile.Check(item, wildCards))
                    {
                        incentive = pavingIncentive;
                    }
                    depths.Add(item, d);
                    parents.Add(item, next);
                    open.Enqueue(item, OctileDistance(item, dest) - incentive);
                }
                else if (depths[item] > d)
                {
                    depths[item] = d;
                    parents[item] = next;
                }
            }
        }
        return null;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected() {
        if (depths == null)
            return;
        foreach (var item in depths)
        {
            Handles.color = Color.green;
            Handles.DrawSolidDisc(item.Key.GetWorldPosition(), Vector3.forward, 0.1f);
        }
        if (path != null)
        {
            foreach (var item in path)
            {
                Handles.color = Color.white;
                Handles.DrawLine(item.Key.GetWorldPosition(), item.Value.GetWorldPosition());
            }
        }
    }
#endif
}

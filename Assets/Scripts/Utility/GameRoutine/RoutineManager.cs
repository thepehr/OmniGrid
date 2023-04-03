using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoutineManager : MonoBehaviour
{
    #region singleton
	private static RoutineManager _instance;
	public static RoutineManager Instance { get { return _instance ? _instance : (_instance = FindObjectOfType<RoutineManager>()); } }
	#endregion
    public List<IGameRoutine> routines = new List<IGameRoutine>();
    private List<IGameRoutine> inQueue = new List<IGameRoutine>();
    private List<IGameRoutine> outQueue = new List<IGameRoutine>();

    public void StartRoutine(IGameRoutine routine){
        inQueue.Add(routine);
    }

    public void EndRoutine(IGameRoutine routine){
        outQueue.Add(routine);
    }

    public void EndAllRoutines()
    {
        foreach (var item in routines)
        {
            EndRoutine(item);
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var item in inQueue)
        {
            routines.Add(item);
            item.Start();
        }
        foreach (var item in outQueue)
        {
            routines.Remove(item);
            item.End();
        }
        inQueue.Clear();
        outQueue.Clear();
        foreach (var item in routines)
        {
            item.Update();
        }
        foreach (var item in routines)
        {
            item.LateUpdate();
        }
       
    }
}

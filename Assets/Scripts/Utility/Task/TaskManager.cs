using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class TaskManager : SerializedMonoBehaviour
{
    #region singleton
	private static TaskManager _instance;
	public static TaskManager Instance { get { return _instance ? _instance : (_instance = FindObjectOfType<TaskManager>()); } }
	#endregion
    
    public Dictionary<string, ITask> taskMap = new Dictionary<string, ITask>();
	
}

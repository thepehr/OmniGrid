using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : SerializedMonoBehaviour
{
    #region singleton
    private static ResourceManager _instance;
    public static ResourceManager Instance { get { return _instance ? _instance : (_instance = FindObjectOfType<ResourceManager>()); } }
    #endregion

    [ShowInInspector]
    public Dictionary<string, ObservableResource> resources = new Dictionary<string, ObservableResource>();

    private void Awake()
    {
        foreach (var item in FindObjectsOfType<ObservableResource>())
        {
            resources.Add(item.resourceName, item);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

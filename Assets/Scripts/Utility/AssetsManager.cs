using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class AssetsManager : SerializedMonoBehaviour
{

    private static AssetsManager _Instance;
    public static AssetsManager Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = FindObjectOfType<AssetsManager>();
            }
            return _Instance;
        }
    }

    public Dictionary<string, GameObject> assets = new Dictionary<string, GameObject>();

    public GameObject this[string name]
    {
        get
        {
            if (assets.ContainsKey(name))
            {
                return assets[name];
            }
            else
                return null;
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

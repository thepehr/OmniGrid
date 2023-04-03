using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public float speed;
    public float scrollSpeed;
    public float minOrthoSize;
    public float maxOrthoSize;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * speed / Time.deltaTime;
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize += Input.mouseScrollDelta.y * scrollSpeed, minOrthoSize, maxOrthoSize);
    }

    private void Awake() 
    {
        
    }
}

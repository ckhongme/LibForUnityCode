using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public int queue;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<MeshRenderer>().material.renderQueue = queue;
        Debug.Log(GetComponent<MeshRenderer>().material.renderQueue);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

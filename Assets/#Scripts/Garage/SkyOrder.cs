using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;


public class SkyOrder : MonoBehaviour
{
    public Volume[] Volues;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Volume Volue in Volues)
        {
            Volue.enabled = true;
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}

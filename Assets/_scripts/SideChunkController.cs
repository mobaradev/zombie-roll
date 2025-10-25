using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class SideChunkController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    void Update()
    {
        if (this.transform.position.z < -100)
        {
            Destroy(this.gameObject);
        }
    }
}

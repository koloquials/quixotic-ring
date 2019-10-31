using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestNavButtons : MonoBehaviour
{
    public bool left;
    public TestManager test;

    void Start()
    {
        
    }
    
    void Update()
    {
    }

    private void OnMouseDown()
    {
        if (left)
        {
            test.LastPage();
        }
        else
        {
            test.NextPage();
        }
    }
}

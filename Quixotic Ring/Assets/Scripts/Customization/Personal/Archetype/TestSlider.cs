using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSlider : MonoBehaviour
{
    public int slider;
    public int value;

    public TestManager test;

    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        test.SetAnswer(slider, value);
    }
}

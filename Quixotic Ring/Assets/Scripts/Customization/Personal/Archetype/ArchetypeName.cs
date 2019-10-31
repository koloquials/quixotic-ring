using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArchetypeName : MonoBehaviour
{
    public int num;
    public AppraisalPick pick;

    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        pick.num = num;
    }
}

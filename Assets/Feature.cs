using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feature : MonoBehaviour
{
    public enum Type
    {
        Head,
        Udder,
        Leg,
        Tail,
        ManHead,
        ManLeg,
        ManArm
    }

    public Type type;
    //public float size = 1f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, transform.localScale.x / 2f);
    }
}

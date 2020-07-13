using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGizmos : MonoBehaviour
{
    public enum Type { NORMAL, WAYPOINT}
    public Type type = Type.NORMAL;

    public Color color = Color.yellow;
    public float radius = 0.1f;

   private void OnDrawGizmos() 
   {
        Gizmos.color = color;

        if (type == Type.NORMAL)
        {            
            Gizmos.DrawSphere(transform.position, radius);
        }
        else
        {
            Gizmos.DrawWireSphere(transform.position, radius);
            Gizmos.DrawIcon(transform.position + Vector3.up, "Enemy", true);
        }       
   }
}

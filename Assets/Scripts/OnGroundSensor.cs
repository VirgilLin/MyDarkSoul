using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGroundSensor : MonoBehaviour
{
    public CapsuleCollider capcol;
    public float offset = 0.1f;
    
    private Vector3 point1;
    private Vector3 point2;
    private float raduis;
    // Start is called before the first frame update
    void Awake()
    {
        raduis = capcol.radius - 0.05f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        point1 = transform.position + transform.up * (raduis - offset);
        point2 = transform.position + transform.up * (capcol.height - offset) - transform.up * raduis;
        Collider[] outputCols = Physics.OverlapCapsule(point1, point2, raduis, LayerMask.GetMask("Ground"));
        if (outputCols.Length != 0)
        {
            SendMessageUpwards("IsGround");
        }
        else
        {
            SendMessageUpwards("IsNotGround");
        }
    }
}

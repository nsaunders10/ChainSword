using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererController : MonoBehaviour
{

    public LineRenderer lineRenderer;
    public Transform point1;
    public Transform point2;
    
    void LateUpdate()
    {
        lineRenderer.SetPosition(0, point1.position);
        lineRenderer.SetPosition(1, point2.position);
    }
}

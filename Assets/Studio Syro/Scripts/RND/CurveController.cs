using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveController : MonoBehaviour
{

    public Transform point0;
    public Transform point1;
    public Transform point2;

    public float maxDistance;

    void Start()
    {
        
    }

    
    void Update()
    {

        Quaternion targetRotation = Quaternion.identity;
        targetRotation.y = transform.rotation.y;
        transform.rotation = targetRotation;
        Vector3 localEndPoint = new Vector3(0, 0, maxDistance);
        point2.localPosition = localEndPoint;
        Vector3 targetPoint = point2.position;
        targetPoint.y = 0;
        point2.position = targetPoint;

        Vector3 centerPointLocal = point0.position - point2.position;
        centerPointLocal.z = Vector3.Distance(point0.position, point2.position) /2;      
        point1.localPosition = centerPointLocal;
        Vector3 centerPoint = point1.position;
        centerPoint.y = point0.position.y + Vector3.Distance(point0.position, point2.position) / 4;
        point1.position = centerPoint;
    }
}

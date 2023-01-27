using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class CurveCreator : MonoBehaviour
{
    public Transform curvePoint;
    public int pointCount = 4;
    public List<Transform> points;
    float t;

    [Button]
    void Create()
    {
        for(int i=0; i < pointCount; i++)
        {
            GameObject newPoint = new GameObject("Point" + i);
            newPoint.transform.parent = transform;
            points.Add(newPoint.transform);
        }
        for (int i = 0; i < pointCount - 1; i++)
        {
            GameObject newPoint = new GameObject("Lerp Point" + i);
            newPoint.transform.parent = transform;
            points.Add(newPoint.transform);
        }

        for (int i = 0; i < pointCount - 2; i++)
        {
            GameObject newPoint = new GameObject("Mid Point" + i);
            newPoint.transform.parent = transform;
            points.Add(newPoint.transform);
        }
        
        points.Add(curvePoint.transform);

    }

    
    void Update()
    {
        //De Casteljau's Algorithm

        t = Mathf.PingPong(Time.time, 1);

        for(int i=0; i < points.Count; i++)
        {
            if(i > pointCount-1 && i < pointCount + (pointCount - 1))
            {
                points[i].position = Vector3.Lerp(points[i - pointCount].position, points[(i - pointCount) + 1].position, t);
            }

            if (i > pointCount-1 + (pointCount - 1) && i < points.Count - 1)
            {
                points[i].position = Vector3.Lerp(points[i - (pointCount - 1)].position, points[(i - (pointCount - 1)) + 1].position, t);
            }

            if (i == points.Count - 1)
            {
               points[i].position = Vector3.Lerp(points[pointCount + pointCount - 1].position, points[i - 1].position, t);


            }
        }
    }
}

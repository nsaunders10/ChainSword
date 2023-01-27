using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointerController : MonoBehaviour
{
    public float maxDistance = 10;
    public PointerController otherPointer;
    public HandBehaviour inputHand;
    public Camera pointerCamera;
    public GameObject hitMarker;
    public LineRenderer lineRenderer;

    public bool isPrimary;

    Ray ray;
    RaycastHit hit;

    public bool uiOn = true;
    VRInputModule inputModulel;

    void Start()
    {   
        foreach (Canvas canvas in FindObjectsOfType<Canvas>())
        {
            if (canvas.renderMode == RenderMode.WorldSpace)
                canvas.worldCamera = pointerCamera;
        }

        inputModulel = FindObjectOfType<VRInputModule>();
    }

    void Update()
    {

        if (!isPrimary)
        {
            if(inputHand.trigger.analogValue > 0.5f)
            {
                otherPointer.isPrimary = false;
                isPrimary = true;
                inputModulel.pointerCamera = pointerCamera;
                inputModulel.inputHand = inputHand;
            }
        }

        PointerEventData data = inputModulel.GetData();
        float targetDistance = data.pointerCurrentRaycast.distance == 0 ? maxDistance : data.pointerCurrentRaycast.distance;

        Vector3 endPoint = transform.position + (transform.forward * targetDistance);
        if (hit.collider)
        {
            endPoint = hit.point;
        }

        ray = new Ray(transform.position, transform.forward * targetDistance);
        Physics.Raycast(ray, out hit, targetDistance);

        hitMarker.transform.position = endPoint;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, endPoint);

        if (uiOn)
        {
            lineRenderer.enabled = true;
            hitMarker.SetActive(true);
        }
        if (!uiOn)
        {
            lineRenderer.enabled = false;
            hitMarker.SetActive(false);
        }
    }
}

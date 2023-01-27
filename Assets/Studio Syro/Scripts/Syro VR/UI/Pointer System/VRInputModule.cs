using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VRInputModule : BaseInputModule
{

    public Camera pointerCamera;

    public HandBehaviour inputHand;

    public bool isNetworked;
    GameObject currentObject;
    PointerEventData pointerData;

    bool pointerDown;

    protected override void Awake()
    {
        base.Awake();
        pointerData = new PointerEventData(eventSystem);
    }

    public override void Process()
    {

        if (!pointerCamera)
        {
            return;
        }

        pointerData.Reset();
        pointerData.position = new Vector2(pointerCamera.pixelWidth / 2, pointerCamera.pixelHeight / 2);

        eventSystem.RaycastAll(pointerData, m_RaycastResultCache);
        pointerData.pointerCurrentRaycast = FindFirstRaycast(m_RaycastResultCache);
        currentObject = pointerData.pointerCurrentRaycast.gameObject;

        m_RaycastResultCache.Clear();

        HandlePointerExitAndEnter(pointerData, currentObject);

        
        if (inputHand.trigger.analogValue > 0.8f)
        {
            SetState(pointerData);
            pointerDown = true;
        }
        if (inputHand.trigger.analogValue < 0.8f && pointerDown)
        {
            pointerDown = false;
            SetStateUp(pointerData);
        }
    }

    public PointerEventData GetData()
    {
        return pointerData;
    }

    void SetState(PointerEventData data)
    {
        pointerData.pointerPressRaycast = data.pointerCurrentRaycast;
        GameObject newPointerPress = ExecuteEvents.ExecuteHierarchy(currentObject, data, ExecuteEvents.pointerDownHandler);

        if (newPointerPress)
        {
            newPointerPress = ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentObject);
        }

        data.pressPosition = data.position;
        data.pointerPress = newPointerPress;
        data.rawPointerPress = currentObject;
    }

    void SetStateUp(PointerEventData data)
    {
        ExecuteEvents.Execute(data.pointerPress, data, ExecuteEvents.pointerUpHandler);

        GameObject pointerUpHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentObject);

        if(data.pointerPress == pointerUpHandler)
        {
            ExecuteEvents.Execute(data.pointerPress, data, ExecuteEvents.pointerClickHandler);
        }

        eventSystem.SetSelectedGameObject(null);
        data.pressPosition = Vector2.zero;
        data.pointerPress = null;
        data.rawPointerPress = null;
    }
}

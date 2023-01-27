using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using Normal.Utility;
using NaughtyAttributes;

public class Vector3Sync : RealtimeComponent<Vector3SyncModel>
{

    public Vector3 targetValue;
    public Vector3 currentValue;


    protected override void OnRealtimeModelReplaced(Vector3SyncModel previousModel, Vector3SyncModel currentModel)
    {
        if (previousModel != null)
        {
            previousModel.vector3ValueDidChange -= Vector3ValueDidChange;
        }

        if (currentModel != null)
        {
            if (currentModel.isFreshModel)
            {
                currentModel.vector3Value = Vector3.zero;
            }

            UpdateValue();
            currentModel.vector3ValueDidChange += Vector3ValueDidChange;
        }
    }
    [Button]
    public void SetValue()
    {
        model.vector3Value = targetValue;
    }

    void Vector3ValueDidChange(Vector3SyncModel model, Vector3 value)
    {
        UpdateValue();
    }

    void UpdateValue()
    {
        currentValue = model.vector3Value;
    }

    public void SetValue(Vector3 targetValue)
    {
        model.vector3Value = targetValue;
    }

}

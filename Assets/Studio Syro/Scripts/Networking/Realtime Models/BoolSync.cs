using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using Normal.Utility;
using NaughtyAttributes;

public class BoolSync : RealtimeComponent<BoolSyncModel>
{

    public bool targetValue;
    public bool currentValue;


    protected override void OnRealtimeModelReplaced(BoolSyncModel previousModel, BoolSyncModel currentModel)
    {
        if (previousModel != null)
        {
            previousModel.boolValueDidChange -= BoolValueDidChange;
        }

        if (currentModel != null)
        {
            if (currentModel.isFreshModel)
            {
                currentModel.boolValue = false;
            }

            UpdateValue();
            currentModel.boolValueDidChange += BoolValueDidChange;
        }
    }
    [Button]
    public void SetValue()
    {
        model.boolValue = targetValue;
    }

    void BoolValueDidChange(BoolSyncModel model, bool value)
    {
        UpdateValue();
    }

    void UpdateValue()
    {
        currentValue = model.boolValue;
    }

    public void SetValue(bool targetValue)
    {
        model.boolValue = targetValue;
    }

}
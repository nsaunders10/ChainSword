using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using Normal.Utility;
using NaughtyAttributes;

public class IntSync : RealtimeComponent<IntSyncModel>
{

    public int targetValue;
    public int currentValue;


    protected override void OnRealtimeModelReplaced(IntSyncModel previousModel, IntSyncModel currentModel)
    {
        if (previousModel != null)
        {
            previousModel.intValueDidChange -= IntValueDidChange;
        }

        if (currentModel != null)
        {
            if (currentModel.isFreshModel)
            {
                currentModel.intValue = 0;
            }

            UpdateValue();
            currentModel.intValueDidChange += IntValueDidChange;
        }
    }
    [Button]
    public void SetValue()
    {
        model.intValue = targetValue;
    }

    void IntValueDidChange(IntSyncModel model, int value)
    {
        UpdateValue();
    }

    void UpdateValue()
    {
        currentValue = model.intValue;
    }

    public void SetValue(int targetValue)
    {
        model.intValue = targetValue;
    }

}

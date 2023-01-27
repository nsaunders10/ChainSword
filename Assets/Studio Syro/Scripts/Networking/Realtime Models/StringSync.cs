using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using Normal.Utility;
using NaughtyAttributes;

public class StringSync : RealtimeComponent
{
    public StringSyncModel _model;

    public string targetValue;
    public string currentValue;

    StringSyncModel model
    {
        set
        {
            if (_model != null)
            {
                _model.stringValueDidChange -= StringValueDidChange;
            }
            _model = value;

            if (_model != null)
            {
                UpdateValue();
                _model.stringValueDidChange += StringValueDidChange;

            }
        }
    }

    [Button]
    public void SetValue()
    {
        _model.stringValue = targetValue;
    }

    void StringValueDidChange(StringSyncModel model, string value)
    {
        UpdateValue();
    }

    void UpdateValue()
    {
        currentValue = _model.stringValue;
    }

    public void SetValue(string targetValue)
    {
        _model.stringValue = targetValue;
    }

}
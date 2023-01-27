using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using Normal.Utility;
using NaughtyAttributes;

public class FloatSync : RealtimeComponent
{
    public FloatSyncModel _model;

    public float targetValue;
    public float currentValue;

    FloatSyncModel model
    {
        set
        {
            if (_model != null)
            {
                _model.floatValueDidChange -= FloatValueDidChange;
            }
            _model = value;

            if (_model != null)
            {
                UpdateValue();
                _model.floatValueDidChange += FloatValueDidChange;

            }
        }
    }

    [Button]
    public void SetValue()
    {
        _model.floatValue = targetValue;
    }

    void FloatValueDidChange(FloatSyncModel model, float value)
    {
        UpdateValue();
    }

    void UpdateValue()
    {
        currentValue = _model.floatValue;
    }

    public void SetValue(float targetValue)
    {
        _model.floatValue = targetValue;
    }

}

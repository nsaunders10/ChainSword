using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using Normal.Utility;
using NaughtyAttributes;

public class ColorSync : RealtimeComponent
{
    
    public ColorSyncModel _model;

    public Color targetValue;
    public Color currentValue;

    ColorSyncModel model
    {
        set
        {
            if (_model != null)
            {
                _model.colorValueDidChange -= ColorValueDidChange;
            }
            _model = value;

            if (_model != null)
            {
                UpdateValue();
                _model.colorValueDidChange += ColorValueDidChange;

            }
        }
    }

    [Button]
    public void SetValue()
    {
        _model.colorValue = targetValue;
    }

    void ColorValueDidChange(ColorSyncModel model, Color value)
    {
        UpdateValue();
    }

    void UpdateValue()
    {
        currentValue = _model.colorValue;
    }

    public void SetValue(Color targetValue)
    {
        _model.colorValue = targetValue;
    }

}
using Normal.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime.Serialization;

[RealtimeModel]
public partial class BoolSyncModel
{
    [RealtimeProperty(1, true, true)]
    private bool _boolValue;


}

/* ----- Begin Normal Autogenerated Code ----- */
public partial class BoolSyncModel : RealtimeModel {
    public bool boolValue {
        get {
            return _boolValueProperty.value;
        }
        set {
            if (_boolValueProperty.value == value) return;
            _boolValueProperty.value = value;
            InvalidateReliableLength();
            FireBoolValueDidChange(value);
        }
    }
    
    public delegate void PropertyChangedHandler<in T>(BoolSyncModel model, T value);
    public event PropertyChangedHandler<bool> boolValueDidChange;
    
    public enum PropertyID : uint {
        BoolValue = 1,
    }
    
    #region Properties
    
    private ReliableProperty<bool> _boolValueProperty;
    
    #endregion
    
    public BoolSyncModel() : base(null) {
        _boolValueProperty = new ReliableProperty<bool>(1, _boolValue);
    }
    
    protected override void OnParentReplaced(RealtimeModel previousParent, RealtimeModel currentParent) {
        _boolValueProperty.UnsubscribeCallback();
    }
    
    private void FireBoolValueDidChange(bool value) {
        try {
            boolValueDidChange?.Invoke(this, value);
        } catch (System.Exception exception) {
            UnityEngine.Debug.LogException(exception);
        }
    }
    
    protected override int WriteLength(StreamContext context) {
        var length = 0;
        length += _boolValueProperty.WriteLength(context);
        return length;
    }
    
    protected override void Write(WriteStream stream, StreamContext context) {
        var writes = false;
        writes |= _boolValueProperty.Write(stream, context);
        if (writes) InvalidateContextLength(context);
    }
    
    protected override void Read(ReadStream stream, StreamContext context) {
        var anyPropertiesChanged = false;
        while (stream.ReadNextPropertyID(out uint propertyID)) {
            var changed = false;
            switch (propertyID) {
                case (uint) PropertyID.BoolValue: {
                    changed = _boolValueProperty.Read(stream, context);
                    if (changed) FireBoolValueDidChange(boolValue);
                    break;
                }
                default: {
                    stream.SkipProperty();
                    break;
                }
            }
            anyPropertiesChanged |= changed;
        }
        if (anyPropertiesChanged) {
            UpdateBackingFields();
        }
    }
    
    private void UpdateBackingFields() {
        _boolValue = boolValue;
    }
    
}
/* ----- End Normal Autogenerated Code ----- */

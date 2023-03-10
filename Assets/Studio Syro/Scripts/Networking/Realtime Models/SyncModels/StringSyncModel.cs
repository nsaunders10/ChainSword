using Normal.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime.Serialization;

[RealtimeModel]
public partial class StringSyncModel
{
    [RealtimeProperty(1, true, true)]
    private string _stringValue;


}

/* ----- Begin Normal Autogenerated Code ----- */
public partial class StringSyncModel : RealtimeModel {
    public string stringValue {
        get {
            return _stringValueProperty.value;
        }
        set {
            if (_stringValueProperty.value == value) return;
            _stringValueProperty.value = value;
            InvalidateReliableLength();
            FireStringValueDidChange(value);
        }
    }
    
    public delegate void PropertyChangedHandler<in T>(StringSyncModel model, T value);
    public event PropertyChangedHandler<string> stringValueDidChange;
    
    public enum PropertyID : uint {
        StringValue = 1,
    }
    
    #region Properties
    
    private ReliableProperty<string> _stringValueProperty;
    
    #endregion
    
    public StringSyncModel() : base(null) {
        _stringValueProperty = new ReliableProperty<string>(1, _stringValue);
    }
    
    protected override void OnParentReplaced(RealtimeModel previousParent, RealtimeModel currentParent) {
        _stringValueProperty.UnsubscribeCallback();
    }
    
    private void FireStringValueDidChange(string value) {
        try {
            stringValueDidChange?.Invoke(this, value);
        } catch (System.Exception exception) {
            UnityEngine.Debug.LogException(exception);
        }
    }
    
    protected override int WriteLength(StreamContext context) {
        var length = 0;
        length += _stringValueProperty.WriteLength(context);
        return length;
    }
    
    protected override void Write(WriteStream stream, StreamContext context) {
        var writes = false;
        writes |= _stringValueProperty.Write(stream, context);
        if (writes) InvalidateContextLength(context);
    }
    
    protected override void Read(ReadStream stream, StreamContext context) {
        var anyPropertiesChanged = false;
        while (stream.ReadNextPropertyID(out uint propertyID)) {
            var changed = false;
            switch (propertyID) {
                case (uint) PropertyID.StringValue: {
                    changed = _stringValueProperty.Read(stream, context);
                    if (changed) FireStringValueDidChange(stringValue);
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
        _stringValue = stringValue;
    }
    
}
/* ----- End Normal Autogenerated Code ----- */

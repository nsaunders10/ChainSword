using System;
using UnityEngine;

[Serializable]
public class VRInputs
{

}

[Serializable]
public class VRInput
{
    public float analogValue;
    public Vector2 axis;
    public bool held;
    public bool down;
    public bool up;
    public InputTouch touch;
}

[Serializable]
public class InputTouch
{
    public bool held;
    public bool down;
    public bool up;
}

/*
[Serializable]
public class InputButton
{
    public bool held;
    public bool down;
    public bool up;
}

[Serializable]
public class InputFaceButton
{
    public bool held;
    public bool down;
    public bool up;
    public InputTouch touch;
}
[Serializable]
public class InputStick
{
    public Vector2 axis;
    public InputButton click;
    public InputTouch touch;
}
[Serializable]
public class InputTrigger
{
    public float axis;
    public bool held;
    public bool down;
    public bool up;
}*/
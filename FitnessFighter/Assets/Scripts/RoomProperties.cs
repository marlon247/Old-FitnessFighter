using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomProperties : MonoBehaviour
{
    public bool Top;
    public bool Right;
    public bool Bottom;
    public bool Left;
    public int xScale;
    public int yScale;
    public Vector4 doors { get { return new Vector4(Convert.ToInt32(Top), Convert.ToInt32(Right), Convert.ToInt32(Bottom), Convert.ToInt32(Left)); } }

    public GameObject ground;



}

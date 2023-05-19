using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionDestination : MonoBehaviour
{
    //传送出口标签
    public enum DestinationTag
    {
        // 
        Plant,Entrance,Stadium,Village,Exit,Room,InDoor,OutDoor
    }
    public DestinationTag destinationTag;
}

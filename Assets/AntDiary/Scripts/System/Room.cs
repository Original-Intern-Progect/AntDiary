using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AntDiary
{
    public abstract class Room<T> : NestElement<T> where T : RoomData
    {
        
    }
}
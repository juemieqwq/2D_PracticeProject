using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class StringExpands
{
    public static string BehaviorNameAndNumToString(this string str, BehaviorContainer.RoleBehavior behaviorName, int num)
    {

        return String.Concat(behaviorName, num);
    }
}




using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBaseState : RoleBaseState
{
    protected Player player;
    protected bool isInit = false;
    protected virtual void PlayerInit()
    {
        if (isInit)
            return;
        player = (host as Player);
        isInit = true;
    }
}

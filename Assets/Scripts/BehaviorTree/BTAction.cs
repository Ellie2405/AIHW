using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//a node that carries a function
public class BTAction : BTNode
{
    Action action;

    public BTAction(Action act)
    {
        action = act;
    }

    public override bool Run()
    {
        action();
        return true;
    }
}

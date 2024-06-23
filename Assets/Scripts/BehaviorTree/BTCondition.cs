using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//a node that carries a bool condition
public class BTCondition : BTNode
{
    Func<bool> condition;

    public BTCondition(Func<bool> toCheck)
    {
        condition = toCheck;
    }

    public override bool Run()
    {
        return condition();
    }
}

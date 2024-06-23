using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//runs through children, returns true if one of the children succeeds like OR gate and skips the later children
public class BTSelector : BTNode
{
    BTNode[] children;

    public BTSelector(BTNode[] childrenNodes)
    {
        children = childrenNodes;
    }

    public override bool Run()
    {
        for (int i = 0; i < children.Length;)
        {
            if (children[i].Run())
            {
                return true;
            }
            else
            {
                i++;
            }
        }
        return false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//runs through all children, only returning true if all succeed like AND gate, one fail returns false and skips later children
public class BTSequence : BTNode
{
    BTNode[] children;

    public BTSequence(BTNode[] childrenNodes)
    {
        children = childrenNodes;
    }

    public override bool Run()
    {
        for (int i = 0; i < children.Length;)
        {
            if (children[i].Run())
            {
                i++;
            }
            else
            {
                return false;
            }
        }
        return true;
    }
}

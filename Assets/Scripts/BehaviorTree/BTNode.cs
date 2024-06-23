using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//abstract class for node for behaviour tree
//implmented Composite nodes and Leaf nodes
public abstract class BTNode
{
    public abstract bool Run();
}

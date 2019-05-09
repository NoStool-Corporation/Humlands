using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LoadModels 
{
    public static GameObject TreeModel;
    public static GameObject CarpenterModel;
    static LoadModels ()
    {
        TreeModel = Resources.Load<GameObject>("Graphics/Models/Treemodel/treemodel2");
        CarpenterModel = Resources.Load<GameObject>("Graphics/Models/Workbench/workbench");
    }
}

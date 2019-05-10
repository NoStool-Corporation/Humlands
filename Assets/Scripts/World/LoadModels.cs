using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LoadModels 
{
    public static GameObject TreeModel;
    public static GameObject NPCPrefab;
    public static GameObject CarpenterModel;

    static LoadModels ()
    {
        if (TreeModel != null)
            return;

        TreeModel = Resources.Load<GameObject>("Graphics/Models/Treemodel/treemodel2");
        NPCPrefab = Resources.Load<GameObject>("Resources/Prefabs/Entity");
        CarpenterModel = Resources.Load<GameObject>("Graphics/Models/Workbench/workbench");
    }

    //TreeModel = Resources.Load<Gameobjekt>
}

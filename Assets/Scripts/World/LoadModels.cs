using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LoadModels 
{
   public static GameObject TreeModel;
    static LoadModels ()
    {
        TreeModel = Resources.Load<GameObject>("Graphics/Models/Treemodel/tree5");
    }
}

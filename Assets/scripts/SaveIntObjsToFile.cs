using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class SaveIntObjsToFile : MonoBehaviour
{
    [SerializeField] InterestingScriptable InterestingObjs;
    public KeyCode SaveToFile;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(SaveToFile))
        {
            Debug.Log("[INFO] Saving interesting vertices and Room transform information");
            jsonToFile(InterestingObjs.IntObjsAll);
        }
        
    }
    public void jsonToFile(InterestingVertices objs)
    {
        string jsoned = JsonUtility.ToJson(objs);
        string fileName = "Interesting" + "_" + InterestingObjs.CurModelId  + ".json";
        System.IO.File.WriteAllText(Application.persistentDataPath + "/" +InterestingObjs.CurModelId+"/"+ fileName, jsoned);
        Debug.Log("[INFO] Saved All Interesting Objs with vertices" + (InterestingObjs.IntObjsAll.GetCount()).ToString());
    }
}

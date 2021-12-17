using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SaveLabel : MonoBehaviour
{
    [SerializeField] CamEyeTransform CamEye;
    [SerializeField] LabelScriptable LabelInfos;
    [SerializeField] InterestingScriptable InterestingObjs;
    [SerializeField] VCScriptable VCInfos;

    [SerializeField] GameObject VCManager;
    public float[] fovs = new float[] { 43.47f, 56.423f, 58.498f };

    public bool collectingMode = false;
    public string CurModelId;

    public KeyCode saveKey;
    public KeyCode confirmKey;

    private Label NewLabel;
    private Vector3[] LocPair;
    
    private void Start()
    {
        //Debug.Log(Application.persistentDataPath);
        LabelInfos.Collecting = collectingMode;
        LabelInfos.Adjusting = false;
        LabelInfos.OneRunTimeLabels = new LabelCollection();
        NewLabel = new Label();
        InterestingObjs.CurModelId = CurModelId;
    }
    private void Update()
    {
        if (!LabelInfos.Adjusting & Input.GetKeyDown(saveKey))
        {
            NewLabel = new Label();
            RandomUser();
            RandomVC();
            LocPair = GetLocpair();
            LabelInfos.Adjusting = true;
        }
        if (LabelInfos.Adjusting)
        {
            if (Input.GetKeyDown(confirmKey))
            {
                Debug.Log("Confirming");
                //Make sure that vc location prediction coorespond to the camera forward.
                if(LocPair[0] == CamEye.UserTransform.forward)
                {
                    AddLabelInfo();
                }
                //set user back to 1.7m
                CamEye.UserTransform.localScale = new Vector3(0.15f, 1.7f, 0.15f);
                VCInfos.VCLocalScale = 1f;
                LabelInfos.Adjusting = false;
            }
        }
    }

    private void RandomUser()
    {
        //random height
        float RandomHeightFactor = UnityEngine.Random.Range(-0.25f, 0.25f);
        //random height change
        CamEye.UserTransform.localScale += Vector3.up * RandomHeightFactor;
        CamEye.UserTransform.position = new Vector3 (CamEye.UserTransform.position.x, 
            CamEye.UserTransform.localScale.y / 2 - 0.02f, CamEye.UserTransform.position.z);
        //random fov
        CamEye.RandomFOV = fovs[UnityEngine.Random.Range(0, fovs.Length-1)];
        Debug.Log("Randomizing FOV" + CamEye.RandomFOV);

    }
    private void RandomVC()
    {
        VCInfos.VCLocalScale = UnityEngine.Random.Range(0.9f, 1.2f);
    }

    private Vector3[] GetLocpair()
    {
        //Calculate the location of the VC given the random user;
        var ManagerScript = VCManager.GetComponent<manager>();
        Vector3[] locpair = ManagerScript.GenerateNew();
        return locpair;
    }
    public void AddLabelInfo()
    {
        Debug.Log("Adding one label");
        NewLabel.QueryCenter = CamEye.QueryCenter;
        float willSavePoint = UnityEngine.Random.Range(0, 1);
        //randomly save some Points Around User for validation with a possibility of 20%
        if ( willSavePoint > 0.7f)
        {
            Debug.Log("Saving points");
            NewLabel.PointsAroundNeutral = CamEye.PointsAroundNeutral;
        }

        NewLabel.FOV = CamEye.RandomFOV;
        
        //user location
        NewLabel.UserLocRelToOrigin = CamEye.UserTransform.position;

        NewLabel.UserHeadRelToQueryCenter = TransToCord(new Vector3(CamEye.UserTransform.position.x,
            CamEye.UserTransform.localScale.y,CamEye.UserTransform.position.z), CamEye.QueryCenter); 

        NewLabel.UserFeetRelToQueryCenter = TransToCord(new Vector3(CamEye.UserTransform.position.x,
            0f,CamEye.UserTransform.position.z), CamEye.QueryCenter); 

        //Interesting Objects
        List<string> InterestingNames = new List<string>();
        foreach (GameObject iObj in InterestingObjs.IntObjInRange)
        {
            InterestingNames.Add(iObj.name);
            //Debug.Log(iObj.name);
        }

        NewLabel.InterestingObjs = InterestingNames;

        // Prediction for Virtual Character position
        Vector3 NeutralHead = new Vector3(LocPair[2].x, VCInfos.VCTop, LocPair[2].z);
        NewLabel.NeutralHeadRelToQueryCenter = TransToCord(NeutralHead, CamEye.QueryCenter);
        NewLabel.NeutralFeetRelToQueryCenter = TransToCord(LocPair[2], CamEye.QueryCenter);

        Vector3 confirmedPos = VCManager.GetComponent<manager>().GetVCLocation();
        NewLabel.VCLocRelToOrigin = confirmedPos;
        NewLabel.VCToNeutral = TransToCord(confirmedPos, LocPair[2]);

        NewLabel.onRight = LocPair[1].x;
        NewLabel.score = 1.0f;

        LabelInfos.OneRunTimeLabels.Add(NewLabel);
    }
    private Vector3 TransToCord(Vector3 toBeTransfered, Vector3 queryCenter)
    {
        return toBeTransfered - queryCenter;
    }

    private void SaveLabelsFromJsonToFile(LabelCollection labelList)
    {
        string jsoned = JsonUtility.ToJson(labelList);
        Debug.Log(jsoned);
        string fileName = CurModelId + "__" + System.DateTime.Now.ToString("MM_dd_yy_hh_mm_ss") + ".json";
        System.IO.File.WriteAllText(Application.persistentDataPath + "/" + CurModelId+ "/" + fileName, jsoned);

    }
    private void OnApplicationQuit()
    {
        //One file will be saved for every runtime
        if (LabelInfos.OneRunTimeLabels.GetCount() > 3)
        {
            SaveLabelsFromJsonToFile(LabelInfos.OneRunTimeLabels);
            Debug.Log(System.DateTime.Now.ToString("MM_dd_yy_hh_mm_ss") + ". Saving File");
        }
    }



}

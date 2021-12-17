using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Threading.Tasks;
using UnityEngine.UI;



public class manager : MonoBehaviour
{
    //from scriptable object
    [SerializeField] CamEyeTransform CamEye;
    [SerializeField] InterestingScriptable InterestingObjs;
    [SerializeField] GameObject virtualCharacter;
    [SerializeField] VCScriptable VCInfos;
    [SerializeField] LabelScriptable LabelInfos;
    [SerializeField] Text InfoText;
    [SerializeField] Text VCText;

    public float distToGround = 0f;
    public float SocialDistance = 2.0f;

    public float VCHeight = 1.5f;
    public float VCSpeed = 0.5f;

    private GameObject CurVCharacter;
    private NavMeshAgent VCAgent;
    private bool FineAdjusting = false;
    private Vector3 NeutralPos;

    void Start()
    {
        InitVC();
        if(!LabelInfos.Collecting)
            InvokeRepeating("GenerateNew", 1f, 2f);
    }

    private void Update()
    {
        NeutralPos = CalNewLoc(CamEye.UserTransform, 0f);
        CamEye.QueryCenter = new Vector3 (NeutralPos.x, 0.5f, NeutralPos.z);

        InfoText.text = "User Loc: " + CamEye.UserTransform.position + "\n"
            + "Angle: " + Vector3.SignedAngle(CamEye.ViewRay, CamEye.UserTransform.forward, CamEye.UserTransform.right) + "\n"
            + "User Height: " + CamEye.UserTransform.localScale.y;

        if (VCAgent != null & !VCAgent.pathPending)
        {
            Quaternion rotation = Quaternion.LookRotation(-CamEye.UserTransform.forward, Vector3.up);
            VCAgent.transform.rotation = rotation;
            VCHeight = VCInfos.VCTop;

            VCText.text = "VC Loc:" + VCAgent.transform.position +"\n"
                + "VC Height: " + VCHeight;
        }
        if (!LabelInfos.Adjusting) FineAdjusting = false;
        if(LabelInfos.Adjusting & Input.GetKeyDown(KeyCode.Space)) FineAdjusting = true;

        if (FineAdjusting)
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            Vector3 move = Time.deltaTime * 1 *(VCAgent.transform.right * x + VCAgent.transform.forward * z);
            Vector3 moveDestination = VCAgent.transform.position + move;
            VCAgent.SetDestination(moveDestination);
        }

    }

    public void InitVC()
    {
        while(VCAgent == null)
        {
            Transform cam_trans = CamEye.UserTransform;
            Vector3 goal_pos = FindLocation(cam_trans, true);
            if (goal_pos != new Vector3(-1, -1, -1))
            {
                Debug.Log("Initializing Virtual Character at Position" + goal_pos);
                CurVCharacter = Instantiate(virtualCharacter, goal_pos, Quaternion.identity);
                VCAgent = CurVCharacter.GetComponent<NavMeshAgent>();
            }
        }
    }
    public Vector3[] GenerateNew()
    {
        //return the pair of the location 
        // camera transform forward - VC current Location - neutral position
        //Generate ray to calibrate
        Debug.DrawRay(CamEye.UserTransform.position, CamEye.UserTransform.forward * 5, Color.green, 2f);
        //Debug.DrawRay(CamEye.UserTransform.position + Vector3.up * 0.5f, CamEye.ViewRay * 5, Color.red, 2f);
        VCAgent.transform.localScale = new Vector3(1, Random.Range(0.8f, 1.2f), 1);
        Transform cam_trans = CamEye.UserTransform;
        bool onRight = Vector3.SignedAngle(cam_trans.forward, CurVCharacter.transform.position - cam_trans.position, Vector3.up)>0; ;

        Vector3 goal_pos = FindLocation(cam_trans,onRight) ;
        Vector3[] LocPair = new Vector3[3];
        LocPair[0] = cam_trans.forward;
        if(goal_pos == new Vector3(-1, -1, -1))
        {
            Debug.Log("No Valide position, stay at the place before");
        }
        else
        {
            VCAgent.SetDestination(goal_pos);
            VCAgent.transform.localScale = new Vector3(1, 1, 1);
            
        }
        if (onRight)
        {
            //previous on right side
            LocPair[1] = new Vector3(1, 0, 0);
        }
        else
        {
            //previous on left side
            LocPair[1] = new Vector3(-1, 0, 0);
        }
        LocPair[2] = NeutralPos;
        return LocPair;
    }
    public Vector3 GetVCLocation()
    {
        return VCAgent.transform.position;
    }

    private Vector3 FindLocation(Transform cam,bool onRight)
    {
        
        float FinalAngle;
        if (onRight)
        {
            float Rangle = GetAngles(onRight);
            FinalAngle = Rangle;
            if(Rangle > CamEye.RandomFOV * 0.5)
            {
                float Langle = GetAngles(false);
                if (Langle > -CamEye.RandomFOV * 0.5)
                    FinalAngle = Langle;
            }
        }
        else
        {
            float Langle = GetAngles(false);
            FinalAngle = Langle;
            if (Langle < CamEye.RandomFOV * 0.5)
            {
                float Rangle = GetAngles(true);
                if (Rangle > CamEye.RandomFOV * 0.5)
                    FinalAngle = Rangle;
            }
        }
        Debug.Log("Jumpe to " + FinalAngle);

        Vector3 newLoc = CalNewLoc(cam, FinalAngle);
        return newLoc;
    }
    public float GetAngles(bool OnRight)
    {
        Debug.Log("Getting Once");
        float StartAngle = 0;
        foreach (GameObject io in InterestingObjs.IntObjInRange)
        {
            var SK = io.GetComponent<AmIInteresting>();
            float[] Angles = SK.GetAngles();

            Debug.Log(io.name + ",left: "+ Angles[0] + ", Right: "+Angles[1]);
            float LowerBound = SK.GetLowerBound();

            if (LowerBound > VCHeight || Angles[1] < - CamEye.RandomFOV * 0.5 || Angles[0] > CamEye.RandomFOV * 0.5 )
                continue;
            if(Angles[0]<StartAngle+5 & Angles[1] > StartAngle-5)
            {
                if (OnRight)
                    StartAngle = Angles[1] + 5f;
                else
                    StartAngle = Angles[0] - 7f;
            }
        }
        return StartAngle;
    }
    
    private bool checkValidate(Vector3 pos, Vector3 r)
    {   //grounded
        bool valide = true;
        Vector3 leftBound = pos - r * 0.3f;
        Vector3 rightBound = pos + r * 0.3f; 

        return valide & isGrounded(leftBound) & isGrounded(rightBound) ;
    }
    private Vector3 CalNewLoc(Transform cam, float angle)
    {
        var x = SocialDistance * Mathf.Cos(angle * Mathf.Deg2Rad);
        var z = SocialDistance* Mathf.Sin(angle * Mathf.Deg2Rad);
        var newPosition = cam.position + cam.forward * x +  cam.right * z;
        return new Vector3 (newPosition.x, distToGround,newPosition.z);
    }  
    private bool isGrounded(Vector3 pos)
    {
        NavMeshHit hit;
        //Physics.Raycast(pos, Vector3.down, distToGround))
        if (NavMesh.SamplePosition(pos, out hit, 1.0f, NavMesh.AllAreas))
            return true;
        return false;
    }

}

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.AI;
//using System.Threading.Tasks;
//using UnityEngine.UI;



//public class ManagerBackup : MonoBehaviour
//{
//    //from scriptable object
//    [SerializeField] CamEyeTransform CamEye;
//    [SerializeField] InterestingScriptable InterestingObjs;
//    [SerializeField]
//    private GameObject virtualCharacter;
//    [SerializeField] LabelScriptable LabelInfos;
//    [SerializeField] Text InfoText;

//    public float distToGround = 0f;
//    public float SocialDistance = 2.0f;

//    public float VC_width = 0.5f;
//    public float VC_height = 2.0f;
//    public float VCSpeed = 0.5f;

//    private List<float> interesting_vecs;

//    private GameObject CurVCharacter;
//    private NavMeshAgent VCAgent;
//    private Vector3 TempGoal;
//    private bool FineAdjusting = false;

//    void Start()
//    {
//        InitVC();
//        if (!LabelInfos.Collecting)
//            InvokeRepeating("GenerateNew", 1f, 3f);
//    }

//    private void Update()
//    {
//        InfoText.text = "User Loc: " + CamEye.UserTransform.position + "\n"
//            + "Angle: " + Vector3.SignedAngle(CamEye.ViewRay, CamEye.UserTransform.forward, CamEye.UserTransform.right) + "\n"
//            + "User Height: " + CamEye.UserTransform.localScale.y;


//        if (VCAgent != null & !VCAgent.pathPending)
//        {
//            Quaternion rotation = Quaternion.LookRotation(-CamEye.UserTransform.forward, Vector3.up);
//            VCAgent.transform.rotation = rotation;
//        }
//        if (!LabelInfos.Adjusting) FineAdjusting = false;
//        if (LabelInfos.Adjusting & Input.GetKeyDown(KeyCode.Space)) FineAdjusting = true;

//        if (FineAdjusting)
//        {
//            float x = Input.GetAxis("Horizontal");
//            float z = Input.GetAxis("Vertical");
//            Vector3 move = VCAgent.transform.right * x + VCAgent.transform.forward * z;
//            //Vector3 movement = new Vector3(x, 0f, z);
//            Vector3 moveDestination = VCAgent.transform.position + move;
//            VCAgent.SetDestination(moveDestination);
//        }

//    }

//    public void InitVC()
//    {
//        while (VCAgent == null)
//        {
//            Transform cam_trans = CamEye.UserTransform;
//            Vector3 goal_pos = FindLocation(cam_trans,  true);
//            if (goal_pos != new Vector3(-1, -1, -1))
//            {
//                Debug.Log("Initializing Virtual Character at Position" + goal_pos);
//                CurVCharacter = Instantiate(virtualCharacter, goal_pos, Quaternion.identity);
//                VCAgent = CurVCharacter.GetComponent<NavMeshAgent>();
//            }
//        }
//    }
//    public Vector3[] GenerateNew()
//    {
//        //Generate ray to calibrate
//        Debug.DrawRay(CamEye.UserTransform.position, CamEye.UserTransform.forward * 5, Color.green, 2f);
//        Debug.DrawRay(CamEye.UserTransform.position + Vector3.up * 0.5f, CamEye.ViewRay * 5, Color.red, 2f);

//        Transform cam_trans = CamEye.UserTransform;
//        bool onRight = Vector3.SignedAngle(cam_trans.forward, CurVCharacter.transform.position - cam_trans.position, Vector3.up) > 0; ;

//        Vector3 goal_pos = FindLocation(cam_trans, CamEye.Objs, onRight);
//        Vector3[] LocPair = new Vector3[3];
//        LocPair[0] = cam_trans.position;
//        LocPair[1] = cam_trans.forward;
//        if (goal_pos == new Vector3(-1, -1, -1))
//        {
//            Debug.Log("No Valide position, stay at the place before");
//            LocPair[2] = CurVCharacter.transform.position;
//        }
//        else
//        {
//            Debug.Log("Setting Destination" + goal_pos);
//            TempGoal = goal_pos;
//            VCAgent.SetDestination(goal_pos);
//            LocPair[2] = goal_pos;
//        }
//        return LocPair;
//    }
//    public Vector3 GetVCLocation()
//    {
//        return VCAgent.transform.position;
//    }
//    private Vector3 FindLocation(Transform cam, List<Transform> objs, bool onRight)
//    {
//        //float start_angle = 0;
//        ////half human + half interesting obj
//        //float dif = 15f;
//        //Vector3 newLoc;

//        //if (objs.Count == 0)
//        //    return CalNewLoc(cam, 0f);

//        //List<Vector3> obj_vecs = new List<Vector3>();
//        //foreach(Transform obj in objs)
//        //    obj_vecs.Add(obj.position - cam.position);

//        //List<float> angles = calAngles(obj_vecs, cam.forward);
//        //angles.Sort();

//        //if (onRight)
//        //    start_angle = JumpToRight(angles, dif);
//        //else
//        //    start_angle = JumpToLeft(angles, dif);

//        //newLoc = CalNewLoc(cam, start_angle);

//        //if (!checkValidate(newLoc, cam.right)){
//        //    return new Vector3(-1, -1, -1);
//        //}
//        float FinalAngle;
//        if (onRight)
//        {
//            float Rangle = GetAngles(onRight);
//            FinalAngle = Rangle;
//            if (Rangle > CamEye.RandomFOV * 0.5)
//            {
//                float Langle = GetAngles(false);
//                if (Langle > -CamEye.RandomFOV * 0.5)
//                    FinalAngle = Langle;
//            }
//        }
//        else
//        {
//            float Langle = GetAngles(false);
//            FinalAngle = Langle;
//            if (Langle < CamEye.RandomFOV * 0.5)
//            {
//                float Rangle = GetAngles(true);
//                if (Rangle > CamEye.RandomFOV * 0.5)
//                    FinalAngle = Rangle;
//            }
//        }
//        Vector3 newLoc = CalNewLoc(cam, FinalAngle);
//        return newLoc;
//    }
//    public float GetAngles(bool OnRight)
//    {
//        float StartAngle = 0;
//        foreach (GameObject io in InterestingObjs.IntObjInRange)
//        {
//            var SK = io.GetComponent<AmIInteresting>();
//            float[] Angles = SK.GetAngles();

//            if (Angles[1] < -CamEye.RandomFOV * 0.5 || Angles[0] > CamEye.RandomFOV * 0.5)
//                continue;
//            if (Angles[0] < StartAngle + 5 & Angles[1] > StartAngle - 5)
//            {
//                if (OnRight)
//                    StartAngle = Angles[1] + 5f;
//                else
//                    StartAngle = Angles[0] - 7f;
//                Debug.Log("New Angle" + StartAngle);

//            }
//        }
//        return StartAngle;
//    }

//    private float JumpToRight(List<float> angles, float dif)
//    {
//        float start_angle = 0f;
//        int i = 0;
//        while (i < angles.Count)
//        {
//            if (angles[i] < -15f)
//            {
//                i += 1;
//                continue;
//            }

//            if (Mathf.Abs(angles[i] - start_angle) > dif)
//            {
//                Debug.Log("enough difference for " + angles[i]);
//                break;
//            }
//            else
//            {
//                if (angles[i] < 0)
//                    start_angle = angles[i] + 18f;
//                else
//                    start_angle = angles[i] + 4f;
//                Debug.Log("Jumping to right" + start_angle);
//            }
//            i += 1;
//        }
//        return start_angle;
//    }
//    private float JumpToLeft(List<float> angles, float dif)
//    {
//        float start_angle = 0f;
//        int i = angles.Count - 1;
//        if (i == -1)
//            return 0f;
//        while (i >= 0)
//        {
//            if (angles[i] > 15f)
//            {
//                i -= 1;
//                continue;
//            }

//            if (Mathf.Abs(start_angle - angles[i]) > dif)
//            {
//                Debug.Log("enough difference for " + angles[i]);
//                break;
//            }
//            else
//            {
//                if (angles[i] > 0)
//                    start_angle = angles[i] - 18f;
//                else
//                    start_angle = angles[i] - 4f;
//                Debug.Log("Jumping to left" + start_angle);
//            }
//            i -= 1;
//        }
//        return start_angle;
//    }
//    private List<float> calAngles(List<Vector3> objs, Vector3 cam_forward)
//    {
//        List<float> angles = new List<float>();

//        if (objs.Count == 0)
//            return angles;

//        foreach (Vector3 obj in objs)
//        {
//            float angle = Vector3.SignedAngle(cam_forward, obj, Vector3.up);
//            if (angle > -40 & angle < 40)
//            {
//                angles.Add(angle);
//            }
//        }
//        return angles;
//    }
//    private bool checkValidate(Vector3 pos, Vector3 r)
//    {   //grounded
//        bool valide = true;
//        Vector3 leftBound = pos - r * VC_width;
//        Vector3 rightBound = pos + r * VC_width;

//        return valide & isGrounded(leftBound) & isGrounded(rightBound);
//    }
//    private Vector3 CalNewLoc(Transform cam, float angle)
//    {
//        var x = SocialDistance * Mathf.Cos(angle * Mathf.Deg2Rad);
//        var z = SocialDistance * Mathf.Sin(angle * Mathf.Deg2Rad);
//        var newPosition = cam.position + cam.forward * x + cam.right * z;
//        return new Vector3(newPosition.x, distToGround, newPosition.z);
//    }
//    private Vector3 rotateTo(Vector3 origin, Vector3 pivot, float angle)
//    {
//        Vector3 angles = new Vector3(angle, 0, 0);
//        Vector3 dir = origin - pivot; // get point direction relative to pivot
//        dir = Quaternion.Euler(angles) * dir; // rotate it
//        origin = dir + pivot; // calculate rotated point
//        return origin; // return it
//    }
//    private bool isGrounded(Vector3 pos)
//    {
//        NavMeshHit hit;
//        //Physics.Raycast(pos, Vector3.down, distToGround))
//        if (NavMesh.SamplePosition(pos, out hit, 1.0f, NavMesh.AllAreas))
//            return true;
//        return false;
//    }

//}

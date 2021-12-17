/*MIT License

Copyright(c) 2018 Vili Volčini / viliwonka

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
namespace DataStructures.ViliWonka.Tests
{

    using KDTree;
    //public enum QType
    //{

    //    ClosestPoint,
    //    KNearest,
    //    Radius,
    //    Interval
    //}

    public class KDTreeTest : MonoBehaviour
    {

        [SerializeField] GameObject room;
        [SerializeField] CamEyeTransform camEye;
        [SerializeField] LabelScriptable LabelInfos;

        //balance factor
        public int BalanceBuildSpeedFactor = 32;
        
        //Query Parameters
        public QType QueryType;
        public int K = 13;
        
        [Range(0f, 100f)]
        public float Radius = 0.5f;
        public Vector3 center;
        public float XRotation = -90f;
        public float queryHeight = 0.5f;

        public Vector3 IntervalSize = new Vector3(0.2f, 0.2f, 0.2f);


        public bool DrawQueryNodes = false;
        public bool DrawResult = true;
        public bool DrawWhole= false;

        Vector3[] WholePCloud;
        Vector3[] ResultPC;

        KDTree tree;
        KDQuery query;

        private void Awake()
        {
            Debug.Log("Awaking");
            MeshFilter RoomMeshFilter = room.GetComponent<MeshFilter>();
            Mesh RoomMesh = RoomMeshFilter.mesh;
            WholePCloud = RoomMesh.vertices;

            // Rotate model to make sure the y axis is the upward axis 
            for (int i = 0; i < WholePCloud.Length; i++)
            {
                WholePCloud[i] = Quaternion.Euler(XRotation, 0, 0) * WholePCloud[i];
                //WholePCloud[i].y += 1;
            }
        }
        void Start()
        {
            DateTime before = DateTime.Now;
            buildOnce();
            DateTime after = DateTime.Now;
            Debug.Log("Build Tree in " + (after - before));
            QueryRadiusAroundUser();
        }
        //private void Update()
        //{
        //    if (LabelInfos.Collecting && !LabelInfos.Adjusting)
        //        QueryRadiusAroundUser();
        //}
        void buildOnce()
        {
            query = new KDQuery();
            tree = new KDTree(WholePCloud, BalanceBuildSpeedFactor);
        }

        public void QueryRadiusAroundUser ()
        {
            if (query == null)
                return;
            var resultIndices = new List<int>();

            //camEye.QueryCenter = center;

            if (camEye.QueryCenter == null)
                center = new Vector3(camEye.UserTransform.position.x, queryHeight,camEye.UserTransform.position.z);
            else
                center = camEye.QueryCenter;

            query.Radius(tree, center, Radius, resultIndices);
            
            Vector3[] PC = new Vector3[resultIndices.Count];
            for (int i = 0; i < resultIndices.Count; i++)
            {
                PC[i] = WholePCloud[resultIndices[i]];
                //PC[i] = WholePCloud[resultIndices[i]] - center;
            }
            camEye.PointsAroundNeutral = PC;
            ResultPC = PC;
        }


        private void OnDrawGizmos()
        {
            if (!DrawResult & !DrawWhole)
            {
                return;
            }
            if (query == null)
                return;

            Vector3 size = 0.01f * Vector3.one;
            if (DrawQueryNodes)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawCube(center, 10f * size);
            }

            if (DrawWhole)
            {
                Gizmos.color = Color.black;
                for (int i = 0; i < WholePCloud.Length; i++)
                {
                    Gizmos.DrawCube(WholePCloud[i], 0.5f * size);
                }
            }
            if(DrawResult & ResultPC != null)
            {
                Color markColor = Color.red;
                markColor.a = 0.5f;
                Gizmos.color = markColor;
                for (int i = 0; i < ResultPC.Length; i++)
                {
                    Gizmos.DrawCube(ResultPC[i], size);
                }
            }

            //var resultIndices = new List<int>();
            //switch (QueryType)
            //{

            //    case QType.ClosestPoint:
            //        {

            //            query.ClosestPoint(tree, transform.position, resultIndices);
            //        }
            //        break;

            //    case QType.KNearest:
            //        {

            //            query.KNearest(tree, transform.position, K, resultIndices);
            //        }
            //        break;

            //    case QType.Radius:
            //        {
            //            Debug.Log("Querying Radius");

            //            query.Radius(tree, center, Radius, resultIndices);

            //            Gizmos.DrawWireSphere(transform.position, Radius);
            //        }
            //        break;

            //    case QType.Interval:
            //        {

            //            query.Interval(tree, transform.position - IntervalSize / 2f, transform.position + IntervalSize / 2f, resultIndices);

            //            Gizmos.DrawWireCube(transform.position, IntervalSize);
            //        }
            //        break;

            //    default:
            //        break;
            //}

            //var directory = EditorUtility.OpenFolderPanel("Save Draco files to folder", "", "");

            //Gizmos.color = Color.green;
            //Gizmos.DrawCube(transform.position, 4f * size);
            //if (drawquerynodes)
            //{
            //    query.drawlastquery();
            //}
        }

    }
}
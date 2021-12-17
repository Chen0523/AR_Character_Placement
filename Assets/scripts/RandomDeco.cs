using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomDeco : MonoBehaviour
{
    //public GameObject[] objs;
    //GameObject toBeSpawned;
    // Start is called before the first frame update
    public float XRange;
    public float YRange;
    public float ZRange;
    public float UpperScale = 3f;
    void Start()
    {
        Random.seed = int.Parse(this.name.Split(char.Parse("_"))[0]);
        //toBeSpawned = objs[Random.Range(0, objs.Length)];
        //GameObject spa = Instantiate(toBeSpawned, this.transform.position, Quaternion.identity);
        //float RandomScale = Random.Range(0.8f, 2f);
        //spa.transform.localScale = new Vector3(RandomScale, RandomScale, RandomScale);
        //spa.transform.rotation = Quaternion.Euler(Random.Range(-180,180), Random.Range(-180, 180), Random.Range(-180, 180));
        //spa.transform.position = this.transform.position;
        //GetComponent<MeshFilter>().mesh = meshes[Random.Range(0, meshes.Length)]; 
        float RandomScale = Random.Range(-0.2f, UpperScale);
        transform.localScale = new Vector3(transform.localScale.x + RandomScale, 
            transform.localScale.y + RandomScale, transform.localScale.z + RandomScale);
        Vector3 RotationToAdd = new Vector3(Random.Range(-XRange, XRange), Random.Range(-YRange, YRange), Random.Range(-ZRange, ZRange));
        transform.Rotate(RotationToAdd, Space.World);
    }
}

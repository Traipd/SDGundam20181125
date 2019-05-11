using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopingMesh1 : MonoBehaviour {

    public int count=2;
	
    private Material mat; 
	private Mesh mesh;
	private Mesh Tmesh;
	private GameObject TgameObject;
	private MeshFilter mf;
	// Use this for initialization
	void Start () {
		mesh = GetComponent<MeshFilter>().mesh;

        TgameObject = new GameObject("TarlingObject");//特效对象
		//TgameObject.transform.SetParent(transform, false);
 
        var mr = TgameObject.AddComponent<MeshRenderer>();//赋予材质
		mat = new Material(Shader.Find("Standard"));
		mr.material = mat;


        mf = TgameObject.AddComponent<MeshFilter>();
	    Tmesh = new Mesh();

		// Debug.Log("mesh.vertices.Length:"+mesh.vertices.Length);
        Vector3[] v3s=new Vector3[mesh.vertices.Length];
		Debug.Log("mesh.vertices.Length:"+mesh.vertices.Length);
		Debug.Log("mesh.uv.Length:"+mesh.uv.Length);
		Debug.Log("mesh.triangles.Length:"+mesh.triangles.Length);
        for(int i=0;i<v3s.Length;i++)
		{
		       
			v3s[i]=GetComponent<Transform>().localToWorldMatrix.MultiplyPoint(mesh.vertices[i]+Vector3.up*count);
			// v3s[i]=mesh.vertices[i]+Vector3.up*count;
			// Debug.Log(v3s[i]);
			// Debug.Log("x:"+mesh.vertices[i].x+"  y:"+mesh.vertices[i].y+"  z:"+mesh.vertices[i].z);
			// Debug.Log(i+" tx:"+v3s[i].x+"  ty:"+v3s[i].y+"  tz:"+v3s[i].z);
		}
		Tmesh.vertices = v3s;

        Tmesh.uv = mesh.uv;
        Tmesh.triangles = mesh.triangles;
		Tmesh.RecalculateBounds();
		Tmesh.RecalculateNormals();

        mf.mesh = Tmesh;

        
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

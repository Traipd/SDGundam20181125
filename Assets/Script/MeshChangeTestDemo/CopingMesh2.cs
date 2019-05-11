using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopingMesh2 : MonoBehaviour {
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
        Vector3[] v3s=new Vector3[mesh.vertices.Length*count];
	    int[] t2s=new int[mesh.triangles.Length*count];
        Vector2[] uv2s=new Vector2[mesh.uv.Length*count];

		for(int j=0;j<count;j++)
		{
			for(int i=0;i<mesh.vertices.Length;i++)
			{
				
				v3s[j*mesh.vertices.Length+i]=GetComponent<Transform>().localToWorldMatrix.MultiplyPoint(mesh.vertices[i]+Vector3.up*(j+1));				
				uv2s[j*mesh.vertices.Length+i]=mesh.uv[i];				

			}
		}
		for(int j=0;j<count;j++)
		{
			for(int i=0;i<mesh.triangles.Length;i++)
			{
				t2s[j*mesh.triangles.Length+i]=mesh.triangles[i]+j*mesh.vertices.Length;			

			}
		}
			

		Tmesh.vertices = v3s;

        Tmesh.uv = uv2s;
        Tmesh.triangles = t2s;

		Tmesh.RecalculateBounds();
		Tmesh.RecalculateNormals();

        mf.mesh = Tmesh;

        
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

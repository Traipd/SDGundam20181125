using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailingMesh : MonoBehaviour {

	 public int count=1;
	
    private Material mat; 
	private Mesh Tmesh;
	private GameObject TgameObject;
	private MeshFilter mf;

	private SkinnedMeshRenderer render;
	
	// Use this for initialization
	void Start () {
		render = GetComponent<SkinnedMeshRenderer>();

        TgameObject = new GameObject("TarlingObject");//特效对象
		// TgameObject.transform.SetParent(transform, false);
 
        var mr = TgameObject.AddComponent<MeshRenderer>();//赋予材质
		mat = new Material(Shader.Find("Standard"));
		mr.material = mat;


        mf = TgameObject.AddComponent<MeshFilter>();
	//    Debug.Log("bones.Length:"+bones.Length);
		
	// 	Debug.Log("mesh.boneWeights.Length:"+mesh.boneWeights.Length);
	// 	Debug.Log("bindPoses.Length:"+bindPoses.Length);
  
        
	}
	
	// Update is called once per frame
	void Update () {

      #region-------从SkinnedMeshRenderer上拷贝可以正确显示的网格
		TgameObject.transform.position = transform.position;
        TgameObject.transform.rotation = transform.rotation;
		Tmesh = new Mesh();
		render.BakeMesh(Tmesh);
	  #endregion
	  
		// Vector3[] v3s=new Vector3[mesh.vertices.Length*count];
	    // int[] t2s=new int[mesh.triangles.Length*count];
        // Vector2[] uv2s=new Vector2[mesh.uv.Length*count];

		// for(int j=0;j<count;j++)
		// {
		// 	for(int i=0;i<mesh.vertices.Length;i++)
		// 	{
				

		// 		// v3s[j*mesh.vertices.Length+i]=bones[weights[i].boneIndex0].worldToLocalMatrix * transform.localToWorldMatrix.MultiplyPoint(mesh.vertices[i]);//GetComponent<Transform>().localToWorldMatrix.MultiplyPoint(mesh.vertices[i])+Vector3.up;//											
		// 		v3s[j*mesh.vertices.Length+i]=Tmesh.vertices[i];//
		// 		v3s[j*mesh.vertices.Length+i]+=Vector3.up;
		// 	}
		// }
		
		// t2s=mesh.triangles;
		// uv2s=mesh.uv;
		
		// Tmesh.vertices = v3s;
        // Tmesh.uv = uv2s;
        // Tmesh.triangles = t2s;

		// Tmesh.RecalculateBounds();
		// Tmesh.RecalculateNormals();

        mf.mesh = Tmesh;
	   
	}
}

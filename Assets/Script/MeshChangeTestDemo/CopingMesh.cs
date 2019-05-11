using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopingMesh : MonoBehaviour {

    public int TarlingVerticrNum = 5;
	public Material mat; 
	private Mesh mesh;
	private GameObject TgameObject;
	private MeshFilter mf;
	private Vector3[] vertices;//系统自动更新
	private Vector3[] TFvertices;
	private Vector2[] Tuvs;//不用更新
	private Vector3[][] Tvertices;
	private int[] tris;//不用更新
	// Use this for initialization
	void Start () {
		mesh = GetComponent<MeshFilter>().mesh;
		vertices = mesh.vertices;

       Tvertices = new Vector3[TarlingVerticrNum+1][];//增设拖动产生的顶点的存储数组
	   TFvertices= new Vector3[TarlingVerticrNum*vertices.Length];
	   Tuvs=new Vector2[TarlingVerticrNum*vertices.Length];
       tris=new int[TarlingVerticrNum*vertices.Length*6];
		for(int i=0; i<TarlingVerticrNum+1; i++)
		{
            Tvertices[i]=vertices;
		}

		TgameObject = new GameObject("TarlingObject");//特效对象
        mf = TgameObject.AddComponent<MeshFilter>();
        var mr = TgameObject.AddComponent<MeshRenderer>();

        mat = new Material(Shader.Find("Standard"));
		mr.material = mat;

        #region -------------------------初始化顶点组---------------------------------------------
        Mesh Tmesh = new Mesh();
        
		for(int i=0; i<TarlingVerticrNum+1; i++)
		{
			for(int j=0;j<vertices.Length;j++)
			{
				TFvertices[i*vertices.Length+j]=Tvertices[i][j];
				Tuvs[i*vertices.Length+j]=new Vector2(i/TarlingVerticrNum,j/vertices.Length);
			}
		}

        for(int i=0; i<TarlingVerticrNum; i++)
		{
			for(int j=0;j<vertices.Length;j++)
			{		
				int vgroupHead=(i*vertices.Length+j)*6;
                tris[vgroupHead]=vgroupHead;
				tris[vgroupHead+1]=vgroupHead+vertices.Length*6;
				tris[vgroupHead+2]=vgroupHead+1;
				tris[vgroupHead+3]=vgroupHead+1;
				tris[vgroupHead+4]=vgroupHead+vertices.Length*6;
				tris[vgroupHead+5]=vgroupHead+1+vertices.Length*6;
			}
		}

        Tmesh.vertices = TFvertices;
        Tmesh.uv = Tuvs;
        Tmesh.triangles = tris;
		Tmesh.RecalculateBounds();
        mf.mesh = Tmesh;
		
       #endregion-------------------------------------------------------------------

		
	}
	
	// Update is called once per frame
	private float count=0;
	void Update () {
		
    #region -------------------------随时间变化---------------------------------------------
	count+=Time.deltaTime;
	if(count>=1)
	{
		count-=1;
		Mesh Tmesh = new Mesh();

        
		for(int i=TarlingVerticrNum-1; i>0; i++)
		{
			Tvertices[i]=Tvertices[i-1];
		}
        Tvertices[0]=vertices;

		for(int i=0; i<TarlingVerticrNum+1; i++)
		{
			for(int j=0;j<vertices.Length;j++)
			{
				TFvertices[i*vertices.Length+j]=Tvertices[i][j];
			}
		}

        

        Tmesh.vertices = TFvertices;
		Tmesh.RecalculateBounds();
        mf.mesh = Tmesh;
		
	}
        
       #endregion-------------------------------------------------------------------
		
	}
}

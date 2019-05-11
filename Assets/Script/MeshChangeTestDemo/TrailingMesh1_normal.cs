using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailingMesh1_normal : MonoBehaviour {

	public bool canSeeAtStart=true;
	public int count=12;//缓存数组长度
	public float interval_time=0.03f;//网格更新时间间隔
	public Material my_mat;

	#region-------从MeshRenderer上拷贝可以正确显示的网格
    private Material Fmat; 
	private Mesh Fmesh;
	private GameObject FgameObject;
	private MeshFilter Fmf;
	private MeshRenderer render;
	#endregion
	
	private Material mat; 
	private Mesh Tmesh;//特效网格
	private GameObject TgameObject;//特效对象
	private MeshFilter mf;
	private MeshRenderer mr;

    Vector3[] v3s;//拖尾顶点坐标
	int[] t2s;//拖尾顶点绘制顺序
	Vector2[] uv2s;//拖尾uv
	// Vector3[] n3s;//拖尾法线

	// Use this for initialization
	void Start () {
		render = GetComponent<MeshRenderer>();

        FgameObject = new GameObject("FTarlingObject");//缓存用的网格对象
        // var fmr = FgameObject.AddComponent<MeshRenderer>();//赋予材质
		FgameObject.AddComponent<MeshRenderer>();//赋予材质
		// Fmat = new Material(Shader.Find("Standard"));
		// fmr.material = Fmat;
        Fmf = FgameObject.AddComponent<MeshFilter>();

		FgameObject.GetComponent<MeshRenderer>().enabled=false;
	//    Debug.Log("bones.Length:"+bones.Length);
		 
		TgameObject = new GameObject("TarlingObject");//特效对象
        mr = TgameObject.AddComponent<MeshRenderer>();
		mr.enabled=canSeeAtStart;  //提前设置不可见

        if(my_mat==null)//赋予材质
		{
		mat = new Material(Shader.Find("Standard"));
		mr.material = mat;
		}else{
			mr.material=my_mat;
		}
		
        
		mf = TgameObject.AddComponent<MeshFilter>();

		#region-------从MeshRenderer上拷贝可以正确显示的网格
			FgameObject.transform.position = transform.position;
			FgameObject.transform.rotation = transform.rotation;
			Fmesh = new Mesh();
			Fmesh = GetComponent<MeshFilter>().mesh;
			Fmf.mesh = Fmesh;//赋予显示用的网格
		#endregion

		v3s=new Vector3[Fmesh.vertices.Length*count];
		t2s=new int[(Fmesh.vertices.Length-1)*(count-1)*6];
		uv2s=new Vector2[Fmesh.uv.Length*count];
		// n3s=new Vector3[Fmesh.normals.Length*count];
		// Debug.Log("Fmesh.uv.Length:"+Fmesh.uv.Length);
		// Debug.Log("Fmesh.vertices.Length:"+Fmesh.vertices.Length);
		

		for(int j=0;j<count;j++)//设置uv和顶点绘制顺序
			{
				for(int i=0;i<Fmesh.vertices.Length;i++)
				{
					
					// v3s[j*Fmesh.vertices.Length+i]=FgameObject.GetComponent<Transform>().localToWorldMatrix.MultiplyPoint(Fmesh.vertices[i]+Vector3.up*(j+1));	
					v3s[j*Fmesh.vertices.Length+i]=GetComponent<Transform>().localToWorldMatrix.MultiplyPoint(Fmesh.vertices[i]);
					if(uv2s.Length!=0)
					{
						uv2s[j*Fmesh.vertices.Length+i]=new Vector2(Fmesh.uv[i].x,j/(count-1));	
					}
						// n3s[j*Fmesh.vertices.Length+i]=Fmesh.normals[i];

					if((i<Fmesh.vertices.Length-1)&&(j<count-1))
					{
						t2s[(j*(Fmesh.vertices.Length-1)+i)*6]=j*Fmesh.vertices.Length+i;
						t2s[(j*(Fmesh.vertices.Length-1)+i)*6+1]=(j+1)*Fmesh.vertices.Length+i;
						t2s[(j*(Fmesh.vertices.Length-1)+i)*6+2]=(j+1)*Fmesh.vertices.Length+i+1;
						t2s[(j*(Fmesh.vertices.Length-1)+i)*6+3]=(j+1)*Fmesh.vertices.Length+i+1;
						t2s[(j*(Fmesh.vertices.Length-1)+i)*6+4]=j*Fmesh.vertices.Length+i+1;
						t2s[(j*(Fmesh.vertices.Length-1)+i)*6+5]=j*Fmesh.vertices.Length+i;
					}	  			

				}
			}
	}
	
	// Update is called once per frame
	float timedelay=0;
	void Update () {

      timedelay+=Time.deltaTime;
	  if(timedelay>interval_time)//时间间隔
	  {	
		timedelay=0;

		#region-------从MeshRenderer上拷贝可以正确显示的网格
			FgameObject.transform.position = transform.position;
			FgameObject.transform.rotation = transform.rotation;
			Fmesh = new Mesh();
			Fmesh = GetComponent<MeshFilter>().mesh;
			Fmf.mesh = Fmesh;//赋予显示用的网格
		#endregion
		
			Tmesh = new Mesh();

			for(int j=count-1;j>=1;j--)//刷新顶点位置
			{
				for(int i=Fmesh.vertices.Length-1;i>=0;i--)
				{
					v3s[j*Fmesh.vertices.Length+i]=v3s[(j-1)*Fmesh.vertices.Length+i];
				}
			}
                for(int i=Fmesh.vertices.Length-1;i>=0;i--)
				{
					v3s[i]=GetComponent<Transform>().localToWorldMatrix.MultiplyPoint(Fmesh.vertices[i]);//+Vector3.up*(1)//FgameObject.GetComponent<Transform>().localToWorldMatrix.MultiplyPoint(Fmesh.vertices[i])
				}

			Tmesh.vertices = v3s;

			Tmesh.uv = uv2s;
			Tmesh.triangles = t2s;
			// Tmesh.normals=n3s;

			Tmesh.RecalculateBounds();
			Tmesh.RecalculateNormals();

			mf.mesh = Tmesh;//赋予显示用的网格
	   }
	}
	///<summary>令拖影可见</summary>
	public void setVisual()
	{
		mr.enabled=true;

		#region-------从MeshRenderer上拷贝可以正确显示的网格
			FgameObject.transform.position = transform.position;
			FgameObject.transform.rotation = transform.rotation;
			Fmesh = new Mesh();
			Fmesh = GetComponent<MeshFilter>().mesh;
			Fmf.mesh = Fmesh;//赋予显示用的网格
		#endregion

		v3s=new Vector3[Fmesh.vertices.Length*count];
		t2s=new int[(Fmesh.vertices.Length-1)*(count-1)*6];
		uv2s=new Vector2[Fmesh.uv.Length*count];

		for(int j=0;j<count;j++)//设置uv和顶点绘制顺序
			{
				for(int i=0;i<Fmesh.vertices.Length;i++)
				{
					
					// v3s[j*Fmesh.vertices.Length+i]=FgameObject.GetComponent<Transform>().localToWorldMatrix.MultiplyPoint(Fmesh.vertices[i]+Vector3.up*(j+1));	
					v3s[j*Fmesh.vertices.Length+i]=GetComponent<Transform>().localToWorldMatrix.MultiplyPoint(Fmesh.vertices[i]);
					if(uv2s.Length!=0)
					{
						uv2s[j*Fmesh.vertices.Length+i]=new Vector2(Fmesh.uv[i].x,j/(count-1));	
					}
						// n3s[j*Fmesh.vertices.Length+i]=Fmesh.normals[i];

					if((i<Fmesh.vertices.Length-1)&&(j<count-1))
					{
						t2s[(j*(Fmesh.vertices.Length-1)+i)*6]=j*Fmesh.vertices.Length+i;
						t2s[(j*(Fmesh.vertices.Length-1)+i)*6+1]=(j+1)*Fmesh.vertices.Length+i;
						t2s[(j*(Fmesh.vertices.Length-1)+i)*6+2]=(j+1)*Fmesh.vertices.Length+i+1;
						t2s[(j*(Fmesh.vertices.Length-1)+i)*6+3]=(j+1)*Fmesh.vertices.Length+i+1;
						t2s[(j*(Fmesh.vertices.Length-1)+i)*6+4]=j*Fmesh.vertices.Length+i+1;
						t2s[(j*(Fmesh.vertices.Length-1)+i)*6+5]=j*Fmesh.vertices.Length+i;
					}	  			

				}
			}
	}
	///<summary>令拖影不可见</summary>
	public void setUnVisual()
	{
		mr.enabled=false;
	}
}

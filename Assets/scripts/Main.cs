using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Main : MonoBehaviour {

	// Use this for initialization

	private Material _mat;
	[Range(1,30)]
	public int col;//lie
	[Range(1,30)]
	public int row;//hang
	[Range(0,1)]
	public float speed;

	private Vector2 _uv;

	private List<Vector4> _listPoint = new List<Vector4>();
	private List<Color> _listColor = new List<Color> ();
	private Dictionary<string,int> _existKeys = new Dictionary<string, int> ();
	private int currentFrame;

	void Start () {
		_mat = new Material (Shader.Find("Hidden/RectImageEffectShader"));

		for (int i = 0; i < row; i++) {
			for (int j = 0; j < col; j++) {
				_listPoint.Add (new Vector4(i,j,0,0));
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton (0) && currentFrame != Time.frameCount) {
			//Debug.Log (Input.mousePosition);

			currentFrame = Time.frameCount;

			float px = Input.mousePosition.x / Screen.width;
			float py = Input.mousePosition.y / Screen.height;

			_uv.x = px;
			_uv.y = py;

			float xGap = 1.0f / row;
			float yGap = 1.0f / col;

			int xIndex = (int)(px / xGap);
			int yIndex = (int)(py / yGap);

			//add other point
//			AddPoint(xIndex-1,yIndex+1);
			AddPoint(xIndex,yIndex+1);
//			AddPoint(xIndex+1,yIndex+1);
//
			AddPoint(xIndex-1,yIndex);
//			AddPoint(xIndex,yIndex,true);
			AddPoint(xIndex+1,yIndex);
//
//			AddPoint(xIndex-1,yIndex-1);
			AddPoint(xIndex,yIndex-1);
//			AddPoint(xIndex+1,yIndex-1);

		}
	}

	private void AddPoint(int xIndex,int yIndex,bool isShow = false){
		if (xIndex >= 0 && xIndex < row) {
			if (yIndex >= 0 && yIndex < col) {

				int temp;
				for (int j = 0; j < _listPoint.Count; j++) {
					Vector4 item = _listPoint[j];
					string existKey = xIndex + "_" + yIndex;
					if (item.x == xIndex && item.y == yIndex) {
						int showSign = isShow ? 1 : (Random.Range(0,100) > 65 ? 1 : 0);
						if(showSign == 1 && !_existKeys.TryGetValue(existKey,out temp)){
							item.w = 1;
							item.z = showSign;
							_listPoint [j] = item;
							//Debug.Log (xIndex +","+yIndex);

							_existKeys.Add (existKey, 1);
						}
						break;
					}
				}
			}
		}
	}


	void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (_mat == null) {
			Graphics.Blit (source, destination);
			return;
		}
		RenderTexture rt = RenderTexture.GetTemporary(Screen.width, Screen.height, 0,RenderTextureFormat.ARGB32); 

		for (int i = _listPoint.Count - 1; i >= 0 ; i--) {
			Vector4 item = _listPoint [i];
			if (item.z == 1) {
				item.w -= speed;
				if (item.w <= 0) {
					item.w = 0;
					item.z = 0;

					string key = item.x + "_" + item.y;
					if (_existKeys.ContainsKey (key)) {
						_existKeys.Remove (key);
					}
				}
				_listPoint [i] = item;
			}
		}

		_mat.SetFloat ("_Row", row);
		_mat.SetFloat ("_Col", col);

		_mat.SetFloat ("_Speed", speed);

		_mat.SetFloat ("_MouseUV_X", _uv.x);
		_mat.SetFloat ("_MouseUV_Y", _uv.y);

		if (_listPoint.Count > 0) {
			_mat.SetVectorArray ("_ListPoint", _listPoint);
		}
		_mat.SetFloat ("_Length", _listPoint.Count);
		//Debug.Log (_listPoint.Count);

		Graphics.Blit(source, rt, _mat, 0);

		Graphics.Blit(rt, destination);

		RenderTexture.ReleaseTemporary (rt);
	}

}

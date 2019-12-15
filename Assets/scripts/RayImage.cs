using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayImage : MonoBehaviour
{
    // Start is called before the first frame update
    
	private Material _mat;
	[Range(1,100)]
	public int row;//hang

    public float minSpeed;
    public float maxSpeed;

    public float minWidth;
    public float maxWidth;

    public float radius = 0.2f;
    private int currentFrame;

    private int _xIndex = -1;

    private List<float> _listSpeed = new List<float>();
    private List<float> _listLenList = new List<float>();

    private List<float> _listItemSpeedList = new List<float>();

    private Vector4 _currentPos = Vector4.zero;

    void Start()
    {
        _mat = new Material (Shader.Find("Jim/RayImageEffectShader"));

        for (int i = 0; i < row; i++) {
            _listSpeed.Add(Random.Range(0.0f,1.0f));
            _listLenList.Add(Random.Range(minWidth,maxWidth));
            _listItemSpeedList.Add(Random.Range(minSpeed,maxSpeed));
		}
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton (0)) {
            if(currentFrame != Time.frameCount){
                currentFrame = Time.frameCount;

                float px = Input.mousePosition.x / Screen.width;
                float py = Input.mousePosition.y / Screen.height;

                _currentPos.x = px;
                _currentPos.y = py;
            }
		}else{
            _currentPos.x = -1;
            _currentPos.y = -1;
        }
    }

    private string sign;
    void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (_mat == null) {
			Graphics.Blit (source, destination);
			return;
		}

        for (int i = _listSpeed.Count - 1; i >= 0 ; i--) {
			float item = _listSpeed [i];
            float p_speed = _listSpeed [i];
            p_speed += _listItemSpeedList[i];
            if (p_speed >= 1.5f) {
                p_speed= 0;
                 _listLenList[i] = Random.Range(minWidth,maxWidth);
                 _listItemSpeedList[i] = Random.Range(minSpeed,maxSpeed);
            }
            _listSpeed [i] = p_speed;
		}

		RenderTexture rt = RenderTexture.GetTemporary(Screen.width, Screen.height, 0,RenderTextureFormat.ARGB32); 
        RenderTexture rt2 = RenderTexture.GetTemporary(Screen.width, Screen.height, 0,RenderTextureFormat.ARGB32); 

        _mat.SetFloat ("_SpeedLength", _listSpeed.Count);
        _mat.SetFloatArray("_ListSpeed",_listSpeed);
        _mat.SetFloatArray("_listLenList",_listLenList);

		Graphics.Blit(source, rt, _mat, 0);

        //扣除圆圈
        Graphics.Blit(rt, rt2, _mat, 1);

        _mat.SetFloat("_Radius",radius);
        _mat.SetVector("_CenterPoint",_currentPos);

		Graphics.Blit(rt2, destination);

		RenderTexture.ReleaseTemporary (rt);
        RenderTexture.ReleaseTemporary(rt2);
	}

}

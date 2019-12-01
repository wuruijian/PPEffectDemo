using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColImage : MonoBehaviour
{
    // Start is called before the first frame update
    
	private Material _mat;
	[Range(1,100)]
	public int row;//hang

    public float speed;

    private int currentFrame;

    private int _xIndex = -1;

    private int _oldIndex = -1;
    private float _oldLeftIndexAlpha = -1;
    private float _oldRightIndexAlpha = -1;

    private List<float> _listPoint = new List<float>();
    private List<float> _listSpeed = new List<float>();

    private float _yHeight = 0f;
    void Start()
    {
        _mat = new Material (Shader.Find("Jim/ColImageEffectShader"));

        for (int i = 0; i < row; i++) {
			_listPoint.Add(i);
            _listSpeed.Add(0);
		}
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton (0) && currentFrame != Time.frameCount) {
			//Debug.Log (Input.mousePosition);

			currentFrame = Time.frameCount;

			float px = Input.mousePosition.x / Screen.width;
			float py = Input.mousePosition.y / Screen.height;

			float xGap = 1.0f / row;

			_xIndex = (int)(px / xGap);

            _yHeight = py;

            if(_xIndex >= 0 && _xIndex < _listPoint.Count){
                if(_oldIndex != _xIndex){
                    _oldLeftIndexAlpha = Random.Range(1,100) > 70 ? 0.75f : 0;
                    _oldRightIndexAlpha = Random.Range(1,100) > 70 ? 0.90f : 0;
                }
                _oldIndex = _xIndex;
                //left
                if(_xIndex - 1 >= 0){
                    _listSpeed[_xIndex-1] = _oldLeftIndexAlpha;
                }
                //middle
                _listSpeed[_xIndex] = 1;
                //right
                if(_xIndex + 1 < _listPoint.Count){
                    _listSpeed[_xIndex+1] = _oldRightIndexAlpha;
                }
            }
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
			float item = _listPoint [i];
            float p_speed = _listSpeed [i];
            p_speed -= speed;
            if (p_speed <= 0) {
                p_speed= 0;
            }
            _listSpeed [i] = p_speed;
		}

		RenderTexture rt = RenderTexture.GetTemporary(Screen.width, Screen.height, 0,RenderTextureFormat.ARGB32); 

		_mat.SetFloat ("_Row", row);

        _mat.SetFloat ("_XIndex", _xIndex);

        _mat.SetFloat("_yHeight",_yHeight);

        _mat.SetFloat ("_SpeedLength", _listSpeed.Count);
        _mat.SetFloatArray("_ListSpeed",_listSpeed);

		Graphics.Blit(source, rt, _mat, 0);

		Graphics.Blit(rt, destination);

		RenderTexture.ReleaseTemporary (rt);
	}

    // void OnGUI()
    // {
    //     if(GUI.Button(new Rect(0,0,100,100),sign)){

    //     }
    // }



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColImage : MonoBehaviour
{
    // Start is called before the first frame update
    
	private Material _mat;
	[Range(1,100)]
	public int row;//hang

    private int currentFrame;

    private int _xIndex = -1;
    void Start()
    {
        _mat = new Material (Shader.Find("Jim/ColImageEffectShader"));
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
		}
    }


    void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (_mat == null) {
			Graphics.Blit (source, destination);
			return;
		}
		RenderTexture rt = RenderTexture.GetTemporary(Screen.width, Screen.height, 0,RenderTextureFormat.ARGB32); 

		_mat.SetFloat ("_Row", row);

        _mat.SetFloat ("_XIndex", _xIndex);

		// _mat.SetFloat ("_Speed", speed);

		Graphics.Blit(source, rt, _mat, 0);

		Graphics.Blit(rt, destination);

		RenderTexture.ReleaseTemporary (rt);
	}



}

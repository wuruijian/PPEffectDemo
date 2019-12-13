using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public enum SceneMode{
        Default,
        RECT,
        COL,
    }

    public SceneMode mode = SceneMode.Default;
    private SceneMode _oldMode = SceneMode.Default;

    private Dictionary<SceneMode,MonoBehaviour> _allImageEffects = new Dictionary<SceneMode, MonoBehaviour>();

    void Start()
    {
        _allImageEffects.Add(SceneMode.RECT,gameObject.GetComponent<RectImage>());
        _allImageEffects.Add(SceneMode.COL,gameObject.GetComponent<ColImage>());
    }

    private bool _isDown = false;
    void Update()
    {
        if (Input.GetMouseButton (0)) {
            if(!_isDown){
                if(mode == SceneMode.Default){
                    mode = SceneMode.COL;
                }else if(mode == SceneMode.COL){
                    mode = SceneMode.RECT;
                }else{
                    mode = SceneMode.COL;
                }
            }

            _isDown = true;

        }else{
            if(_isDown){
                _isDown = false;
            }
        }
        if(_oldMode != mode){
            _oldMode = mode;

            foreach (KeyValuePair<SceneMode,MonoBehaviour> item in _allImageEffects)
            {
                if(item.Key == mode){
                    item.Value.enabled = true;
                }else{
                    item.Value.enabled = false;
                }
            }

            switch (mode)
            {
                case SceneMode.RECT:
                    
                    break;
                case SceneMode.COL:
                    
                    break;
                default:
                    break;
            }
        }   
    }
}

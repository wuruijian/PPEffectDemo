using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Game : MonoBehaviour
{
    public enum SceneMode{
        RECT = 1,
        COL = 2,
        RAY = 3,
        STANDBY = 4,
    }

    public SceneMode mode = SceneMode.STANDBY;
    private SceneMode _oldMode = SceneMode.STANDBY;

    private int _delayTime = 60;

    private Dictionary<SceneMode,MonoBehaviour> _allImageEffects = new Dictionary<SceneMode, MonoBehaviour>();

    public AudioSource bgSound;

    void Awake()
    {
        Application.targetFrameRate = 60;
        Application.runInBackground = true;
        Screen.SetResolution(1920,1080,true);
    }
    void Start()
    {
        _allImageEffects.Add(SceneMode.RECT,gameObject.GetComponent<RectImage>());
        _allImageEffects.Add(SceneMode.COL,gameObject.GetComponent<ColImage>());
        _allImageEffects.Add(SceneMode.RAY,gameObject.GetComponent<RayImage>());
        _allImageEffects.Add(SceneMode.STANDBY,gameObject.GetComponent<RayImage2>());

        _delayTime = int.Parse( File.ReadAllText(Application.streamingAssetsPath + "/config.txt"));

        StartCoroutine("LoadSound");

        Cursor.visible = false;
        _oldMousePos = Input.mousePosition;
    }

    IEnumerator LoadSound()
    {
        WWW soundWWW = new WWW("file://" + Application.streamingAssetsPath + "/bg.ogg");
        yield return soundWWW;
        if(soundWWW.isDone && soundWWW.error == null){
            bgSound.clip = soundWWW.GetAudioClip(false,true);
            bgSound.Play();
        }
    }

    private bool _isDown = false;

    private int _totalNoTouchFrame = 0;

    private SceneMode _lastGameMode = SceneMode.STANDBY;
    private Vector3 _oldMousePos = Vector3.zero;

    public static bool isMove = false;
    void Update()
    {
        if(Input.GetKey(KeyCode.Escape)){
            Application.Quit();
        }

        if(Vector3.Equals(_oldMousePos,Input.mousePosition)){
            Game.isMove = false;
            OnUp();
        }else{
            _oldMousePos = Input.mousePosition;
            Game.isMove = true;
            OnDown();
        }

        // if (Input.GetMouseButton (0)) {

        // }else{

        // }
        if(_oldMode != mode){
            _oldMode = mode;
            if(mode != SceneMode.STANDBY){
                _lastGameMode = mode;
            }
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

    void OnDown(){
        _totalNoTouchFrame = 0;
        if(!_isDown){
            if(mode == SceneMode.STANDBY){
                if(_lastGameMode == SceneMode.COL){
                    mode = SceneMode.RECT;
                }else if(_lastGameMode == SceneMode.RECT){
                    mode = SceneMode.COL;
                }else{
                    mode = SceneMode.RECT;
                }
            }
        }
        _isDown = true;
    }

    void OnUp(){
        if(_isDown){
            _isDown = false;
        }
        _totalNoTouchFrame++;
        if(_totalNoTouchFrame >= _delayTime * 60){
            _totalNoTouchFrame = 0;
            mode = SceneMode.STANDBY;
        }
    }
}

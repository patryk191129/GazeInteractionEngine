using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelpers;



namespace GazeInteractionEngine
{


    public class GazeInteractionManager : MonoSingleton<GazeInteractionManager>
    {
        private Camera _sceneCamera;
        private CalibrationDemo _calibrationDemo;

        private LineRenderer _heading;

        [SerializeField]
        private Vector3 _standardViewportPoint = new Vector3(0.5f, 0.5f, 10);

        private Vector2 _gazePointLeft;
        private Vector2 _gazePointRight;
        private Vector2 _gazePointCenter;
        private Vector3 _viewportPoint;
        private Vector3 _gazeWorldPosition;


        private Vector3 _gazeEmulateNoise;
        private Vector3 _gazeEmulatePosition;

        public LayerMask layerMask;

        public bool GazeEmulate = true;
        private IGazeListener _currentListener;



        public Vector2 ViewPortPoint
        {
            get { return _viewportPoint; }
        }

        public Vector3 gazeWorldPosition
        {
            get { return _gazeWorldPosition; }
        }


        void Start()
        {
            PupilData.calculateMovingAverage = false;

            _sceneCamera = gameObject.GetComponent<Camera>();
            _calibrationDemo = gameObject.GetComponent<CalibrationDemo>();
            _heading = gameObject.GetComponent<LineRenderer>();
        }

        void OnEnable()
        {
            if (PupilTools.IsConnected)
            {
                PupilTools.IsGazing = true;
                PupilTools.SubscribeTo("gaze");
            }
        }

        void Update()
        {

            if (PupilTools.IsConnected && PupilTools.IsGazing)
            {
                _gazePointLeft = PupilData._2D.GetEyePosition(_sceneCamera, PupilData.leftEyeID);
                _gazePointRight = PupilData._2D.GetEyePosition(_sceneCamera, PupilData.rightEyeID);
                _gazePointCenter = PupilData._2D.GazePosition;

                Vector3 gazeTargetPos = new Vector3(_gazePointCenter.x, _gazePointCenter.y, 10.0f);
                _viewportPoint = Vector3.Lerp(_viewportPoint, gazeTargetPos, 30.0f * Time.deltaTime);

            }
            else
            {
                //Vector3 gazeNoise = new Vector3(Random.Range(-0.06f, 0.06f), Random.Range(-0.06f, 0.06f), 0);

                if (GazeEmulate)
                {
                    Vector3 tmpViewPort = Input.mousePosition;
                    tmpViewPort = new Vector3(tmpViewPort.x / Screen.currentResolution.width, tmpViewPort.y / Screen.currentResolution.height, 10);                    
                    _viewportPoint = Vector3.Lerp(_viewportPoint, tmpViewPort, 250.0f * Time.deltaTime);

                }                    
                //else
                    //_viewportPoint = Vector3.Lerp(_viewportPoint, _standardViewportPoint + gazeNoise, 25.0f * Time.deltaTime);

            }

            if (Input.GetKeyUp(KeyCode.L))
                _heading.enabled = !_heading.enabled;


            _heading.SetPosition(0, _sceneCamera.transform.position - _sceneCamera.transform.up);


            float thickness = .04f;

            Ray ray = _sceneCamera.ViewportPointToRay(_viewportPoint);
            RaycastHit hit;
            if (Physics.SphereCast(ray, thickness, out hit, Mathf.Infinity, layerMask))
            {
                _gazeWorldPosition = hit.point;



                if (hit.transform.GetComponent<IGazeListener>() != null)
                {
                    IGazeListener listener = hit.transform.GetComponent<IGazeListener>();
                    listener.OnEvent(EVENT_TYPE.ON_EYE_INTERACTION, this, null);

                    if (_currentListener != null && _currentListener != listener)
                        try
                        {
                            _currentListener.OnEvent(EVENT_TYPE.ON_EYE_STOP_INTERACTION, this, null);
                        }
                        catch { }

                    _currentListener = listener;                   

                }
                else
                {
                    if (_currentListener != null)
                    {
                        try
                        {
                            _currentListener.OnEvent(EVENT_TYPE.ON_EYE_STOP_INTERACTION, this, null);
                        }
                        catch { }
                        _currentListener = null;
                    }
                }

                _heading.SetPosition(1, hit.point);
            }
            else
            {
                _gazeWorldPosition = Vector3.positiveInfinity;

                _heading.SetPosition(1, ray.origin + ray.direction * 50f);

                if (_currentListener != null)
                {
                    try
                    {
                        _currentListener.OnEvent(EVENT_TYPE.ON_EYE_STOP_INTERACTION, this, null);
                    }
                    catch
                    {

                    }
                        _currentListener = null;
                }
            }
        }      


    }
}
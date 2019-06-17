using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelpers;
using UnityEngine.UI;


namespace GazeInteractionEngine
{



    public class GazeInteractionEngineDebug : MonoBehaviour
    {

        public Text gaze2D;
        public Text gaze3D;
        public Text gaze3DLeft;
        public Text gaze3DRight;

        public Text leftEye;
        public Text rightEye;



        // Update is called once per frame
        void Update()
        {
            Vector2 data2D = PupilData._2D.GazePosition;
            Vector3 data3D = PupilData._3D.GazePosition;

            Vector3 leftEyePos = PupilData._2D.LeftEyePosition;
            Vector3 rightEyePos = PupilData._2D.RightEyePosition;

            Vector3 leftGazeNormal = PupilData._3D.LeftGazeNormal;
            Vector3 rightGazeNormal = PupilData._3D.RightGazeNormal;
            
            gaze2D.text = "x="+data2D.x+",y="+data2D.y;
            gaze3D.text = "x=" + data3D.x + ",y=" + data3D.y + ",z=" + data3D.z;
            gaze3DLeft.text = "x=" + leftGazeNormal.x + ",y=" + leftGazeNormal.y + ",z=" + leftGazeNormal.z;
            gaze3DRight.text = "x= " + rightGazeNormal.x + "y=" + rightGazeNormal.y + "z=" + rightGazeNormal.z;
            leftEye.text = "x=" + leftEyePos.x + ",y=" + leftEyePos.y + ",z=" + leftEyePos.z;
            rightEye.text = "x=" + rightEyePos.x + ",y=" + rightEyePos.y + ",z=" + rightEyePos.z;

        }
    }


}

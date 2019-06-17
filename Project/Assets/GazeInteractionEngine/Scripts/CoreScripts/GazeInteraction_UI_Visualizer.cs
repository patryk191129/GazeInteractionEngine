using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelpers;
using UnityEngine.UI;

namespace GazeInteractionEngine
{

    public class GazeInteraction_UI_Visualizer : MonoBehaviour
    {

        public RectTransform gazeIcon;
        private RectTransform _canvasTransform;
        private Vector2 _rectDimensions;


        private void Start()
        {
            _canvasTransform = this.GetComponent<RectTransform>();
        }


        private void Update()
        {
            Vector2 gazePosition = GazeInteractionManager.instance.ViewPortPoint;
            _rectDimensions = _canvasTransform.rect.size;
            gazeIcon.position = new Vector2(_rectDimensions.x * gazePosition.x, _rectDimensions.y * gazePosition.y);
        }


    }

}
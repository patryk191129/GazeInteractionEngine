using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GazeInteractionEngine
{

    public enum EVENT_TYPE
    {
        ON_EYE_INTERACTION,
        ON_EYE_STOP_INTERACTION,

        BROADCAST_OBJECT_START_INTERACTION,
        BROADCAST_OBJECT_END_INTERACTION,
        BROADCAST_BUTTON_PRESSED,
    }

    public interface IGazeListener
    {
        void OnEvent(EVENT_TYPE eventType, Component Sender, Object param = null);
    }

}
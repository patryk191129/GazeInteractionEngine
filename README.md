# GazeInteractionEngine

## Introduction
Virtual reality systems are computer-generated environments where an user experiences sensations perceived by the human senses. These systems are based
primarily on providing video and audio signals, and offer the opportunity to
interact directly with the created scene with the help of touch or other form
of manipulation using hands. Vision systems for virtual reality environments
consist most frequently of head-mounted goggles, which are equipped with two
liquid crystal displays placed opposite the eyes in a way that enables stereoscopic
vision. The image displayed in the helmet is rendered independently for the left
and right eye, and then combined into the stereopair. More and more solutions
appear on the market, and the most popular include: HTC Vive, Oculus Rift
CV1, Playstation VR (dedicated for Sony Playstation 4), Google Cardboard
(dedicated for Android mobile devices).


## Gaze-driven ray-cast interface concept
The concept of using an eye tracker for steering in the virtual reality environment
assumes the usage of the eye focus point to interact with objects of the virtual
scene. An overview of the system built upon this concept is presented in Fig. 1
and the process of interaction consists of the following steps:
– mapping the direction of the user’s eye focus on the screen coordinates,
– generating a primary ray (raycasting using the sphere) from the coordinates
of the user’s eye focus direction,
– intersection analysis with scene objects,
– indication of the object pointed by the sight,
– handling the event associated with the object,
– rendering a stereopair for virtual reality goggles.



[![Gaze typing](https://i.imgur.com/dZZmnv2.png)](https://youtu.be/dmeprfAubg0)
[![Gaze typing](https://i.imgur.com/Aseo1yV.jpg)](https://www.youtube.com/watch?v=GUtP3QCAoaQ)
[![Gaze typing](https://i.imgur.com/kgl4Nzv.jpg)](https://youtu.be/YZsHS-JFCzs?t=2246)


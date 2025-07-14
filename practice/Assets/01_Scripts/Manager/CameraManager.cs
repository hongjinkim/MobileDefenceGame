using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : BasicSingleton<CameraManager>
{
    public Camera mainCamera;
    public Camera uiCamera;
    public Camera debugCamera;
}

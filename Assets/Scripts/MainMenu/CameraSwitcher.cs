using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSwitcher : MonoBehaviour
{
    public CinemachineVirtualCamera[] virtualCameras;
    private int currentCameraIndex;

    void Start()
    {
        currentCameraIndex = 0;
        ShowCurrentCamera();
    }

    public void SwitchCamera(int cameraIndex)
    {
        if (cameraIndex >= 0 && cameraIndex < virtualCameras.Length)
        {
            currentCameraIndex = cameraIndex;
            ShowCurrentCamera();
        }
    }

    void ShowCurrentCamera()
    {
        for (int i = 0; i < virtualCameras.Length; i++)
        {
            if (i == currentCameraIndex)
            {
                virtualCameras[i].Priority = 20;
                Debug.Log("Activating Camera: " + i);
            }
            else
            {
                virtualCameras[i].Priority = 10;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEntrance : MonoBehaviour
{
    [SerializeField] private string transitionName;

    private void Start()
    {
        if (SceneManagement.Instance == null)
        {
            Debug.LogError("SceneManagement.Instance is null! Check if SceneManagement is in the scene.");
            return;
        }

        if (PlayerController.Instance == null)
        {
            Debug.LogError("PlayerController.Instance is null! Ensure PlayerController is properly initialized.");
            return;
        }

        if (transitionName == SceneManagement.Instance.SceneTransitionName)
        {
            PlayerController.Instance.transform.position = this.transform.position;
            CameraController.Instance.SetPlayerCameraFollow();
            UIFade.Instance.FadeToWhite();
        }
    }

}

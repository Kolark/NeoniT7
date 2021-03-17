using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEditor;
public class CameraController : MonoBehaviour
{
    private static CameraController instance;
    public static CameraController Instance { get => instance; }

    CinemachineVirtualCamera cinemachineVirtualCamera;
    CinemachineConfiner cinemachineConfiner;
    Transform toFollow;
    private void Awake()
    {
        #region singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        #endregion
        cinemachineVirtualCamera = transform.GetChild(1).GetComponent<CinemachineVirtualCamera>();
        cinemachineConfiner = transform.GetChild(1).GetComponent<CinemachineConfiner>();
    }
    /// <summary>
    /// Waits for player instance to exist
    /// </summary>
    /// <returns></returns>
    IEnumerator Start()
    {
        yield return new WaitUntil(() => BasicCharacter.Instance != null);
        toFollow = BasicCharacter.Instance.transform;
        cinemachineVirtualCamera.Follow = toFollow;
    }

    /// <summary>
    /// Called when there is a chamber Change therefore the current cameraconfiner needs to change
    /// </summary>
    /// <param name="compositeCollider2D"></param>
    public void ChangeConfiner(CompositeCollider2D compositeCollider2D)
    {
        cinemachineConfiner.m_BoundingShape2D = compositeCollider2D;
    }
}

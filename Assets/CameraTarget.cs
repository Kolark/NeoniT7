using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraTarget : MonoBehaviour
{
    CinemachineVirtualCamera virtualCamera;
    Collider2D col2d;
    int chamberIndex = 0;
    //public InitCameraControllerInfo info;
    private void Awake()
    {
        col2d = GetComponent<Collider2D>();
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }
    public void setIndex(int i)
    {
        chamberIndex = i;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //BasicCharacter basicCharacter = collision.GetComponent<BasicCharacter>();
        if (collision.CompareTag("Player"))
        {
            CameraController.Instance.LowerAllCamerasPriority();
            virtualCamera.Priority = 11;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            bool updatedChamber = CameraController.Instance.updateChamber(chamberIndex);
            col2d.isTrigger = !updatedChamber;

        }
    }

#if UNITY_EDITOR
     public void AdjustTarget(InitCameraControllerInfo info,float scale)
    {
        BoxCollider2D boxCollider2D = GetComponent<BoxCollider2D>();
        CinemachineVirtualCamera virtualCamera = GetComponent<CinemachineVirtualCamera>();
        virtualCamera.Priority = 10;
        float proportions = (float)Screen.width / (float)Screen.height;
        Camera main = transform.parent.parent.GetComponentInChildren<Camera>();
        virtualCamera.m_Lens.OrthographicSize = (info.width / (main.aspect * 2))*scale;
        boxCollider2D.size = new Vector2(info.width*scale, info.height*scale);
    }
#endif
}

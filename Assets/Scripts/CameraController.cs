using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraController : MonoBehaviour
{
    private static CameraController instance;
    public static CameraController Instance { get => instance; }

    private int chambersUnlocked = 1;

    Transform[] positions;
    CinemachineVirtualCamera[] cameras;
    CameraTarget[] targets;

    [SerializeField] float scale = 1f;
    [SerializeField]InitCameraControllerInfo info;
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
        positions = new Transform[transform.GetChild(0).childCount];
        cameras = new CinemachineVirtualCamera[transform.GetChild(0).childCount];
        targets = new CameraTarget[transform.GetChild(0).childCount];
        for (int i = 0; i < positions.Length; i++)
        {
            positions[i] = transform.GetChild(0).GetChild(i);
            cameras[i] = positions[i].GetComponent<CinemachineVirtualCamera>();
            targets[i] = positions[i].GetComponent<CameraTarget>();
            targets[i].setIndex(i + 1);
        }
    }
    public void ChangePriority()
    {
        cameras[chambersUnlocked - 1].Priority = 11;
    }
    public bool updateChamber(int exitChamber)
    {
        bool chamberStatus = exitChamber == chambersUnlocked;
        if (chamberStatus)
        {
            chambersUnlocked++;
        }
        LowerAllCamerasPriority();
        ChangePriority();
        return chamberStatus;
    }
    public void LowerAllCamerasPriority()
    {
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].Priority = 10;
        }
    }

    Color[] gizmosColors = { Color.red, Color.blue, Color.green, Color.cyan, Color.yellow, Color.magenta };
    private void OnDrawGizmos()
    {
        positions = new Transform[transform.GetChild(0).childCount];
        for (int i = 0; i < positions.Length; i++)
        {
            positions[i] = transform.GetChild(0).GetChild(i);
        }
        
        for (int i = 0; i < positions.Length; i++)
        {
            Gizmos.color = gizmosColors[i % gizmosColors.Length]- new Color(0,0,0,0.5f);
            Gizmos.DrawCube(positions[i].position, new Vector3(info.width*scale, info.height*scale, 0));
        }
    }
    private void OnValidate()
    {
        
        positions = new Transform[transform.GetChild(0).childCount];
        cameras = new CinemachineVirtualCamera[transform.GetChild(0).childCount];
        for (int i = 0; i < positions.Length; i++)
        {
            positions[i] = transform.GetChild(0).GetChild(i);
            cameras[i] = positions[i].GetComponent<CinemachineVirtualCamera>();
        }
        for (int i = 0; i < positions.Length; i++)
        {
            BoxCollider2D boxCollider2D;
            positions[i].position = new Vector3(i * info.spacing*scale,positions[i].position.y, 0);
            boxCollider2D = positions[i].GetComponent<BoxCollider2D>();
            boxCollider2D.size = new Vector2(info.width * scale, info.height* scale);
            Vector3 pos = boxCollider2D.transform.position;
            pos.z = -info.cameraDistance * scale;
            boxCollider2D.transform.position = pos;
        }
        cameras[0].Priority = 11;
    }
    private void OnDestroy()
    {
        if(instance != this)
        {
            instance = null;
        }
    }
}

[System.Serializable]
public struct InitCameraControllerInfo
{
    public float cameraDistance;
    public float spacing;
    public float width;
    public float height;

}
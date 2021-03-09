using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEditor;
public class CameraController : MonoBehaviour
{
    private static CameraController instance;
    public static CameraController Instance { get => instance; }

    private int chambersUnlocked = 1;

    Transform[] positions;
    CinemachineVirtualCamera[] cameras;
    CameraTarget[] targets;

    [SerializeField] float scale = 1f;
    [SerializeField] List<InitCameraControllerInfo> infos;



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
        for (int i = 0; i < infos.Count; i++)
        {
            Transform pos = transform.GetChild(0).GetChild(i);
            if (pos != null)
            {
                Gizmos.color = gizmosColors[i % gizmosColors.Length] - new Color(0, 0, 0, 0.5f);
                Gizmos.DrawCube(pos.position, new Vector3(infos[i].width*scale, infos[i].height*scale, 0));
                Handles.color = Color.white;
                Handles.Label(pos.position, "Section: " + i);
            }

        }

    }


    private void OnValidate()
    {
        int length = transform.GetChild(0).childCount;
        #region RemoveAddSections
        if (length != infos.Count)
        {
            int diff = infos.Count - length;
            if (diff > 0)//Add Cameras
            {
                Object prefab = AssetDatabase.LoadAssetAtPath($"Assets/Prefabs/CameraSection.prefab", typeof(GameObject));
                for (int i = 0; i < diff; i++)
                {
                    GameObject toInstatiate = PrefabUtility.InstantiatePrefab(prefab, transform.GetChild(0)) as GameObject;
                }
            }
            else//Remove Cameras
            {
                for (int i = 0; i < Mathf.Abs(diff); i++)
                {
                    #if UNITY_EDITOR
                    UnityEditor.EditorApplication.delayCall += () =>
                    {
                        try
                        {
                            DestroyImmediate(transform.GetChild(0).GetChild(transform.GetChild(0).childCount - 1).gameObject);
                        }
                        catch (System.Exception)
                        {

                            throw;
                        }
                        
                    };
                    #endif
                    //DestroyImmediate();
                }
            }
        }
        #endregion
        length = transform.GetChild(0).childCount;
        positions = new Transform[length];
        CameraTarget[] cameraTargets = new CameraTarget[length];
        
        for (int i = 0; i < positions.Length; i++)
        {
            positions[i] = transform.GetChild(0).GetChild(i);
            cameraTargets[i] = positions[i].GetComponent<CameraTarget>();
        }
        float Xpos = positions[0].position.x;
        for (int i = 1; i < positions.Length; i++)
        {
            Xpos += (infos[i].width *scale)/ 2 + (infos[i - 1].width*scale)/ 2;
            positions[i].position = new Vector3(Xpos, positions[i].position.y, -10);
        }
        for (int i = 0; i < infos.Count; i++)
        {
            cameraTargets[i].AdjustTarget(infos[i],scale);
        }
        CinemachineVirtualCamera firstCamera = transform.GetChild(0).GetChild(0).GetComponent<CinemachineVirtualCamera>();
        firstCamera.Priority = 11;




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
    //public float cameraDistance;
    //public float spacing;
    public float width;
    public float height;

}
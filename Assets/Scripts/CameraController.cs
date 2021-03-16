using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEditor;
public class CameraController : MonoBehaviour
{
    private static CameraController instance;
    public static CameraController Instance { get => instance; }
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
    }

    public bool updateChamber(int exitChamber)
    {
        
        return false;
    }
}

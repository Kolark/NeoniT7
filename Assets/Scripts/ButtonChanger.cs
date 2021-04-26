using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonChanger : MonoBehaviour
{
    Transform[] keys;
    private void Awake()
    {
        keys = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            keys[i] = transform.GetChild(i);
        }
    }
    private void Start()
    {
        InputController.Instance.OnControlChanged += onControlsChanged;
        onControlsChanged(InputController.Instance.CurrentControlScheme);
    }

    public void onControlsChanged(ControllerType type)
    {
        Debug.Log("type: " + type.ToString());
        for (int i = 0; i < keys.Length; i++)
        {
            keys[i].gameObject.SetActive(false);
        }
        keys[(int)type].gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        InputController.Instance.OnControlChanged -= onControlsChanged;
    }
}

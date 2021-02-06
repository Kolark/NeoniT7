using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class DeveloperMode : MonoBehaviour {

    //This script should methods that reference static controllers/managers
    //and be able to modify them to grant developers an advantage in terms of testing.

    [UnityEditor.MenuItem("Tools/Information")]
    public static void TestFunction() {
        Debug.Log("This is a test developer function.");
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
public class ClickHandler2 : MonoBehaviour, IPointerClickHandler
{
	public enum EnumExample
	{
		FIRST,
		SECOND,
		THIRD
	}

	[System.Serializable]
	public class PointerEvents2 : UnityEvent2<PointerEventData> { }

	/// <summary>
	/// 
	/// </summary>
	[SerializeField]
	private Text m_LogText = null;

	/// <summary>
	/// 
	/// </summary>
	[SerializeField]
	[Tooltip("Regular events (This tooltip will appear on Inspector)")]
	private UnityEvent2 unityEvents2 = null;

	/// <summary>
	/// 
	/// </summary>
	[SerializeField]
	[Tooltip("PointerEventData dynamic events (This tooltip will appear on Inspector)")]
	private PointerEvents2 pointerEvents2 = null;

	/// <summary>
	/// 
	/// </summary>
	/// <param name="message"></param>
	/// <param name="args"></param>
	void Log(string message, params object[] args)
	{
		Debug.LogFormat(message, args);
		m_LogText.text += string.Format(message, args);
		m_LogText.text += System.Environment.NewLine;
		m_LogText.text += System.Environment.NewLine;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="eventData"></param>
	public void OnPointerClick(PointerEventData eventData)
	{
		unityEvents2.Invoke();
		pointerEvents2.Invoke(eventData);
	}

	public void Test()
	{
		Log("Test method. No params");
	}

	public void Test(int i1, int i2, int i3)
	{
		Log("Test method. int param1:{0} - int param2:{1} - int param3:{2}", i1, i2, i3);
	}

	public void Test(string s1, string s2, string s3)
	{
		Log("Test method. string param1:{0} - string param2:{1} - string param3:{2}", s1, s2, s3);
	}

	public void Test(EnumExample enumExample1, EnumExample enumExample2, EnumExample enumExample3)
	{
		Log("Test method. enum param1:{0} - enum param2:{1} - enum param3:{2}", enumExample1, enumExample2, enumExample3);
	}

	public void Test(GameObject go1, GameObject go2)
	{
		Log("Test method. GameObject param1: {0} - GameObject param2: {1}", go1, go2);
	}

	public void Test(int i, string s, GameObject go, EnumExample enumExample)
	{
		Log("Test method. int: {0} - string: {1} - GameObject: {2} - Enum: {3}", i, s, go, enumExample);
	}

	public void Test(Vector2 v)
	{
		Log("Test method. Vector2 param:{0}", v);
	}

	public void Test(Vector3 v)
	{
		Log("Test method. Vector3 param:{0}", v);
	}

	public void Test(Vector4 v)
	{
		Log("Test method. Vector4 param:{0}", v);
	}

	public void TestLayer([Layer] int layer)
	{
		Log("TestLayer method. layer index:{0} - name:{1}", layer, LayerMask.LayerToName(layer));
	}

	[Layer("Ignore Raycast")]
	public void TestLayer(int layer1, [Layer(0)] int layer2)
	{
		Log("TestLayer method. layer1 index:{0} - name:{1} - layer2 index:{2} - name:{3}", layer1, LayerMask.LayerToName(layer1), layer2, LayerMask.LayerToName(layer2));
	}

	public void TestColor(Color color)
	{
		Log("TestColor method. color param:{0}", color);
	}

	public void TestLayerMask(LayerMask layerMask)
	{
		string layers = string.Empty;
		for (int i = 0; i < 32; i++)
		{
			if ((layerMask.value & 1 << i) > 0 && !string.IsNullOrEmpty(LayerMask.LayerToName(i)))
				layers += LayerMask.LayerToName(i) + ", ";
		}
		Log("TestLayerMask method. Selected layers from layerMask:{0}", layers);
	}

	public void Test(PointerEventData eventData)
	{
		Log("Test method. Dynamic PointerEventData param:{0}", eventData);
	}

	public void Test(PointerEventData eventData, string s)
	{
		Log("Test method. string param:{1} - Dynamic PointerEventData param:{0}", eventData, s);
	}

	public void Test(PointerEventData eventData, int i)
	{
		Log("Test method. int param:{1} - Dynamic PointerEventData param:{0}", eventData, i);
	}

	// THE METHODS BELOW ARE NOT AVAILABLE TO CHOOSE
	public void Test(ArrayList list)
	{
		Log("Test method. list param:{0}", list);
	}

	public void Test(List<Object> list)
	{
		Log("Test method. list param:{0}", list);
	}

	// HELPER PROPERTIES

	/// <summary>
	/// Created for <see cref="Test(int, int, int)"/>
	/// </summary>
	private UpdatableInvokableCall<int, int, int> _Unused0 { get; set; }

	/// <summary>
	/// Created for <see cref="Test(EnumExample, EnumExample, EnumExample)"/>
	/// </summary>
	private UpdatableInvokableCall<EnumExample, EnumExample, EnumExample> _Unused1 { get; set; }

	/// <summary>
	/// Created for <see cref="Test(int, string, GameObject, EnumExample)"/>
	/// </summary>
	private UpdatableInvokableCall<int, string, GameObject, EnumExample> _Unused2 { get; set; }

	/// <summary>
	/// Created for <see cref="TestLayer(int, int)"/>
	/// </summary>
	private UpdatableInvokableCall<int, int> _Unused3 { get; set; }

	/// <summary>
	/// Created for <see cref="TestColor(Color)"/>
	/// </summary>
	private UpdatableInvokableCall<Color> _Unused4 { get; set; }

	/// <summary>
	/// Created for <see cref="TestLayerMask(LayerMask)"/>
	/// </summary>
	private UpdatableInvokableCall<LayerMask> _Unused5 { get; set; }

	/// <summary>
	/// Created for <see cref="Test(PointerEventData, string)"/>
	/// </summary>
	private UpdatableInvokableCall<PointerEventData, string> _Unused6 { get; set; }

	/// <summary>
	/// Created for <see cref="Test(PointerEventData, int)"/>
	/// </summary>
	private UpdatableInvokableCall<PointerEventData, int> _Unused7 { get; set; }
}
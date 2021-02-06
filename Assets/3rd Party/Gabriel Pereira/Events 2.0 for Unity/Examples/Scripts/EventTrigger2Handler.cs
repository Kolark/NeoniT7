using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EventTrigger2Handler : MonoBehaviour
{
	/// <summary>
	/// 
	/// </summary>
	[SerializeField]
	private Text m_LogText = null;

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

	public void OnPointerEnter(BaseEventData eventData, string s)
	{
		Log("OnPointerEnter - string: {1}, eventData: {0}", (PointerEventData)eventData, s);
	}

	public void OnPointerExit(BaseEventData eventData, string s)
	{
		Log("OnPointerExit - string: {1}, eventData: {0}", (PointerEventData)eventData, s);
	}

	public void OnPointerDown(BaseEventData eventData, string s)
	{
		Log("OnPointerDown - string: {1}, eventData: {0}", (PointerEventData)eventData, s);
	}

	public void OnPointerUp(BaseEventData eventData, string s)
	{
		Log("OnPointerDown - string: {1}, eventData: {0}", (PointerEventData)eventData, s);
	}

	public void OnPointerClick(BaseEventData eventData, string s)
	{
		Log("OnPointerClick - string: {1}, eventData: {0}", (PointerEventData)eventData, s);
	}

	public void OnDrag(BaseEventData eventData, string s)
	{
		Log("OnDrag - string: {1}, eventData: {0}", (PointerEventData)eventData, s);
	}

	public void OnDrop(BaseEventData eventData, string s)
	{
		Log("OnDrop - string: {1}, eventData: {0}", (PointerEventData)eventData, s);
	}

	public void OnScroll(BaseEventData eventData, string s)
	{
		Log("OnScroll - string: {1}, eventData: {0}", (PointerEventData)eventData, s);
	}

	public void OnUpdateSelected(BaseEventData eventData, string s)
	{
		Log("OnUpdateSelected - string: {1}, eventData: {0}", eventData, s);
	}

	public void OnSelect(BaseEventData eventData, string s)
	{
		Log("OnSelect - string: {1}, eventData: {0}", eventData, s);
	}

	public void OnDeselect(BaseEventData eventData, string s)
	{
		Log("OnDeselect - string: {1}, eventData: {0}", eventData, s);
	}

	public void OnMove(BaseEventData eventData, string s)
	{
		Log("OnMove - string: {1}, eventData: {0}", (AxisEventData)eventData, s);
	}

	public void OnInitializePotentialDrag(BaseEventData eventData, string s)
	{
		Log("OnInitializePotentialDrag - string: {1}, eventData: {0}", (PointerEventData)eventData, s);
	}

	public void OnBeginDrag(BaseEventData eventData, string s)
	{
		Log("OnBeginDrag - string: {1}, eventData: {0}", (PointerEventData)eventData, s);
	}

	public void OnEndDrag(BaseEventData eventData, string s)
	{
		Log("OnEndDrag - string: {1}, eventData: {0}", eventData, s);
	}

	public void OnSubmit(BaseEventData eventData, string s)
	{
		Log("OnSubmit - string: {1}, eventData: {0}", eventData, s);
	}

	public void OnCancel(BaseEventData eventData, string s)
	{
		Log("OnCancel - string: {1}, eventData: {0}", eventData, s);
	}
}
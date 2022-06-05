using UnityEngine;
using TMPro;

public class ConsoleView : MonoBehaviour {

	private ConsoleController console; 

	bool didShow = false;

	public GameObject viewContainer; // should be a child of this GameObject
	public TMP_Text logTextArea;
	public TMP_InputField inputField;


	void Start()
    {
		console = new ConsoleController();
		
		console.visibilityChanged += OnVisibilityChanged;
		console.logChanged += OnLogChanged;
		
		UpdateLogString(console.log);
		ToggleVisibility(); // begin off
	}

    // Update is called once per frame
    void Update()
    {
		//Toggle visibility when tilde key pressed
		if (Input.GetKeyUp("`"))
		{
			ToggleVisibility();
		}
		
	}

	void ToggleVisibility()
	{
		SetVisibility(!viewContainer.activeSelf);
	}

	void SetVisibility(bool visible)
	{
		viewContainer.SetActive(visible);
		if (visible) SetInputFocus();		
	}

	void OnVisibilityChanged(bool visible)
	{
		SetVisibility(visible);
	}

	void OnLogChanged(string[] newLog)
	{
		UpdateLogString(newLog);
	}

	void UpdateLogString(string[] newLog)
	{
		if (newLog == null)
		{
			logTextArea.text = "";
		}
		else
		{
			logTextArea.text = string.Join("\n", newLog);
		}
	}

	/// <summary>
	/// Event that should be called by anything wanting to submit the current input to the console.
	/// </summary>
	public void RunCommand()
	{
		if (string.IsNullOrWhiteSpace(inputField.text)) return;
		console.RunCommandString(inputField.text);
		inputField.text = "";
		SetInputFocus();
	}

	void SetInputFocus()
    {
		inputField.Select();  // reposition cursor
		inputField.ActivateInputField();
	}
}





//~ConsoleView()
//{
//	console.visibilityChanged -= OnVisibilityChanged;
//	console.logChanged -= OnLogChanged;
//}




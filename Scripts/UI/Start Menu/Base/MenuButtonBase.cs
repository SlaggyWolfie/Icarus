using UnityEngine;
using UnityEngine.UI;

public abstract class MenuButtonBase : MonoBehaviour {

    private Button _button;
	// Use this for initialization
	void Awake () {
        _button = gameObject.GetComponent<Button>();
        if ( !_button )
            Debug.LogError("Button component missing for gameObject " + gameObject.name + "!");
        _button.onClick.AddListener(Action);
    }

    public abstract void Action();
}

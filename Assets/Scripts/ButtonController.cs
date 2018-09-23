using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ButtonController : MonoBehaviour {
    public string triggerScene = "";
    private GameObject DialogController;

    // Use this for initialization
    void Start () {
        DialogController = GameObject.Find("DialogController");
        this.GetComponent<Button>().onClick.AddListener(OnClick);
    }
	
	// Update is called once per frame
	void Update () {
        if (DialogController.GetComponent<DialogManager>().DialogBox == null) { 
            this.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            this.transform.Find("Text").GetComponent<Text>().color = new Color(50f / 255, 50f / 255, 50f / 255, 1);
        } else {
            this.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0);
            this.transform.Find("Text").GetComponent<Text>().color = new Color(50f / 255, 50f / 255, 50f / 255, 0);
        }
    }

    void OnClick()
    {
        if (DialogController.GetComponent<DialogManager>().DialogBox == null)
        {
            DialogController.GetComponent<DialogManager>().initDialog(triggerScene);
            
        }
    }
}

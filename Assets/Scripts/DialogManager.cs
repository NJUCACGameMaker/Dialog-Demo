using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    private static DialogManager instance;
    public GameObject DialogPrefab;
    public GameObject DialogBox;             // 对话框+立绘整体
    public GameObject NameText;              // 显示角色姓名的文本框
    public GameObject DialogText;            // 显示对话框文字的文本框
    public GameObject Character;             // 立绘图像

    private Dialog currentDialog;            // 当前对话
    private int id;                          // temp 测试用
    private DialogLoader loader;

    public static DialogManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new DialogManager();
            }
            return instance;
        }
    }

    public void initDialog()
    {
        loader = new DialogLoader();
        loader.loadData();
        DialogBox = Instantiate(DialogPrefab) as GameObject;
        currentDialog = loader.context[0];
        id = 0;
        displayDialog(currentDialog);
    }

    private void displayDialog(Dialog dialog)
    {
        Text nameText = DialogBox.transform.Find("NameText").GetComponent<Text>();
        nameText.text = dialog.characterName;
        Text dialogText = DialogBox.transform.Find("DialogText").GetComponent<Text>();
        dialogText.text = dialog.text;
        Image characterImage = DialogBox.transform.Find("Character").GetComponent<Image>();
        Sprite sp = Resources.Load(dialog.imagePath, typeof(Sprite)) as Sprite;
        Debug.Log(dialog.imagePath);
        characterImage.sprite = sp;
    }

    public void DestoryDiaLog()
    {
        if (DialogBox != null)
            Destroy(DialogBox);
    }

    public bool IsEmptyDialog()
    {
        if (GameObject.Find("Dialogbox(Clone)"))
        {

            return false;
        }
        else
        {
            return true;
        }

    }

    void Start()
    {
        initDialog();
    }

    private void setNextDialog()
    {
        id++;
        if (id >= loader.context.Count)
            id = 0;
        currentDialog = loader.context[id];
        displayDialog(currentDialog);
    }

    private void OnGUI()
    {
        if (Event.current != null && Event.current.type == EventType.MouseDown)
            setNextDialog();
    }
}
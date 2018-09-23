using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    private static DialogManager instance;
    public GameObject DialogPrefab;
    public GameObject DialogBox;               // 对话框+立绘整体
    public double textSpeed = 0.04;            // 文字显示速度
    
    private Dialog currentDialog;              // 当前对话
    private int id;                            // temp 测试用
    private DialogLoader loader;

    private string tempDialog;                 // 逐字显示用
    private bool dialogFlag;                   // 判断是否在逐字显示
    private double timer;
    private bool animationLock;                // 在播放特定动画的时候锁死交互

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
        dialogFlag = false;
        DialogBox.transform.Find("NamePanel").Find("NameText").GetComponent<Text>().text = "";
        animationLock = true;
        StartCoroutine(nameAnimation("", currentDialog.characterName));
        displayDialog(currentDialog);
    }

    private void displayDialog(Dialog dialog)
    {
        Image characterImage = DialogBox.transform.Find("Character").GetComponent<Image>();
        Sprite sp = Resources.Load(dialog.imagePath, typeof(Sprite)) as Sprite;
        Debug.Log(dialog.imagePath);
        characterImage.sprite = sp;
        tempDialog = "";
        timer = 0;
        dialogFlag = true;
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

    void Update()
    {
        if (tempDialog == currentDialog.text)
        {
            dialogFlag = false;
        }
        if (dialogFlag)
        {
            Text dialogText = DialogBox.transform.Find("DialogPanel").Find("DialogText").GetComponent<Text>();
            if (timer > textSpeed)
            {
                timer = 0;
                tempDialog = currentDialog.text.Substring(0, tempDialog.Length + 1);
            }
            dialogText.text = tempDialog;
            timer += Time.deltaTime;
        }
    }

    private IEnumerator nameAnimation(string name1, string name2)
    {
        GameObject replacedName = Instantiate(DialogBox.transform.Find("NamePanel").Find("NameText").gameObject) as GameObject;
        replacedName.transform.position = DialogBox.transform.Find("NamePanel").Find("NameText").position - new Vector3(20f, 0, 0);
        replacedName.GetComponent<Text>().text = name2;
        Color c1 = DialogBox.transform.Find("NamePanel").Find("NameText").GetComponent<Text>().color;
        Color c2 = replacedName.GetComponent<Text>().color;
        c2 = new Color(c2.r, c2.g, c2.b, 0f);
        replacedName.GetComponent<Text>().color = c2;
        replacedName.transform.parent = DialogBox.transform.Find("NamePanel");

        for (float t = 1f; t > 0; t -= 0.05f)
        {
            DialogBox.transform.Find("NamePanel").Find("NameText").transform.position += new Vector3(1f, 0, 0);
            replacedName.transform.position += new Vector3(1f, 0, 0);
            c1.a -= 0.05f;
            c2.a += 0.05f;
            DialogBox.transform.Find("NamePanel").Find("NameText").GetComponent<Text>().color = c1;
            replacedName.GetComponent<Text>().color = c2;
            yield return null;//下一帧继续执行for循环
            yield return new WaitForSeconds(0.006f);//0.006秒后继续执行for循环
        }
        DialogBox.transform.Find("NamePanel").Find("NameText").transform.position -= new Vector3(20f, 0, 0);
        DialogBox.transform.Find("NamePanel").Find("NameText").GetComponent<Text>().color = new Color(c1.r, c1.g, c1.b, 1.0f);
        DialogBox.transform.Find("NamePanel").Find("NameText").GetComponent<Text>().text = name2;
        Destroy(replacedName);
        animationLock = false;
    }

    private void setNextDialog()
    {
        string name1 = currentDialog.characterName;
        id++;
        if (id >= loader.context.Count)
            id = 0;
        currentDialog = loader.context[id];
        if (name1 != currentDialog.characterName)
        {
            animationLock = true;
            StartCoroutine(nameAnimation(name1, currentDialog.characterName));
        }
        displayDialog(currentDialog);
    }

    private void OnGUI()
    {
        if (!animationLock && Event.current != null && Event.current.type == EventType.MouseDown) {
            // 如果当前文字已经全部出现，则进入下一句
            // 否则将当前这句话直接显示出来
            if (tempDialog == currentDialog.text) {
                setNextDialog();
            }
            else { 
                tempDialog = currentDialog.text;
                Text dialogText = DialogBox.transform.Find("DialogPanel").Find("DialogText").GetComponent<Text>();
                dialogText.text = tempDialog;
            }
        }
    }
}
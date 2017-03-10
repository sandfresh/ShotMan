using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Window : MonoBehaviour
{

    public Text titleLabel; 
    public Text contentLabel;
    public Button[] buttonArray;
 
    private static string path = "Prefab/Window/";

    public void setMessageBox(string title, string content)
    {
        this.titleLabel.text = title;
        this.contentLabel.text = content;
        buttonArray[0].onClick.AddListener(() => { this.Close(); });
    }
     

    public static void Show(string title, string content)
    { 
        var box = (Instantiate(Resources.Load<GameObject>(path+"Window")) as GameObject);
        var script = box.GetComponent<Window>(); 
        script.setMessageBox(title,content); 
    }
     
    public static void Show(GameObject parent, string title, string content)
    {
     
    }
     

    public static void Show(string title, string content, Action action0, Action action1)
    {

    }

    public void Close()
    {
        Destroy(gameObject);
    }
}

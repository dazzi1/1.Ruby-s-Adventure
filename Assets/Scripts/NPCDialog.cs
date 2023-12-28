using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCDialog : MonoBehaviour
{
    public GameObject dialogBox;
    public float displayTime = 4.0f;
    private float timerDisplay;
    public Text dialogTest;
    public AudioSource audioSource;
    public AudioClip completeTaskclip;
    private bool hasPlayed;

    void Start()
    {
        //关闭对话框
        dialogBox.SetActive(false);
        timerDisplay = -1;
    }


    void Update()
    {
        //对话框持续4s
        if (timerDisplay >= 0)
        {
            timerDisplay -= Time.deltaTime;
            if (timerDisplay < 0)
            {
                dialogBox.SetActive(false);
            }
        }
    }

    //显示对话框
    public void DisplayDialog()
    {
        timerDisplay = displayTime;
        dialogBox.SetActive(true);
        // 成功接取任务
        UIHealthBar.instance.hasTask = true;
        if (UIHealthBar.instance.fixedNum>=2) 
        {
            //已经完成任务，需要修改对话框内容
            dialogTest.text = "哦，伟大的Ruby,谢谢你,你真的太棒了";
            if (!hasPlayed) {
                audioSource.PlayOneShot(completeTaskclip);
                hasPlayed = true;
            }
            
        }
    }
}

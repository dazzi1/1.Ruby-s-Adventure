using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    public Image mask;
    private float originalSize;

    // 单例设计模式
    public static UIHealthBar instance { get; private set; }
    // 是否接取任务
    public bool hasTask;
    // 是否完成任务
    // public bool ifCompleteTask;
    // 修理数量
    public int fixedNum;

    // 实例化
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        // 设置血条初始长度
        originalSize = mask.rectTransform.rect.width;
    }

    // Update is called once per frame
    void Update()
    {

    }
    // 根据输入比值 设置当前UI血条的显示
    public void SetValue(float fillPercent)
    {
        mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.
            Axis.Horizontal, originalSize * fillPercent); ;
    }
}

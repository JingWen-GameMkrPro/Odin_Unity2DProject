using System.Collections;
using TMPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTipSystemController : MonoBehaviour
{
    /// <summary>
    /// 以下為外界可以呼叫的API
    /// </summary>
    /// <param name="text">提示訊息</param>
    /// <param name="continueSecond">持續的秒數</param>
    /// <param name="canBeOverride">當持續時間未結束，會不會被新的訊息覆蓋</param>

    public TMP_Text tiptext;
    private float continueSecond = 3.0f;
    private float timer;
    private bool canBeOverride = false;

    private void Start()
    {

       

    }



    public void ShowTip(string text, float duration, bool canBeOverride)
    {
        // 當呼叫此函式時，將會顯示提示訊息框物件 (應該設定visible，不要設定active，這會使腳本無法運作)
        // 此時將會進入倒數計時，時間結束後將會重新隱藏提示訊息框，unity計時功能請參閱 coroutine
        // 您可以簡單自行在這邊的 Start() 呼叫此函式，看看是否可以出現文字並在時間後隱藏

        tiptext.text = text;
        tiptext.gameObject.SetActive(true);

        this.continueSecond = duration;
        this.canBeOverride = canBeOverride;

        if (canBeOverride)
        {
            StopAllCoroutines();
            timer = 0;
        }

    }

    private void Update()
    {
        if (tiptext.gameObject.activeSelf && canBeOverride)
        {
            timer += Time.deltaTime;
            if (timer >= continueSecond)
            {
                tiptext.gameObject.SetActive(false);
                timer = 0;
            }
        }
    }
}
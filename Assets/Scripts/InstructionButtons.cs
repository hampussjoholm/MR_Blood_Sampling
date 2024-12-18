using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI; 


public class InstructionButtons : MonoBehaviour
{
    private float timer = 0f;  // 计时器
    private bool isTimerRunning = false;  // 是否计时
    private bool hasClickedNext = false;  // 是否已点击 Next
    public TMP_Text timerText;  // 用于显示时间的 Text 组件
    private int nbr = 0;
    private int maxNbr = 8;
    private int minNbr = 0;
    private GameObject[] slides = new GameObject[10];
    public GameObject slide1;
    public GameObject slide2;
    public GameObject slide3;
    public GameObject slide4_1;
    public GameObject slide5;
    public GameObject slide6;
    public GameObject slide7;
    public GameObject slide8;
    public GameObject slide9;
    public AudioClip[] slideAudioClips;
    public AudioSource audioSource;  // AudioSource 用于播放音频
    public GameObject needleGlow;

   

    private void Start() {
        slides[0] = slide1;
        slides[1] = slide2;
        slides[2] = slide3;
        slides[3] = slide4_1;
        slides[4] = slide5;
        slides[5] = slide6;
        slides[6] = slide7;
        slides[7] = slide8;
        slides[8] = slide9;

    }

    public void GoToSlide(int slideIndex) {
        if (slideIndex >= minNbr && slideIndex <= maxNbr) {
            slides[nbr].SetActive(false);  // hide
            nbr = slideIndex;  // update
            slides[nbr].SetActive(true);  // display
            if (slideIndex < slideAudioClips.Length) {
                audioSource.clip = slideAudioClips[slideIndex];
                audioSource.Play();
            }
        }
        if (nbr == maxNbr) {
            isTimerRunning = false;  // 停止计时
        }
    }

    public void Next() {
        if (!hasClickedNext) {  // 如果还没点击过 Next
            hasClickedNext = true;  // 记录点击过
            isTimerRunning = true;  // 启动计时器
        }
        if (nbr != 3) {
            slides[nbr].SetActive(false);
            nbr++;
            slides[nbr].SetActive(true);
            if (nbr < slideAudioClips.Length) {
                audioSource.clip = slideAudioClips[nbr];
                audioSource.Play();
            }
            //slide2.SetActive(true);
        } 
        if (nbr == 3) {
            needleGlow.SetActive(true);
        } else {
            needleGlow.SetActive(false);
        }
        if (nbr == maxNbr) {
            isTimerRunning = false;  // 停止计时
        }

    }

    public void Previous() {
        if (nbr != minNbr) {
            slides[nbr].SetActive(false);
            nbr--;
            slides[nbr].SetActive(true);
        }

    }
    private void Update() {
        if (isTimerRunning) {
            timer += Time.deltaTime;  // 增加计时器时间
            UpdateTimerText();  // 更新显示时间
        }
    }

    // 更新 Text 组件来显示时间
    private void UpdateTimerText() {
        float minutes = Mathf.FloorToInt(timer / 60);  // 获取分钟
        float seconds = Mathf.FloorToInt(timer % 60);  // 获取秒数
        float milliseconds = (timer * 100) % 100;  // 获取毫秒

        // 格式化时间并更新到 UI 上
        timerText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
    }
}

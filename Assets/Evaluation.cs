using System.Collections;
using System.Collections.Generic;
using TMPro;
//using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.UI;

public class Evaluation : MonoBehaviour
{
    public GameObject[] objects;         // 包含所有物体的数组
    public GameObject vessel1;            // 第五个物体，vessel
    public GameObject vessel2;
    public TextMeshProUGUI stepText;
    public float distanceThreshold = 0.01f;  // 距离阈值 //I think we can decrease this a bit now

    private int currentStep = 0;         // 当前步骤索引
    private List<int> userOrder = new List<int>();
    private AudioSource audioSource;     // 音频源组件
    public AudioClip stepAudioClip;      // 用来存储音频片段
    public GameObject needle_in_arm;
    public GameObject vessel1_in_needle;
    public GameObject vessel2_in_needle;
    public GameObject vessel2_in_tube;
    public GameObject needle_in_arm_with_tube;
    public GameObject put_needle_in_arm_with_tube;
    public GameObject bandaid_on_arm;
    public InstructionButtons instructionButtons;
    private float distance;
    private float distance_for_used_tube_to_vessel;
    private float distance_for_used_tube_to_collection;
    public float distanceThreshold2 = 0.1f;
    public GameObject tubecollection;



    void Start() {
        audioSource = GetComponent<AudioSource>();  // 
        stepText.text = $"Step {currentStep + 1}: Check swab";  // 

    }

    void Update() {
       
        if (currentStep < objects.Length+1) {
            

            distance = 1000f;

            if (currentStep == 1) { // step 1 is putting needle in arm
                // here check if 2 distance vessels are close enough. if so, distance = 0f; else distance = 1000f;
                if ((Vector3.Distance(vessel1_in_needle.transform.position, vessel1.transform.position) < distanceThreshold) && (Vector3.Distance(vessel2_in_needle.transform.position, vessel2.transform.position) < distanceThreshold)) {
                    distance = 0f;
                } else {
                    distance = 1000f;
                }
            } 
            else if (currentStep == 2) { // step 2 is putting tube in needle
                if (Vector3.Distance(vessel2_in_tube.transform.position, vessel2.transform.position) < distanceThreshold) {
                    distance = 0f;
                } else {
                    distance = 1000f;
                }
            } else if (currentStep == 4) { // step 2 is putting tube in needle
                distance = Vector3.Distance(objects[3].transform.position, vessel1.transform.position);
            } else {
                distance = Vector3.Distance(objects[currentStep].transform.position, vessel1.transform.position);
            }


            // 
            if (distance < distanceThreshold) {
                // 
                userOrder.Add(currentStep);

                // 
                if (IsInCorrectOrder()) {
                    // 
                    UpdateStepText();
                    PlayAudio();

                    // 激活相应的物体
                    if (currentStep == 1) {
                        objects[1].SetActive(false); // 体
                        needle_in_arm.SetActive(true); // 启用带有针的物体
                    }
                    if (currentStep == 2) {
                        objects[2].SetActive(false);
                        needle_in_arm.SetActive(false);// 禁用原始的tube物体
                        //StartCoroutine(EnableNeedleWithTubeAfterDelay(1f));
                        needle_in_arm_with_tube.SetActive(true);  // 启用物体
                        
                       
                        //needle_in_arm_with_tube.SetActive(true); // 
                    }

                    if (currentStep == 4) {
                        objects[3].SetActive(false);
                        bandaid_on_arm.SetActive(true);// 
                        //needle_in_arm_with_tube.SetActive(true); // 
                    }

                    instructionButtons.GoToSlide(currentStep + 1 + 3);
                    /*if (currentStep < 3) {
                        instructionButtons.GoToSlide(currentStep + 1 + 3);
                    } else {
                        instructionButtons.GoToSlide(currentStep + 1 + 3+1);
                    }*/
                    currentStep++;  // 增加步骤
                   
           


                } else {
                    stepText.text = "Wrong order! Please try again."; // 步骤顺序错误
                }
            }
            if (needle_in_arm_with_tube.activeSelf) {
                stepText.text = "Please remove the needle and tube together!";
                distance_for_used_tube_to_collection = Vector3.Distance(needle_in_arm_with_tube.transform.position, tubecollection.transform.position);
                //if (distance_for_used_tube_to_collection>0.2) {
                  //  stepText.text = $"distance is {distance_for_used_tube_to_collection} ";
                //}
                if (distance_for_used_tube_to_collection < distanceThreshold2) {
                    stepText.text = "Removed";
                    needle_in_arm_with_tube.SetActive(false);
                    put_needle_in_arm_with_tube.SetActive(true);

                    instructionButtons.GoToSlide(currentStep + 1 + 3);//CURRENT STEP=3
                    stepText.text = $"current step is {currentStep} ";
                    UpdateStepText();
                    currentStep++;

                }
            }


        }
    }

   
    IEnumerator EnableNeedleWithTubeAfterDelay(float delay) {
        yield return new WaitForSeconds(delay); // 等待指定的延时时间
        needle_in_arm_with_tube.SetActive(true);  // 启用物体
    }
    // 检查步骤是否按正确顺序执行
    bool IsInCorrectOrder() {
        for (int i = 1; i < userOrder.Count; i++) {
            if (userOrder[i] < userOrder[i - 1]) {
                return false; // 如果顺序错误，返回false
            }
        }
        return true; // 顺序正确
    }

    // 更新步骤文本
    void UpdateStepText() {
        if (currentStep == objects.Length+1) {
            stepText.text = "Good job! All steps completed.";  // 所有步骤完成
        } else {
            stepText.text = $"Step {currentStep + 1}: Done!";  // 当前步骤完成
        }
    }

    // 播放音频
    void PlayAudio() {
        if (audioSource != null && stepAudioClip != null) {
            audioSource.PlayOneShot(stepAudioClip);  // 播放音频片段
        }
    }
}

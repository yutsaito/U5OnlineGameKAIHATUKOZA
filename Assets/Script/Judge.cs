using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Judge : MonoBehaviour
{
    private string correctAnswerText;
    //public AudioSource correctAudioSource;
   // public AudioSource incorrectAudioSource;
    //public AudioClip correctAudioClip;
    //public AudioClip incorrectAudioClip;

    // Start is called before the first frame update
    void Start()
    {
        //correctAudioSource = Resources.Load<AudioSource>(power_2_a.wav);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void JudgeAnswer()              //選択肢ﾎﾞﾀﾝが押されたら呼び出されるよう、ﾎﾞﾀﾝのOnClickにする
    {
        Debug.Log("選択肢ﾎﾞﾀﾝがおされたよ！");
        //選択したこのﾎﾞﾀﾝのﾃｷｽﾄﾗﾍﾞﾙを取得する
        Text selectedBtn = this.GetComponentInChildren<Text>();


        //選択肢ﾎﾞﾀﾝを触れなくする が、なぜか働かない！　　→　外で宣言すればいいかも！  →　ダメでした・・・
        for (int i = 1; i <= 4; i++)
        {
            Button setAnsButton = GameObject.Find("Canvas/Button" + i).GetComponent<Button>();
            setAnsButton.interactable = false;
        }


        //正解・不正解音をならす準備
        //AudioSource audioSource = this.GetComponent<AudioSource>();
        //親ｵﾌﾞｼﾞｪｸﾄにアタッチされているMonsterController.csが持っている正答(corAnswer)を取得する


        //正解取得
        //Script corAnswerText = transform.root.gameObject.GetComponent<MonsterController>();  //無理だった
        correctAnswerText = transform.root.gameObject.GetComponent<MonsterController>().correctWord.text;

        //正誤判定
        if (selectedBtn.text == correctAnswerText)
        {
            Debug.Log("正解");
            //正解音
            //audioSource.clip = correctAudioClip;
            //audioSource.Play();

        }
        else
        {    //不正解だったら、You dont know! とこのボタンに表示、さらに選択肢ﾎﾞﾀﾝの表記を空白にする

            Debug.Log("間違い");
            //不正解音
           // audioSource.clip = incorrectAudioClip;
           // audioSource.Play();
                                                             //SetWordMgrｵﾌﾞｼﾞｪｸﾄにｱﾀｯﾁしてあるSetWordMgr.csのﾒｿｯﾄﾞInputText()を呼び出す
        }

    }

   
    

}



//親オブジェクトをスクリプトで取得するには、
//transform.～を使う

//transform.root.gameObject ; ←一番上の親を取得
//transform.parent.gameObject; ←一つ上の親を取得



//ﾓﾝｽﾀｰのﾎﾞﾀﾝから、質問Word、正解wordを保存しておく
//Transform.Findは子ｵﾌﾞｼﾞｪｸﾄから探す、非ｱｸﾃｨﾌﾞも//秋山さんに教えてもらった
//Start()内に置くと、ﾃｷｽﾄは全てdefaultの"button"になってしまった　　→　　SetWordMgrのStart()をAwake()に書き換えたらButtonに値が入った後にこっちにくるようになった。
//quizWord = this.transform.Find("Canvas/Button0").GetComponentInChildren<Text>();
//correctWord = this.transform.Find("Canvas/Button1").GetComponentInChildren<Text>();

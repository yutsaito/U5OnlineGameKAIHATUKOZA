using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Judge : MonoBehaviour
{
    private string correctAnswerText;
    //public AudioSource correctAudioSource;
   // public AudioSource incorrectAudioSource;
    public AudioClip correctAudioClip;
    public AudioClip incorrectAudioClip;
    private AudioSource audioSource;
    public bool isCorrect=false;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();       //CDプレーヤーのようなイメージ
        //ResourcesｸﾗｽのLoadで、Resourcesﾌｫﾙﾀﾞのsoundﾌｫﾙﾀﾞ中の該当ﾌｧｲﾙを、AudioClipとしてﾛｰﾄﾞせよ
        correctAudioClip = Resources.Load("Sound/DM-CGS-26") as AudioClip;
        incorrectAudioClip = Resources.Load("Sound/power_2_a") as AudioClip;
        //ｼｰﾝ切り替え時に、現在使用していないアセットを破棄すること
        //Resources.UnloadUnusedAssets();
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
        correctAnswerText = transform.root.gameObject.GetComponent<MonsterController>().correctWord;

        //正誤判定
        if (selectedBtn.text == correctAnswerText)
        {
            isCorrect = true;
            Debug.Log("正解");
            //正解音
            audioSource.clip = correctAudioClip;
            audioSource.Play();
            //Monsterに正解Messageを言わせる  →　MonsterController.csのMonsterMessageﾒｿｯﾄﾞに、引数”正解”を渡し、正解ｺﾒﾝﾄを言わせる
            transform.root.gameObject.GetComponent<MonsterController>().MonsterMessage(ref isCorrect);
            //一番上の親ｵﾌﾞｼﾞｪｸﾄ(Monster)にｱﾀｯﾁしてあるMonsterController.csのﾒｿｯﾄﾞMonsterMessage()を、引数isCorrectを渡して呼び出す
        }
        else
        {    //不正解だったら、You dont know! とこのボタンに表示、さらに選択肢ﾎﾞﾀﾝの表記を空白にする
            isCorrect = false;
            Debug.Log("間違い");
            //不正解音
           audioSource.clip = incorrectAudioClip;
           audioSource.Play();
            //Monsterに間違いMessageを言わせる
            transform.root.gameObject.GetComponent<MonsterController>().MonsterMessage(ref isCorrect);
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



////BGM(SE)の読み込み
//private AudioSource audioSource;
//audioSource = gameObject.AddComponent<AudioSource>();
//audioSource.clip = Resources.Load("Sound/BGM/sample") as AudioClip;

////Prefabの読み込み
//GameObject prefab;
//prefab = (GameObject) Resources.Load("Prefabs/sample");

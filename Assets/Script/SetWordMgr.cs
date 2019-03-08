using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class SetWordMgr : MonoBehaviour
{
    //WordsDataBase（本来独立したｸﾗｽにしたいが・・・、現時点でよくわからない20190108）
    string[] word5Sets = { "effort", "努力", "期待", "実行", "効果", "suppose", "思う", "支える", "越える", "探す", "quote", "引用する", "褒める", "喜ぶ", "戦う", "opposite", "反対の", "賛成の", "遠方の", "地方の" };

    public int totallQuestions;     //全Question数　Inspectorで設定する（まずは）

    //ここからが単語登録ｼｰﾝのﾒｲﾝﾌﾟﾛｸﾞﾗﾑ(①Wordをinput, ②選択肢ﾎﾞﾀﾝをｸﾘｯｸ、③正誤判定、以上の繰り返しを制限時間で終え、対戦開始画面に移行)
    Button questionButton;     //QuestionWordはドアに壁に貼り付けたﾎﾞﾀﾝに表示する         InputField inputWord; //入力Word
    Text questionText;        //string inputWordText;           //入力WordのText

    //選択肢をｼｬｯﾌﾙしﾎﾞﾀﾝに書き込むｸﾗｽのｲﾝｽﾀﾝｽ、そのまま次のResultSceneでも使うために、関数の外で宣言することにした
    public SetAnsLabelSet setAnsLabelSet = new SetAnsLabelSet();

    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(this);                    //次のシーンでも問題ﾘｽﾄを使うので
    }

    // Update is called once per frame
    void Update()
    {

    }

    //壁にはったQuestionButtonを押すことで呼ばれる、ここからプログラムが始まる
    public void OnQuestionButtonClicked()
    {
        //入力されたwordを取得(もともとはUI→InputFieldだったものをInputQtnと名を変え、そこのInputFieldを取得(これで入力textが取得できる))
        //inputWord = GameObject.Find("InputQtn").GetComponent<InputField>();
        questionButton = GameObject.Find("SetWord/QuestionButton").GetComponent<Button>();      //QuestionButtonを見つける
        questionText = questionButton.GetComponentInChildren<Text>();

        //乱数でQuestionWordを選ぶ
        int questionWordNo = Random.Range(0, word5Sets.Length / 5);
        Debug.Log(questionWordNo);
 //       Debug.Log(word5Sets[5]);
        questionText.text = word5Sets[questionWordNo*5];

        //qWord[]は解答選択肢を入れる変数
        string[] qWord = new string[4];
        //回答を配列変数qWord[]にｾｯﾄ
        for (int i = 0; i <= 3; i++)
        {
            qWord[i] = word5Sets[questionWordNo*5+1 + i];        
        }

        string corAnswer = qWord[0];        //正答を保存しておく

        //Judge0.csに正答を渡しておく
 //       Judge0.SetToBeRegisteredWord(corAnswer);

        setAnsLabelSet.GetSelections(ref qWord[0], qWord[1], qWord[2], qWord[3]);　　//自作クラスの自作メソッド　回答選択肢をｼｬｯﾌﾙする

        //Judge0.cs;     //これは各選択肢ﾎﾞﾀﾝに貼り付けておくべし！ﾎﾞﾀﾝｸﾘｯｸで正誤判定する
        //正解したらドアが開くようにする。　　　　　　//正解したら、Register可能にする
        //間違えたら解答選択肢をシャッフルし、もう一度同じ問題をやる。扉が開くまでやる。　　　　　//間違えたら、もう一度単語入力からやり直す→→→つまり、このSetWordMgr.csのInputText()を最初からやりなおす（呼ぶ）
        //以上から、ﾎﾞﾀﾝにｱﾀｯﾁしたJudge0.csで正誤判定をし正解ならドアを開け、間違いならもう一度このクラスのOnQuestionButtonClicked()ﾒｿｯﾄﾞを呼ぶ
    }

    //選択肢のｾｯﾄのクラス(上にｲﾝｽﾀﾝｽを記述してある)
    public class SetAnsLabelSet
    {
        //選択肢をﾎﾞﾀﾝに作成（正答、誤答1、誤答2、誤答3、誤答4）
        public void GetSelections(ref string b, string c, string d, string e)
        {
            string[] array3 = new string[4];    //例{ "効果", "実行", "努力", "期待" };
            array3[0] = b;
            array3[1] = c;
            array3[2] = d;
            array3[3] = e;

            //ｼｬhｯﾌﾙ
            string[] array4 = array3.OrderBy(i => Guid.NewGuid()).ToArray();
            //5つのﾎﾞﾀﾝに代入
            for (int i = 1; i <= 4; i++)
            {
                Text ansText = GameObject.Find("SetWord/AnsButton" + i).GetComponentInChildren<Text>();
                ansText.text = array4[i - 1];
                Button AnsButton = GameObject.Find("SetWord/AnsButton" + i).GetComponent<Button>(); //これは選択肢ﾎﾞﾀﾝが押されることにより呼び出されるJudge0の中でfalseにされる
                AnsButton.interactable = true;

            }

        }
    }
}

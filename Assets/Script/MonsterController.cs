using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class MonsterController : MonoBehaviour
{
    public String quizWord;
    public String correctWord;
    //ﾎﾞﾀﾝのﾃｷｽﾄを読み取るための配列変数
    private Text[] buttonText = new Text[5];
    //選択肢ﾎﾞﾀﾝのみを読み取り保存するための配列変数
    private Text[] buttonText2 = new Text[4];
    private GameObject player;
    private bool isEncounteredToPlayer = false;
    //子ｵﾌﾞｼﾞｪｸﾄであるﾎﾞﾀﾝ類を隠す⇔表示させるためのCanvas
    private GameObject myCanvas;
    //正解・不正解を受けてﾓﾝｽﾀｰが発する言葉を表示するためのCanvas
    private GameObject myCanvasForMessage;
    //[SerializeField]にしたのは、ﾓﾝｽﾀｰごとに違う言葉を発するように、Inspectorで個々に入力するため
    [SerializeField] String correctMessageText;
    [SerializeField] String incorrectMessageText;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");     //ﾋｴﾗﾙｷｰからPlayerを探して変数に入れる

        //ﾓﾝｽﾀｰのﾎﾞﾀﾝから、質問Word、正解wordを保存しておく
        //Transform.Findは子ｵﾌﾞｼﾞｪｸﾄから探す、非ｱｸﾃｨﾌﾞも//秋山さんに教えてもらった
        //Start()内に置くと、ﾃｷｽﾄは全てdefaultの"button"になってしまった　　→　　SetWordMgrのStart()をAwake()に書き換えたらButtonに値が入った後にこっちにくるようになった。
        quizWord = this.transform.Find("Canvas/Button0").GetComponentInChildren<Text>().text;       //ｸｲｽﾞ英単語はｹﾞｰﾑｽﾀｰﾄ時にSetWordMgrから渡されるﾎﾞﾀﾝ0のtext(Textではなく）を読み取る、その後も変化なし
        correctWord = this.transform.Find("Canvas/Button1").GetComponentInChildren<Text>().text;    //正解英単語はｹﾞｰﾑｽﾀｰﾄ時にSetWordMgrから渡されるﾎﾞﾀﾝ１のtextを読み取る、その後も変化なし
        Debug.Log(quizWord);
        Debug.Log(correctWord);
        for (int i = 0; i <= 4; i++)
        {
            buttonText[i] = this.gameObject.transform.Find("Canvas/Button" + i).GetComponentInChildren<Text>();     //buttonText[0]:問題 buttonText[1]:正解 buttonText[2-4]:誤回答
            Debug.Log(buttonText[i].text);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Playerと出会ったらそっちをゆっくり向く
        if (isEncounteredToPlayer)
        {
            // 補完スピードを決める
            float speed = 0.05f;
            // ターゲット方向のベクトルを取得
            Vector3 relativePos = player.transform.position - this.transform.position;
            //   Vector3型の変数 relativePos に、Playerの位置情報ﾏｲﾅｽこれの位置情報を入れよ
            // 方向を、回転情報に変換
            Quaternion rotation = Quaternion.LookRotation(relativePos);
            //ｸｫｰﾀﾆｵﾝ型の変数 rotation にQuatanionクラスのLookRotation()を入れる
            // 現在の回転情報と、ターゲット方向の回転情報を補完する
            transform.rotation = Quaternion.Slerp(this.transform.rotation, rotation, speed);
        }
        else
        {
            this.transform.Rotate(0, 1, 0);
        }


    }

    //Playerとの衝突処理
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerTag")
        {
            //明日20190321は以下を試す。 →　これではパッと向いてしまう
            //var aim = other.gameObject.transform.position - this.gameObject.transform.position;
            //var look = Quaternion.LookRotation(aim);
            //this.gameObject.transform.localRotation = look;
            isEncounteredToPlayer = true;

            //まず、シャッフルする
            suffle(ref buttonText[1], buttonText[2], buttonText[3], buttonText[4]);
            



            //ﾎﾞﾀﾝ表示はゆっくりだしたい(今はぱっとでる)
            myCanvas = this.transform.Find("Canvas").gameObject;
            CanvasGroup myCanvasGroup = myCanvas.GetComponent<CanvasGroup>();
            myCanvasGroup.alpha = 0.9f;
            myCanvasGroup.interactable = true;

        }
    }

    public void suffle(ref Text b, Text c, Text d, Text e)
    {
        //クラスTextで配列をつくるとうまくいかない（以下秋山さんの説明）　　→　変数string[]の配列にすれば、中身を直接変えられる
        //インデックスが0, 1, 2, 3 のText配列に、
        //シャッフルしてインデックスが1, 2, 3, 0 のText配列の文字列を順番に代入していくとして、
        //最初の0に1の文字列を代入します。
        //ここで注意してほしいのは、シャッフルした方の配列のインデックス0(配列の最後尾)の文字列も1と同じ文字列になる、ということです。
        //配列をシャッフルしても、Textは内容が完全にコピーされるわけでなく、それぞれの要素が指すのは元の配列と「同じ」Textです。

        //元の配列、シャッフルした配列の要素は別物でなく同じデータを参照しているので、代入操作によって内容がかぶってしまうことになります。

        //結局、Textはクラスであって、その配列の順番を変えると全ての中身も（ﾘﾝｸされて）ついてくる、ということ。
        //そういう状態で、たとえば一番最初にあらたに持ってきた配列要素の中身の一部であるtextを代入すると、
        //その持ってこられた元の配列要素がﾘﾝｸされているので、元の配列自体が変わってしまう。・・・秋山さんの説明の方がわかりやすいね・・・
        //やっぱり今いちピンとこない。        //Debug.Log(array4[0].text);で正常にｼｬｯﾌﾙされたのが表示されたので。

        //Text[] array3 = new Text[4];
        //array3[0] = b;
        //array3[1] = c;
        //array3[2] = d;
        //array3[3] = e;

        String[] texts1 = new string[4];
        texts1[0] = b.text;
        texts1[1] = c.text;
        texts1[2] = d.text;
        texts1[3] = e.text;

        //Text[] array4 = array3.OrderBy(i => Guid.NewGuid()).ToArray();     //ここがおかしい20190322
        String[] texts2 = texts1.OrderBy(i => Guid.NewGuid()).ToArray();

        //Debug.Log(array4[0].text);
        //Debug.Log(array4[1].text);
        //Debug.Log(array4[2].text);
        //Debug.Log(array4[3].text);  //ここはOKみたい

        buttonText2[0] = this.transform.Find("Canvas/Button1").GetComponentInChildren<Text>();
        buttonText2[0].text = texts2[0];
        buttonText2[1] = this.transform.Find("Canvas/Button2").GetComponentInChildren<Text>();
        buttonText2[1].text = texts2[1];
        buttonText2[2] = this.transform.Find("Canvas/Button3").GetComponentInChildren<Text>();
        buttonText2[2].text = texts2[2];
        buttonText2[3] = this.transform.Find("Canvas/Button4").GetComponentInChildren<Text>();
        buttonText2[3].text = texts2[3];

        //buttonText2[0] = this.transform.Find("Canvas/Button1").GetComponentInChildren<Text>();  
        //buttonText2[0].text = array4[0].text;
        //buttonText2[1] = this.transform.Find("Canvas/Button2").GetComponentInChildren<Text>();
        //buttonText2[1].text = array4[1].text;
        //buttonText2[2] = this.transform.Find("Canvas/Button3").GetComponentInChildren<Text>();
        //buttonText2[2].text = array4[2].text;
        //buttonText2[3] = this.transform.Find("Canvas/Button4").GetComponentInChildren<Text>();
        //buttonText2[3].text = array4[3].text;


        Debug.Log(buttonText2[0].text);
        Debug.Log(buttonText2[1].text);
        Debug.Log(buttonText2[2].text);
        Debug.Log(buttonText2[3].text);

    }

    public void MonsterMessage(ref bool isCorrect)
    {
        Debug.Log("ﾒｯｾｰｼﾞがあるよ！");
        //ﾒｯｾｰｼﾞ表示
        String myMessage = this.transform.Find("MonsterMessageCanvas/MonsterMessageText").GetComponent<Text>().text;
 //       Debug.Log(myMessage);
        if (isCorrect)
        {
            myMessage= correctMessageText;      //これでは表示されない
            this.transform.Find("MonsterMessageCanvas/MonsterMessageText").GetComponent<Text>().text = correctMessageText;
            Debug.Log(myMessage);
        }
        else
        {
            myMessage = incorrectMessageText;       //これでは表示されない

            Debug.Log(myMessage);
            this.transform.Find("MonsterMessageCanvas/MonsterMessageText").GetComponent<Text>().text = incorrectMessageText;
        }

        myCanvasForMessage = this.transform.Find("MonsterMessageCanvas").gameObject;
        CanvasGroup myCanvasGroup = myCanvasForMessage.GetComponent<CanvasGroup>();
        myCanvasGroup.alpha = 1.0f;
    }

    //ｼｬｯﾌﾙ後の選択肢を入れなおす
    //        for (int i = 0; i <4; i++)
    //        {
    //            Debug.Log(array4[i].text);
    //             buttonText[i+1].text = array4[i].text;     //buttonText[0]:問題 buttonText[1]:正解 buttonText[2-4]:誤回答
    ////            Debug.Log(array4[i-1].text);
    //        }
    //    }

    /// 重複のないランダムな配列を取得
    /// </summary>
    // public int[] randAry(int max, int length)
    //  {
    //     return Enumerable.Range(0, 3).OrderBy(n => Guid.NewGuid()).Take(length).ToArray();
    // }


    //    //5つのﾎﾞﾀﾝに代入
    //    //for (int i = 1; i <= 4; i++)
    //    //{
    //    //    Text setAnsLabel = GameObject.Find("SetWord/SetAnsButton" + i).GetComponentInChildren<Text>();
    //    //    setAnsLabel.text = array4[i - 1];
    //    //    //このﾎﾞﾀﾝに触れるよう、interactable を Trueに→うまくいかなかった。
    //    //    // bool canvasGroupFBtn = GameObject.Find("SetWord/SetAnsButton" + i).GetComponent<Button>().interactable; //これは選択肢ﾎﾞﾀﾝが押されることにより呼び出されるJudge0の中でfalseにされる
    //    //    // canvasGroupFBtn = true;
    //    //    // 平野さんのアドバイス
    //    //    //setAnsButtonInteractable = GameObject.Find("SetWord/SetAnsButton" + i).GetComponent<Button>().interactable;
    //    //    //> setAnsButtonInteractable = false;
    //    //    // で制御されているかと思いますが、こちらですとsetAnsButtonInteractableという変数がfalseになるだけで、ボタン自体がfalseになるわけではございません。
    //    //    //ではボタン自体をfalseにするには、
    //    //    //Button ansButton = GameObject.Find("SetWord/SetAnsButton" + i).GetComponent<Button>();
    //    //    // ansButton.enabled = false;
    //    //    //  といったように、取得したボタン自体を変数に入れてあげてenabledフラグを書き換えると良いです。
    //    //    //setAnsButtonInteractable変数はあくまでtrueかfalseかが入っているだけのboolの変数ですので、現状ボタンとは関係のないものになっています。
    //    //    //操作はボタンに対して行いたいので、ボタンを変数として保存する必要がございます。
    //    //    Button setAnsButton = GameObject.Find("SetWord/SetAnsButton" + i).GetComponent<Button>(); //これは選択肢ﾎﾞﾀﾝが押されることにより呼び出されるJudge0の中でfalseにされる
    //    //    setAnsButton.interactable = true;

    //    //}
    //    //Text[] array3 = new Text[4];    //例{ "効果", "実行", "努力", "期待" };
    //    //array3[0] = b;
    //    //array3[1] = c;
    //    //array3[2] = d;
    //    //array3[3] = e;
    //    ////ｼｬｯﾌﾙ後の値用配列変数
    //    //Text[] buttonTextsuffled = buttonTextsuffled.OrderBy(i => Guid.NewGuid()).ToArray();
    //}
}


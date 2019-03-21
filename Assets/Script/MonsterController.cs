using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterController : MonoBehaviour
{
    private Text quizWord;
    private Text correctWord;
    private Text[] buttonText=new Text[4];
    private GameObject player;
    private bool isEncounteredToPlayer=false;
    private GameObject myCanvas;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");     //ﾋｴﾗﾙｷｰからPlayerを探して変数に入れる

        //ﾓﾝｽﾀｰのﾎﾞﾀﾝから、質問Word、正解wordを保存しておく
        //Transform.Findは子ｵﾌﾞｼﾞｪｸﾄから探す、非ｱｸﾃｨﾌﾞも//秋山さんに教えてもらった
        //Start()内に置くと、ﾃｷｽﾄは全てdefaultの"button"になってしまった
        //quizWord = this.transform.Find("Canvas/Button0").GetComponentInChildren<Text>();
        //correctWord = this.transform.Find("Canvas/Button1").GetComponentInChildren<Text>();
        //Debug.Log(quizWord.text);
        //Debug.Log(correctWord.text);
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


            //ﾓﾝｽﾀｰのﾎﾞﾀﾝから、質問Word、正解wordを保存しておく　　→　これは後でシャッフルするのでよくないだろう・・・
            //Transform.Findは子ｵﾌﾞｼﾞｪｸﾄから探す、非ｱｸﾃｨﾌﾞも//秋山さんに教えてもらった
            quizWord = this.transform.Find("Canvas/Button0").GetComponentInChildren<Text>();
            correctWord = this.transform.Find("Canvas/Button1").GetComponentInChildren<Text>();
            Debug.Log(quizWord.text);
            Debug.Log(correctWord.text);

            //ﾎﾞﾀﾝ表示はゆっくりだしたい(今はぱっとでる)
            myCanvas = this.transform.Find("Canvas").gameObject;
            CanvasGroup myCanvasGroup = myCanvas.GetComponent<CanvasGroup>();
            myCanvasGroup.alpha = 1;



            //以下でﾓﾝｽﾀｰが正対しなかった
            //float playerAngle = other.transform.localEulerAngles.y;
            //float myAngle = this.gameObject.transform.localEulerAngles.y;
            //Debug.Log(playerAngle);
            //Debug.Log(myAngle);
            //Debug.Log(playerAngle - myAngle);
            //this.gameObject.transform.Rotate(new Vector3(0f,myAngle-playerAngle+180.0f,0f));
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody myRigidbody;
    ////はしごに触っていたらtrue
    //    private bool isTouchingLadder = false;

     [SerializeField] float m_moveSpeedOnLadder = 1.0f;    // このメンバ変数は上の方に書いた方がよい  鈴木さんに書いてもらった
                                                           //    m：メンバ変数（privateな変数）を示す
                                                           //s：静的変数（static付き変数）を示す
    //ﾓﾝｽﾀｰと出会った時のﾌﾗｸﾞ
    //private bool isEncountered=false;
    public bool isEncountered=false;

    // Start is called before the first frame update
    void Start()
    {
        //Rigidbodyコンポーネントを取得
        this.myRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {       //FixedUpdateにしても変わらずに回ってしまうのだが・・・
        //if (Input.GetKey(KeyCode.LeftArrow))
        //{
        //    this.transform.Rotate(0, -2, 0);
        //}
        //if (Input.GetKey(KeyCode.RightArrow))
        //{
        //    this.transform.Rotate(0, 2, 0);
        //}

        //if (Input.GetKey(KeyCode.UpArrow))
        //{
        //    // ローカルX軸正方向に移動
        //   // this.myRigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;
        //    if (this.myRigidbody.velocity.magnitude < 2.0f)     //速度制限
        //    {
        //        this.myRigidbody.AddForce(transform.TransformDirection(Vector3.forward) * 0.25f, ForceMode.Impulse);
        //    }
        //} 
        //ﾓﾝｽﾀｰと出会ってなければ動ける
        if (!isEncountered) {PlayerMovement(); }
        
    }

    //ﾓﾝｽﾀｰとの衝突処理
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "MonsterTag") {
            //まず、画面を止めたい→出会ったらﾌﾗｸﾞをtrue
            isEncountered = true;
            //次にモンスターを正対させたい　　→　SetWordMgrで書く、ということは　isEncounteredはpublicにする
            //選択肢をランダムに配列しなおしたい　　→　ここで書く
            //ﾎﾞﾀﾝ表示はゆっくりだしたい(今はぱっとでる)
            GameObject monsterCanvas = other.transform.Find("Canvas").gameObject;
            CanvasGroup monsterCanvasGroup = monsterCanvas.GetComponent<CanvasGroup>();
            monsterCanvasGroup.alpha = 1; 
            //クイズに行く
        }
    }

    void PlayerMovement()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            this.transform.Rotate(0, -2, 0);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            this.transform.Rotate(0, 2, 0);
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            // ローカルX軸正方向に移動
            // this.myRigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;
            if (this.myRigidbody.velocity.magnitude < 2.0f)     //速度制限
            {
                this.myRigidbody.AddForce(transform.TransformDirection(Vector3.forward) * 0.25f, ForceMode.Impulse);
            }
        }
    }  

    /* === 以降 梯子を上り下りする処理 === */

   // [SerializeField] float m_moveSpeedOnLadder = 5.0f;    // このメンバ変数は上の方に書いた方がよい  鈴木さんに書いてもらった

    void OnTriggerStay(Collider other)      //鈴木さんに書いてもらった
    {
        if (other.gameObject.tag == "Ladder")
        {
            //Debug.Log("LADDER!!");
            myRigidbody.useGravity = false;
            Debug.Log(myRigidbody.useGravity);
            // float v = Input.GetAxisRaw("Vertical");       //スマホでどうしていいかわからないので、if(Input.GetKey(KeyCode.Uparrow)にする
            // if (v == 0)
            //{
            //    int v = 1;
            //    myRigidbody.velocity = Vector3.zero;
            //}
            //else
            //{
            //    myRigidbody.velocity = Vector3.up * v * m_moveSpeedOnLadder;
            //}

            
            //はしごのUp/Down/Stay(1/-1/0)
            int v = 0;
            if (Input.GetKey(KeyCode.UpArrow))
            {
                v = 1;
                //               myRigidbody.velocity = Vector3.zero;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                v = -1;
            }
            //else { v = 0; }
            myRigidbody.velocity = Vector3.up * v * m_moveSpeedOnLadder;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Ladder")
        {
            myRigidbody.useGravity = true;
        }
    }
}






//基礎１. 「target_1」を取得してみましょう
//Transform.Findを使った場合
//例１）Transform target = this.transform.Find(“Parent_2 / target_1”);

//例２）GameObject target = this.transform.Find(“Parent_2 / target_1”).gameObject;

//Transform.Findを使えば、target_1が非アクティブでも取得可能ですね。

//注意が必要なのは、Transform.Findを使ったときは「Transform」で取得されることです。

//例２では「.gameObject」として、TransformからGameObjectを取得しています。


//基礎１. 「target_1」を取得してみましょう
//“Script”にスクリプトがアタッチされているため、

//this.transform.Find(“Parent_2/target”);

//このように直接”子オブジェクト”を探す書き方ができません。

 

//一旦、targetの親オブジェクトで「アクティブ」になっているParent_2を取得し、

//そのあとでtargetを取得する、という手順を踏みます。

//１．GameObject temp = GameObject.Find(“Parent_1/Parent_2”);

//２．GameObject target = temp.transform.Find(“target”).gameObject;

//この手順を踏めば、非アクティブなtargetも取得することができます。

 

//上記を１つにまとめて書くこともできます。

//GameObject target = GameObject.Find(“Parent_1/Parent_2”).transform.Find(“target”).gameObject;

//こんな感じになります。





//衝突処理
//void OnTriggerStay(Collider other)     //OnTriggerEnterでは衝突した瞬間だけしか働かないだろう、接触中みたいな関数がないとうまくいかないだろう　→　OnTriggerStayというのがあった！
//        {
//            //障害物に衝突
//            //まず、はしごに衝突したら上か下にいく。前に少し進める　　→　難しそう！はしごの上下判定も必要。さらに組み合わせのあるはしごもある
//            ////自動的に上or下にワープするのはかえって難しい感じ。単純にUpForce、DownForceを使ってみるか
//            //if (other.gameObject.tag == "Ladder" && Input.GetKey(KeyCode.UpArrow))
//            //{
//            //    Debug.Log("LADDER!");
//            //           isTouchingLadder = true;
//            //    calledNo++;
//            //    if(calledNo%10<5){ yurayuraNo *= -1; }
//            //    this.myRigidbody.AddForce(transform.TransformDirection(Vector3.up) * 0.39f, ForceMode.Impulse); //こうしてもダメ、→0.1では力が弱すぎた
//            //    //this.transform.Rotate(0,0,yurayuraNo,Space.World);

//        }
//        }
//}


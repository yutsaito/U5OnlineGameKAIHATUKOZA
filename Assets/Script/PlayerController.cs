using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody myRigidbody;
    //はしごに触っていたらtrue
        private bool isTouchingLadder = false;
    //はしご登るときのゆらゆら
    // private float yurayuraNo = 0.3f;
    //はしごのCode通過回数
    // private int calledNo=0;
    //はしごのUp/Down/Stay(1/-1/0)
    // private int v = 0;

     [SerializeField] float m_moveSpeedOnLadder = 1.0f;    // このメンバ変数は上の方に書いた方がよい  鈴木さんに書いてもらった
//    m：メンバ変数（privateな変数）を示す
//s：静的変数（static付き変数）を示す

    // Start is called before the first frame update
    void Start()
    {
        //Rigidbodyコンポーネントを取得
        this.myRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()

    {       //FixedUpdateにしても変わらずに回ってしまうのだが・・・
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
            //はしごをゆらすため、GameObjectを取得、したいが、どうやって取得する　→　gameObjectが衝突しているLadderのハズ
            
            //はしごのUp/Down/Stay(1/-1/0)
            int v = 0;
            if (Input.GetKey(KeyCode.UpArrow))
            {
                v = 1;
                //               myRigidbody.velocity = Vector3.zero;
                //はしごをちょっと揺らす  →　あきらめた
                //for(int i = 0; i < 40; i++) {  gameObject.transform.Translate(0.01f,0.01f,0.01f);}
                //for (int i = 0; i < 500; i++) ;
                //for (int i = 0; i < 40; i++) { gameObject.transform.Translate(-0.01f, -0.01f, -0.01f); }
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


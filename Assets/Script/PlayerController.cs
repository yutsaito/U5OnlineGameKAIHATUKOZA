using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody myRigidbody;
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
    //衝突処理
        void OntriggerStay(Collider other)     //OnTriggerEnterでは衝突した瞬間だけしか働かないだろう、接触中みたいな関数がないとうまくいかないだろう　→　OnTriggerStayというのがあった！
        {
            //障害物に衝突
            //まず、はしごに衝突したら上か下にいく。前に少し進める　　→　難しそう！はしごの上下判定も必要。さらに組み合わせのあるはしごもある
            //自動的に上or下にワープするのはかえって難しい感じ。単純にUpForce、DownForceを使ってみるか
            if (other.gameObject.tag == "Ladder")  
            {
                Debug.Log("LADDER!");
                //はしごの大きさを取得したい！
                //positionを↑or↓、且つ、前に
                //this.myRigidbody.transform.position=new Vector3(     )
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    this.myRigidbody.AddForce(transform.TransformDirection(Vector3.up) * 0.1f, ForceMode.Impulse);
                }
            }
        }
}


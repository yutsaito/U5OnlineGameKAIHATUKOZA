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
            this.transform.Rotate(0, -1, 0);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            this.transform.Rotate(0, 1, 0);
        }



        if (Input.GetKey(KeyCode.UpArrow))
        {
            // ローカルX軸正方向に移動
            //ローカルY軸の回転を制限
            this.myRigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;
            //次のは壁にぶつかった後、ずっと回転してしまう。
            if (this.myRigidbody.velocity.magnitude < 2.0f)
            {
                this.myRigidbody.AddForce(transform.TransformDirection(Vector3.forward) * 0.25f, ForceMode.Impulse);
                //this.myRigidbody.AddForce(transform.TransformDirection(Vector3.forward)*0.25f, ForceMode.Acceleration); Accelerationでは動かず
                //this.myRigidbody.AddForce(transform.forward*Input.GetAxisRaw("Horizontal")*0.5f,ForceMode.Impulse);動かず
                //transform.right ローカル座標のX軸   transform.up ローカル座標のY軸  transform.forward ローカル座標のZ軸
                //Input.GetAxisRaw(string axisName)      特定方向の入力を - 1 ～ 1 の正規化された値で返す
                //  Input.GetAxis("Vertical")           垂直方向キー
                // Input.GetAxis("Horizontal")         水平方向キー
                //動いている間、FreezeRotation.yをtrueににすればいいのではないか？ }
            }
        }
        else
        {
            this.myRigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            //まだ回転してしまうことはあるが、上矢印ｷｰでとまるようにはなった。
        }
    }
}


//ｱﾄﾞﾌｫ-ｽのﾍﾞｸﾀ-3の力にtransform.forwardなどを掛けると、その方向にﾌｫ-ｽがかかり加速して行きます。

//ﾌｫ-ｽﾓ-ﾄﾞはアクセラレーションの方が楽です。

//clamp(与えたい加速度 , 希望最高速度-現在速度[+-両方])
//しておくことを忘れないようにしましょう。
//速度コントロールが楽になりますね。
//[既実行ならすみません]

//あとこれらは全て、fixupdateの中で行うと正しい動作をします。
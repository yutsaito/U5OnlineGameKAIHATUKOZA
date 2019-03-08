using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;

public class manage : MonoBehaviourPunCallbacks
{
            //これはMainCameraにｱﾀｯﾁするスクリプト

    private bool keyLock;
    // Start is called before the first frame update
    void Start()
    {
        keyLock = false;
        //PhotonNetwoork PUN2のサーバーへ接続い、ロビーへ入室
        PhotonNetwork.ConnectUsingSettings();   //ConnectUsingSettingsはPUN2で、Photon.PUNに記述されているﾈｯﾄﾜｰｸ接続を一手に引き受けるクラス
        //PhotonNetworkのConnectUsingSettingを使え
    }

    //ロビーに入室した
    //void OnJoinedLobby()        //これは自分で作ったクラス?→違うらしい、警告がでてるけどわけわからず
    //{
    //    //とりあえずどこかのルームへ入室する
    //    PhotonNetwork.JoinRandomRoom();
    //        //PhotonNetworkのJoinRandomRoom()クラスを使って、（今ある）どこかのルームへ入れ
    //}
    //ロビーに入室した
    public override void OnConnectedToMaster()
    {
        Debug.Log("マスターサーバーに接続しました");
        PhotonNetwork.JoinRandomRoom();
    }
    //void OnJoinedRoom() { }       //これはPhotonが用意してくれているハズのクラスだが、PUN2ではMonoBehaviourには含まれていない、のだったハズ
    //このクラスの最初の継承のところを、Monobehaiviour  →  MonoBehaviourPunCallbacksとして、public override void OnJoinedRoom(){}を使う
    public override void OnJoinedRoom()         //Photonが用意してくれているクラスで、部屋に入ったら自動的に呼ばれる
    {
        Debug.Log("ルームへ入室しました");
        keyLock = true;
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        // base.OnJoinRandomFailed(returnCode, message);  これなんだろ？
        Debug.Log("ランダムルームに入室できませんでした。ルームを作ります。");
        CreateRoom();       //これは自分で作ったクラス、下にある
    }

    void CreateRoom()
    {
        Debug.Log("ルームを作っています");
        PhotonNetwork.CreateRoom("room3110");       //名前だけで大丈夫なのだろうか？
    }

    private void FixedUpdate()      //FixedUpdate()というのは初めて見た
    {
        //左クリックされたらｵﾌﾞｼﾞｪｸﾄを読み込み 
        if (Input.GetMouseButtonDown(0) && keyLock)
        {       //もし、ﾏｳｽが押され且つ変数keyLockがtrueなら(ルームに入っていたら)
            //GameObject mySyncObj = PhotonNetwork.Instantiate("Cube", new Vector3(9.0f, 0f, 0f), Quaternion.identity, 0);
            GameObject mySyncObj = PhotonNetwork.Instantiate("Cube", new Vector3(0f, 0f, 0f), Quaternion.identity, 0);
            //変数mySyncObjRBにPhotonNetwork型の(?)ｲﾝｽﾀﾝｽを作れ、Cubeという名、位置は9，0，0、回転無、(このPhotonを適用したﾌﾟﾛｼﾞｪｸﾄの)Versionはゼロ
            //動きをつけるためにRigidbodyを取得し、力を加える
            Rigidbody mySyncObjRB = mySyncObj.GetComponent<Rigidbody>();    //変数mySyncObjRBに変数mySyncObjにｱﾀｯﾁされているｺﾝﾎﾟｰﾈﾝﾄRigidbodyを入れよ
            mySyncObjRB.isKinematic = false;    //変数mySyncOjbRBの設定項目isKineticをfalseにせよ(物理演算を受けるようにせよ、ということ)
            //わかりづらいが、isKinetic=trueだと、Addforceや重力の影響は受けず、Scriptから位置をせっていしますぜ、という宣言らしい
            float rndPow = Random.Range(1.0f, 5.0f);        //ランダムな値のパワー値を入れた変数rndPowを生成
            mySyncObjRB.AddForce(Vector3.left * rndPow, ForceMode.Impulse);
            //（ｹﾞｰﾑｵﾌﾞｼﾞｪｸﾄにｱﾀｯﾁしてある）ﾘｼﾞｯﾄﾞﾎﾞﾃﾞｨ型の変数mySyncObjRBに、左方向にﾗﾝﾀﾞﾑ値のパワーを加えよ、衝撃ﾓｰﾄﾞで
        }
    }
}

//    // Update is called once per frame
//    void Update()
//    {
        
//    }
//}

using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class FastConnect : MonoBehaviourPunCallbacks
{
    public string nickName;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    void Start()
    {
        nickName = "Player" + Random.Range(1111, 9999);
        if (!PhotonNetwork.IsConnected)
        {
            Connect();
        }
    }

    void Connect()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.NickName = nickName;
        PhotonNetwork.JoinOrCreateRoom("TEST", new RoomOptions { MaxPlayers = 0 }, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.Instantiate("Player", new Vector3(PhotonNetwork.LocalPlayer.ActorNumber + 30, 0, 30), Quaternion.identity);
    }
}

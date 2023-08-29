using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public TMP_Text StatusText;
    public TMP_InputField roomInput, NickNameInput;
    public PhotonView playerPrefab;

    void Awake()
    {
        Screen.SetResolution(960, 540, false);
        //DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        print(PhotonNetwork.IsConnected);
        StatusText.text = PhotonNetwork.NetworkClientState.ToString();
        if(Input.GetKeyDown(KeyCode.Space))
            Info();
    }


    // 1. 로그인 시 서버 접속
    public void Connect()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
    

    // 2-1. 서버 연결 확인 -> 연결 완료
    public override void OnConnectedToMaster()
    {
        print("서버 접속완료");
        //PhotonNetwork.LocalPlayer.NickName = NickNameInput.text;
        //SceneManager.LoadScene(1);
    }

    public void Disconnect() => PhotonNetwork.Disconnect();

    // 2-2. 서버 연결 확인 -> 연결되지 않음
    public override void OnDisconnected(DisconnectCause cause) => print("연결 끊김");


    // 3. 로비 접속
    public void JoinLobby() => PhotonNetwork.JoinLobby();

    public override void OnJoinedLobby()
    {
        print("로비접속완료");
    } 


    // 로비에 있는 동안 룸 정보 업데이트시 불러옴
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            print(roomList[i].Name);
        }
    }

    // 방 만들기
    public void CreateRoom() => PhotonNetwork.CreateRoom(roomInput.text, new RoomOptions { MaxPlayers = 2 });

    // 특정 방 접속하기
    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(roomInput.text);
        PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);
    } 

    public void JoinOrCreateRoom()
    {
        PhotonNetwork.JoinOrCreateRoom(roomInput.text, new RoomOptions { MaxPlayers = 2 }, null);
        PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);
    }

    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
        PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);
    }

    public void LeaveRoom() => PhotonNetwork.LeaveRoom();

    public override void OnCreatedRoom() => print("방 생성 완료");

    public override void OnJoinedRoom() => print("방 접속 완료");

    public override void OnCreateRoomFailed(short returnCode, string message) => print("방 만들기 실패");

    public override void OnJoinRoomFailed(short returnCode, string message) => print("방 접속 실패");

    public override void OnJoinRandomFailed(short returnCode, string message) => print("랜덤 방 참가 실패");



    [ContextMenu("정보")]
    void Info()
    {
        if (PhotonNetwork.InRoom)
        {
            print("현재 방 이름 : " + PhotonNetwork.CurrentRoom.Name);
            print("현재 방 인원수 : " + PhotonNetwork.CurrentRoom.PlayerCount);
            print("현재 방 최대 인원수 : " + PhotonNetwork.CurrentRoom.MaxPlayers);

            string playerStr = "방에 있는 플레이어 목록 : ";
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++) playerStr += PhotonNetwork.PlayerList[i].NickName + ", ";
            print(playerStr);
        }
        else
        {
            print("접속한 인원 수 : " + PhotonNetwork.CountOfPlayers);
            print("방 개수 : " + PhotonNetwork.CountOfRooms);
            print("모든 방에 있는 인원 수 : " + PhotonNetwork.CountOfPlayersInRooms);
            print("로비인지 확인 : " + PhotonNetwork.InLobby);
            print("연결 됐는지 확인: " + PhotonNetwork.IsConnected);
        }
    }
}
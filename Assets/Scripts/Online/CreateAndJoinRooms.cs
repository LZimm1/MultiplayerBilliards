using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private InputField createInput;
    [SerializeField]
    private InputField joinInput;
    [SerializeField]
    private InputField userNameInput;
    public void CreateRoom(){
        if(userNameInput.text != ""&&createInput.text != ""){
            PhotonNetwork.CreateRoom(createInput.text);
            PhotonNetwork.NickName = userNameInput.text;
        }
    }

    public void JoinRoom(){
        if(userNameInput.text != ""&&joinInput.text != ""){
            PhotonNetwork.JoinRoom(joinInput.text);
            PhotonNetwork.NickName = userNameInput.text;
        }
    }

    public override void OnJoinedRoom(){
        PhotonNetwork.LoadLevel("Game");
    }

}

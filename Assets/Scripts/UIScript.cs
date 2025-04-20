using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;
public class UIScript : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Text userName1;
    [SerializeField]
    private Text userName2;
    [SerializeField]
    private Text winnerText;
    private turnManager turnManagerRef;
    private GameManager gameManagerRef;
    private OfflineGameManager offlineGameManagerRef;
    public GameObject stripesInd;
    public GameObject solidsInd;
    [SerializeField]
    private InputField user1,user2;
    [SerializeField]
    private GameObject musicOnButton,musicOffButton;
    public GameObject music;
    // Start is called before the first frame update
    void Start()
    {
        if(stripesInd&&solidsInd){
            stripesInd.transform.position = new Vector3(100f,0f,0f);
            solidsInd.transform.position = new Vector3(100f,0f,0f);
        }
        if(GameObject.FindWithTag("turnManager")){
            turnManagerRef = GameObject.FindWithTag("turnManager").GetComponent<turnManager>();
        }
        if(GameObject.FindWithTag("GameManager")&&GameObject.FindWithTag("GameManager").GetComponent<GameManager>()){
            gameManagerRef = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        }
        if(GameObject.FindWithTag("GameManager")&&GameObject.FindWithTag("GameManager").GetComponent<OfflineGameManager>()){
            offlineGameManagerRef = GameObject.FindWithTag("GameManager").GetComponent<OfflineGameManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!music && GameObject.FindWithTag("backgroundMusic")){
            music = GameObject.FindWithTag("backgroundMusic");
        }
        if(SceneManager.GetActiveScene().name=="Game"){
            if(userName1&&turnManagerRef.player1){
                userName1.text = turnManagerRef.player1.GetComponent<PhotonView>().Owner.NickName;
            }
            if(userName2&&turnManagerRef.player2){
                userName2.text = turnManagerRef.player2.GetComponent<PhotonView>().Owner.NickName;
            }
        }
        if(SceneManager.GetActiveScene().name=="OfflineGame"){
            if(userName1){
                userName1.text = OfflineGameManager.userName1;
            }
            if(userName2){
                userName2.text = OfflineGameManager.userName2;
            }
        }
        if(stripesInd&&solidsInd){
            if(PlayerOne.stripes){
                stripesInd.transform.position = new Vector3(-3.625f,3.95f,0f);
                solidsInd.transform.position = new Vector3(3.625f,3.95f,0f);
            }
            if(PlayerOne.solids){
                stripesInd.transform.position = new Vector3(3.625f,3.95f,0f);
                solidsInd.transform.position = new Vector3(-3.625f,3.95f,0f);
            }
            else{
                if(OfflinePlayerOne.stripes){
                    stripesInd.transform.position = new Vector3(-3.625f,3.95f,0f);
                    solidsInd.transform.position = new Vector3(3.625f,3.95f,0f);
                }
                if(OfflinePlayerOne.solids){
                    stripesInd.transform.position = new Vector3(3.625f,3.95f,0f);
                    solidsInd.transform.position = new Vector3(-3.625f,3.95f,0f);
                }
            }
        }
        if(winnerText){
            if(SceneManager.GetActiveScene().name == "GameWon"){
                winnerText.text = "Winner: " + gameManagerRef.winnerName;
            }
            else{
                winnerText.text = "Winner: " + offlineGameManagerRef.winnerName;
            }
        }
        if(musicOnButton&&musicOffButton&&music){
            if(music.GetComponent<AudioSource>().volume == 0){
                musicOnButton.transform.position = new Vector3(4.549988f,1.800003f,0f);
                musicOffButton.transform.position = new Vector3(100f,0f,0f);
            }
            else{
                musicOffButton.transform.position = new Vector3(4.549988f,1.800003f,0f);
                musicOnButton.transform.position = new Vector3(100f,0f,0f);
            }
        }

    }
    public void loadOnline(){
        if(gameManagerRef){
            PhotonNetwork.LeaveRoom();
            Destroy(gameManagerRef.gameObject);
        }
        SceneManager.LoadScene("Loading");
        
    }
    public void loadOffline(){
        SceneManager.LoadScene("OfflineMenu");
        if(offlineGameManagerRef){
            Destroy(offlineGameManagerRef.gameObject);
        }
    }
    public void loadMenu(){
        if(SceneManager.GetActiveScene().name == "GameWon"||SceneManager.GetActiveScene().name == "Lobby"){
            PhotonNetwork.Disconnect();
        }
        SceneManager.LoadScene("MainMenu");
    }
    public void loadOfflineGame(){
        if(user1.text != ""){
            OfflineGameManager.userName1 = user1.text;
        }
        else{
            OfflineGameManager.userName1 = "Player One";
        }
        if(user2.text != ""){
            OfflineGameManager.userName2 = user2.text;
        }
        else{
            OfflineGameManager.userName2 = "Player Two";
        }
        SceneManager.LoadScene("OfflineGame");
    }
    public void musicOff(){
        if(music){
            music.GetComponent<AudioSource>().volume = 0f;
        }
    }
    public void musicOn(){
        if(music){
            music.GetComponent<AudioSource>().volume = 0.05f;
        }
    }
}

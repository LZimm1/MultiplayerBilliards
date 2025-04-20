using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviourPunCallbacks
{
    private static GameManager instance;
    [SerializeField]
    private turnManager turnManagerRef;
    [SerializeField]
    private PlayerOne playerOneRef;
    [SerializeField]
    private PlayerTwo playerTwoRef;
    [SerializeField]
    private GameObject cueBall;

    private mouseRadius mouseRad;
    public static bool playerOneTIP;
    public static bool playerTwoTIP;

    private bool playerOneExtra;
    private bool playerTwoExtra;
    public PhotonView view;
    public static bool awaitingScratch;
    public static bool playerOne8Ball;
    public static bool playerTwo8Ball; 
    private GameObject pocketPicker;
    public static bool eightHitIn;
    public string winnerName;
    // TIP: Turn In Progress
    // Start is called before the first frame update
    private void Awake(){
        playerOneExtra = false;
        playerTwoExtra = false;
        awaitingScratch = false;
        eightHitIn = false;
        if(instance == null){
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else{
            Destroy(gameObject);
        }
        view = gameObject.GetComponent<PhotonView>();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(SceneManager.GetActiveScene().name == "MainMenu"){
            Destroy(gameObject);
        }
        if(!turnManagerRef&&GameObject.FindWithTag("turnManager")){
            turnManagerRef = GameObject.FindWithTag("turnManager").GetComponent<turnManager>();
        }
        if(!playerOneRef&&GameObject.FindWithTag("PlayerOne")){
            playerOneRef = GameObject.FindWithTag("PlayerOne").GetComponent<PlayerOne>();
        }
        if(!playerTwoRef&&GameObject.FindWithTag("PlayerTwo")){
            playerTwoRef = GameObject.FindWithTag("PlayerTwo").GetComponent<PlayerTwo>();
        }
        if(!cueBall&&GameObject.FindWithTag("cueBall")){
            cueBall = GameObject.FindWithTag("cueBall");
        }
        if(!mouseRad&&GameObject.FindWithTag("mouseRadius")){
            mouseRad = GameObject.FindWithTag("mouseRadius").GetComponent<mouseRadius>();
        }
        if(!pocketPicker&&GameObject.FindWithTag("pocketPicker")){
            pocketPicker = GameObject.FindWithTag("pocketPicker");
        }
        if(SceneManager.GetActiveScene().name =="Game"&&view.IsMine){
            if((turnManagerRef.player1Turn||turnManagerRef.player2Turn)&&(!playerOneRef||!playerTwoRef)&&view.IsMine){
                view.RPC("loadDC",RpcTarget.All);
            }
            if(playerOneTIP){
                playerOneTurn();
            }
            if(playerTwoTIP){
                playerTwoTurn();
            }
            if(PlayerOne.ballsStopped && PlayerTwo.ballsStopped){
                if(PlayerOne.solids){
                    if(!(GameObject.FindWithTag("1")||GameObject.FindWithTag("2")||GameObject.FindWithTag("3")||GameObject.FindWithTag("4")||GameObject.FindWithTag("5")||GameObject.FindWithTag("6")||GameObject.FindWithTag("7"))){
                        playerOne8Ball = true;
                    }
                    else{
                        playerOne8Ball = false;
                    }
                    if(!(GameObject.FindWithTag("9")||GameObject.FindWithTag("10")||GameObject.FindWithTag("11")||GameObject.FindWithTag("12")||GameObject.FindWithTag("13")||GameObject.FindWithTag("14")||GameObject.FindWithTag("15"))){
                        playerTwo8Ball = true;
                    }
                    else{
                        playerTwo8Ball = false;
                    }
                }
                if(PlayerOne.stripes){
                    if(!(GameObject.FindWithTag("1")||GameObject.FindWithTag("2")||GameObject.FindWithTag("3")||GameObject.FindWithTag("4")||GameObject.FindWithTag("5")||GameObject.FindWithTag("6")||GameObject.FindWithTag("7"))){
                        playerTwo8Ball = true;
                    }
                    else{
                        playerTwo8Ball = false;
                    }
                    if(!(GameObject.FindWithTag("9")||GameObject.FindWithTag("10")||GameObject.FindWithTag("11")||GameObject.FindWithTag("12")||GameObject.FindWithTag("13")||GameObject.FindWithTag("14")||GameObject.FindWithTag("15"))){
                        playerOne8Ball = true;
                    }
                    else{
                        playerOne8Ball = false;
                    }
                }
                if(eightHitIn){
                    if(turnManagerRef.player1Turn&&!playerOne8Ball){
                        view.RPC("setWinnerName",RpcTarget.All,false);
                    }
                    else if(turnManagerRef.player2Turn&&!playerTwo8Ball){
                        view.RPC("setWinnerName",RpcTarget.All,true);
                    }
                    else{
                        if(Ball.eightInRightPocket){
                            if(turnManagerRef.player1Turn){
                                if(!playerOneRef.scratch){
                                    view.RPC("setWinnerName",RpcTarget.All,true);
                                }
                                else{
                                    view.RPC("setWinnerName",RpcTarget.All,false);
                                }
                            }
                            if(turnManagerRef.player2Turn){
                                if(!playerTwoRef.scratch){  
                                    view.RPC("setWinnerName",RpcTarget.All,false);
                                }
                                else{
                                    view.RPC("setWinnerName",RpcTarget.All,true);
                                }
                            }
                            
                        }
                        else{
                            if(turnManagerRef.player1Turn){
                                view.RPC("setWinnerName",RpcTarget.All,false);
                            }
                            if(turnManagerRef.player2Turn){
                                view.RPC("setWinnerName",RpcTarget.All,true);
                            }
                        }
                    }
                    eightHitIn = false;
                    StartCoroutine(gameWinnerWait());
                    
                }
            }
            
        }

    }
    private void playerOneTurn(){
        if(PlayerOne.ballsStopped&&PlayerTwo.ballsStopped&&view.IsMine){
            if(Ball.ballHit==null){
                view.RPC("setAwaitingScratch",RpcTarget.All);
            }
            else if(PlayerOne.stripes&&(Ball.ballHit.CompareTag("1")||Ball.ballHit.CompareTag("2")||Ball.ballHit.CompareTag("3")||Ball.ballHit.CompareTag("4")||Ball.ballHit.CompareTag("5")||Ball.ballHit.CompareTag("6")||Ball.ballHit.CompareTag("7"))){
                view.RPC("setAwaitingScratch",RpcTarget.All);
            }
            else if(PlayerOne.solids&&(Ball.ballHit.CompareTag("9")||Ball.ballHit.CompareTag("10")||Ball.ballHit.CompareTag("11")||Ball.ballHit.CompareTag("12")||Ball.ballHit.CompareTag("13")||Ball.ballHit.CompareTag("14")||Ball.ballHit.CompareTag("15"))){
                view.RPC("setAwaitingScratch",RpcTarget.All);
            }
            else if(Ball.ballHit.CompareTag("8")&&!playerOne8Ball){
                view.RPC("setAwaitingScratch",RpcTarget.All);
            }
            if(Ball.solidsHit>Ball.stripesHit){
                playerOneRef.view.RPC("setSolids", RpcTarget.All);
                playerTwoRef.view.RPC("setStripes", RpcTarget.All);
            }
            else if(Ball.stripesHit>Ball.solidsHit){
                playerOneRef.view.RPC("setStripes", RpcTarget.All);
                playerTwoRef.view.RPC("setSolids", RpcTarget.All);
            }
            
            view.RPC("resetBallsHit",RpcTarget.All);
            if(!eightHitIn){
                if(awaitingScratch){
                    view.RPC("setScratch",RpcTarget.All,true);
                    view.RPC("resetCueBall",RpcTarget.All,false);
                    turnManagerRef.view.RPC("SwitchTurns", RpcTarget.All);
                    view.RPC("resetAwaitingScratch",RpcTarget.All);
                    view.RPC("resetExtraTurn",RpcTarget.All,true);
                }
                else if(!playerOneExtra){
                    turnManagerRef.view.RPC("SwitchTurns", RpcTarget.All);
                }
                else{
                    view.RPC("resetExtraTurn",RpcTarget.All,true);
                }
            }
        }
        
        
    }
    private void playerTwoTurn(){
        if(PlayerOne.ballsStopped&&PlayerTwo.ballsStopped&&view.IsMine){
            if(Ball.ballHit==null){
                view.RPC("setAwaitingScratch",RpcTarget.All);
            }
            else if(PlayerTwo.stripes&&(Ball.ballHit.CompareTag("1")||Ball.ballHit.CompareTag("2")||Ball.ballHit.CompareTag("3")||Ball.ballHit.CompareTag("4")||Ball.ballHit.CompareTag("5")||Ball.ballHit.CompareTag("6")||Ball.ballHit.CompareTag("7"))){
                view.RPC("setAwaitingScratch",RpcTarget.All);
            }
            else if(PlayerTwo.solids&&(Ball.ballHit.CompareTag("9")||Ball.ballHit.CompareTag("10")||Ball.ballHit.CompareTag("11")||Ball.ballHit.CompareTag("12")||Ball.ballHit.CompareTag("13")||Ball.ballHit.CompareTag("14")||Ball.ballHit.CompareTag("15"))){
                view.RPC("setAwaitingScratch",RpcTarget.All);
            }
            else if(Ball.ballHit.CompareTag("8")&&!playerTwo8Ball){
                view.RPC("setAwaitingScratch",RpcTarget.All);
            }
            if(Ball.solidsHit>Ball.stripesHit){
                playerTwoRef.view.RPC("setSolids", RpcTarget.All);
                playerOneRef.view.RPC("setStripes", RpcTarget.All);
            }
            else if(Ball.stripesHit>Ball.solidsHit){
                playerTwoRef.view.RPC("setStripes", RpcTarget.All);
                playerOneRef.view.RPC("setSolids", RpcTarget.All);
            }
            
            
            view.RPC("resetBallsHit",RpcTarget.All);
            if(!eightHitIn){
                if(awaitingScratch){
                    view.RPC("setScratch",RpcTarget.All,false);
                    view.RPC("resetCueBall",RpcTarget.All,true);
                    turnManagerRef.view.RPC("SwitchTurns", RpcTarget.All);
                    view.RPC("resetAwaitingScratch",RpcTarget.All);
                    view.RPC("resetExtraTurn",RpcTarget.All,false);
                }
                else if(!playerTwoExtra){
                    turnManagerRef.view.RPC("SwitchTurns", RpcTarget.All);
                }
                else{
                    view.RPC("resetExtraTurn",RpcTarget.All,false);
                }
            }
        }
        
    }
    [PunRPC]
    void setAwaitingScratch(){
        awaitingScratch = true;
    }
    [PunRPC]
    void resetExtraTurn(bool playerOne){
        if(playerOne){
            playerOneExtra = false;
        }
        else{
            playerTwoExtra = false;
        }
    }
    [PunRPC]
    void giveExtraTurn(bool playerOne){
        if(playerOne){
            playerOneExtra = true;
        }
        else{
            playerTwoExtra = true;
        }
    }
    [PunRPC]
    void resetCueBall(bool p1Turn){
        if(cueBall.transform.position.Equals(new Vector3(100f,0f,0f))){
            if(p1Turn){
                PlayerOne.cueBallUnplaced = true;
            }
            else{
                PlayerTwo.cueBallUnplaced = true;
            }
        }
        cueBall.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        cueBall.GetComponent<Rigidbody2D>().totalForce = Vector3.zero;
        

    }
    [PunRPC]
    void setScratch(bool playerOne){
        if(playerOne){
            playerTwoRef.scratch = true;
        }
        else{
            playerOneRef.scratch = true;
        }
        playerOneExtra = false;
        playerTwoExtra = false;
    }
    [PunRPC]
    void resetAwaitingScratch(){
        awaitingScratch = false;
    }
    [PunRPC]
    void resetBallsHit(){
        Ball.solidsHit=0;
        Ball.stripesHit=0;
        Ball.ballHit = null;
        playerOneTIP = false;
        playerTwoTIP = false;
    }
    [PunRPC]
    void chosePocket(Vector3 position){
        if(pocketPicker){
            pocketPicker.transform.position = position;
        }
    }
    IEnumerator gameWinnerWait(){
        view.RPC("setTurnsFalse",RpcTarget.All);
        yield return new WaitForSeconds(1.5f);
        view.RPC("setEightInRightPocket",RpcTarget.All,false);
        view.RPC("loadGameWinnerScene",RpcTarget.All);
    }
    [PunRPC]
    void eightBallHitIn(){
        eightHitIn = true;
    }
    [PunRPC]
    void loadGameWinnerScene(){
        SceneManager.LoadScene("GameWon");
    }
    [PunRPC]
    void setEightInRightPocket(bool correct){
        if(correct){
            Ball.eightInRightPocket = true;
        }
        else{
            Ball.eightInRightPocket = false;
        }
    }
    [PunRPC]
    void setWinnerName(bool playerOne){
        if(playerOne){
            winnerName = playerOneRef.GetComponent<PhotonView>().Owner.NickName;
        }
        else{
            winnerName = playerTwoRef.GetComponent<PhotonView>().Owner.NickName;
        }
    }
    [PunRPC]
    void setTurnsFalse(){
        turnManagerRef.player1Turn = false;
        turnManagerRef.player2Turn = false;
    }
    [PunRPC]
    void loadDC(){
        SceneManager.LoadScene("Disconnect");
    }

}

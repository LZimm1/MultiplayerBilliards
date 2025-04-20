using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class OfflineGameManager : MonoBehaviour
{
    private static OfflineGameManager instance;
    [SerializeField]
    private OfflineTurnManager turnManagerRef;
    [SerializeField]
    private OfflinePlayerOne playerOneRef;
    [SerializeField]
    private OfflinePlayerTwo playerTwoRef;
    [SerializeField]
    private GameObject cueBall;

    private OfflineMouseRadius mouseRad;
    public static bool playerOneTIP;
    public static bool playerTwoTIP;

    private bool playerOneExtra;
    private bool playerTwoExtra;
    public static bool awaitingScratch;
    public static bool playerOne8Ball;
    public static bool playerTwo8Ball; 
    public GameObject pocketPicker;
    public static bool eightHitIn;
    public string winnerName;
    public static string userName1;
    public static string userName2;
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
            turnManagerRef = GameObject.FindWithTag("turnManager").GetComponent<OfflineTurnManager>();
        }
        if(!playerOneRef&&GameObject.FindWithTag("PlayerOne")){
            playerOneRef = GameObject.FindWithTag("PlayerOne").GetComponent<OfflinePlayerOne>();
        }
        if(!playerTwoRef&&GameObject.FindWithTag("PlayerTwo")){
            playerTwoRef = GameObject.FindWithTag("PlayerTwo").GetComponent<OfflinePlayerTwo>();
        }
        if(!cueBall&&GameObject.FindWithTag("cueBall")){
            cueBall = GameObject.FindWithTag("cueBall");
        }
        if(!mouseRad&&GameObject.FindWithTag("mouseRadius")){
            mouseRad = GameObject.FindWithTag("mouseRadius").GetComponent<OfflineMouseRadius>();
        }
        if(!pocketPicker&&GameObject.FindWithTag("pocketPicker")){
            pocketPicker = GameObject.FindWithTag("pocketPicker");
        }
        if(SceneManager.GetActiveScene().name =="OfflineGame"){
            if(playerOneTIP){
                playerOneTurn();
            }
            if(playerTwoTIP){
                playerTwoTurn();
            }
            if(OfflinePlayerOne.ballsStopped && OfflinePlayerTwo.ballsStopped){
                if(OfflinePlayerOne.solids){
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
                if(OfflinePlayerOne.stripes){
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
            }
            if(eightHitIn){
                if(turnManagerRef.player1Turn&&!playerOne8Ball){
                    setWinnerName(false);
                }
                else if(turnManagerRef.player2Turn&&!playerTwo8Ball){
                    setWinnerName(true);
                }
                else{
                    if(OfflineBall.eightInRightPocket){
                        if(turnManagerRef.player1Turn){
                            if(!playerOneRef.scratch){
                                setWinnerName(true);
                            }
                            else{
                                setWinnerName(false);
                            }
                        }
                        if(turnManagerRef.player2Turn){
                            if(!playerTwoRef.scratch){  
                                setWinnerName(false);
                            }
                            else{
                                setWinnerName(true);
                            }
                        }
                        
                    }
                    else{
                        if(turnManagerRef.player1Turn){
                            setWinnerName(false);
                        }
                        if(turnManagerRef.player2Turn){
                            setWinnerName(true);
                        }
                    }
                }
                eightHitIn = false;
                StartCoroutine(gameWinnerWait());
                
            }
        }

    }
    private void playerOneTurn(){
        if(OfflinePlayerOne.ballsStopped&&OfflinePlayerTwo.ballsStopped){
            if(OfflineBall.ballHit==null){
                setAwaitingScratch();
            }
            else if(OfflinePlayerOne.stripes&&(OfflineBall.ballHit.CompareTag("1")||OfflineBall.ballHit.CompareTag("2")||OfflineBall.ballHit.CompareTag("3")||OfflineBall.ballHit.CompareTag("4")||OfflineBall.ballHit.CompareTag("5")||OfflineBall.ballHit.CompareTag("6")||OfflineBall.ballHit.CompareTag("7"))){
                setAwaitingScratch();
            }
            else if(OfflinePlayerOne.solids&&(OfflineBall.ballHit.CompareTag("9")||OfflineBall.ballHit.CompareTag("10")||OfflineBall.ballHit.CompareTag("11")||OfflineBall.ballHit.CompareTag("12")||OfflineBall.ballHit.CompareTag("13")||OfflineBall.ballHit.CompareTag("14")||OfflineBall.ballHit.CompareTag("15"))){
                setAwaitingScratch();
            }
            else if(OfflineBall.ballHit.CompareTag("8")&&!playerOne8Ball){
                setAwaitingScratch();
            }
            if(OfflineBall.solidsHit>OfflineBall.stripesHit){
                playerOneRef.setSolids();
                playerTwoRef.setStripes();
            }
            else if(OfflineBall.stripesHit>OfflineBall.solidsHit){
                playerOneRef.setStripes();
                playerTwoRef.setSolids();
            }
            resetBallsHit();
            if(!eightHitIn){
                if(awaitingScratch){
                    setScratch(true);
                    resetCueBall(false);
                    turnManagerRef.SwitchTurns();
                    resetAwaitingScratch();
                    resetExtraTurn(true);
                }
                else if(!playerOneExtra){
                    turnManagerRef.SwitchTurns();
                }
                else{
                    resetExtraTurn(true);
                }
            }
        }  
    }
    private void playerTwoTurn(){
        if(OfflinePlayerOne.ballsStopped&&OfflinePlayerTwo.ballsStopped){
            if(OfflineBall.ballHit==null){
                setAwaitingScratch();
            }
            else if(OfflinePlayerTwo.stripes&&(OfflineBall.ballHit.CompareTag("1")||OfflineBall.ballHit.CompareTag("2")||OfflineBall.ballHit.CompareTag("3")||OfflineBall.ballHit.CompareTag("4")||OfflineBall.ballHit.CompareTag("5")||OfflineBall.ballHit.CompareTag("6")||OfflineBall.ballHit.CompareTag("7"))){
                setAwaitingScratch();
            }
            else if(OfflinePlayerTwo.solids&&(OfflineBall.ballHit.CompareTag("9")||OfflineBall.ballHit.CompareTag("10")||OfflineBall.ballHit.CompareTag("11")||OfflineBall.ballHit.CompareTag("12")||OfflineBall.ballHit.CompareTag("13")||OfflineBall.ballHit.CompareTag("14")||OfflineBall.ballHit.CompareTag("15"))){
                setAwaitingScratch();
            }
            else if(OfflineBall.ballHit.CompareTag("8")&&!playerTwo8Ball){
                setAwaitingScratch();
            }
            if(OfflineBall.solidsHit>OfflineBall.stripesHit){
                playerTwoRef.setSolids();
                playerOneRef.setStripes();
            }
            else if(OfflineBall.stripesHit>OfflineBall.solidsHit){
                playerTwoRef.setStripes();
                playerOneRef.setSolids();
            }
            resetBallsHit();
            if(!eightHitIn){
                if(awaitingScratch){
                    setScratch(false);
                    resetCueBall(true);
                    turnManagerRef.SwitchTurns();
                    resetAwaitingScratch();
                    resetExtraTurn(false);
                }
                else if(!playerTwoExtra){
                    turnManagerRef.SwitchTurns();
                }
                else{
                    resetExtraTurn(false);
                }
            }
        }
    }
    void setAwaitingScratch(){
        awaitingScratch = true;
    }
    void resetExtraTurn(bool playerOne){
        if(playerOne){
            playerOneExtra = false;
        }
        else{
            playerTwoExtra = false;
        }
    }
    public void giveExtraTurn(bool playerOne){
        if(playerOne){
            playerOneExtra = true;
        }
        else{
            playerTwoExtra = true;
        }
    }
    void resetCueBall(bool p1Turn){
        if(cueBall.transform.position.Equals(new Vector3(100f,0f,0f))){
            if(p1Turn){
                OfflinePlayerOne.cueBallUnplaced = true;
            }
            else{
                OfflinePlayerTwo.cueBallUnplaced = true;
            }
        }
        cueBall.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        cueBall.GetComponent<Rigidbody2D>().totalForce = Vector3.zero;
        

    }
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
    public void chosePocket(Vector3 position){
        if(pocketPicker){
            pocketPicker.transform.position = position;
        }
    }
    void resetAwaitingScratch(){
        awaitingScratch = false;
    }
    void resetBallsHit(){
        OfflineBall.solidsHit=0;
        OfflineBall.stripesHit=0;
        OfflineBall.ballHit = null;
        playerOneTIP = false;
        playerTwoTIP = false;
    }
    IEnumerator gameWinnerWait(){
        setTurnsFalse();
        yield return new WaitForSeconds(1.5f);
        setEightInRightPocket(false);
        loadGameWinnerScene();
    }
    public void eightBallHitIn(){
        eightHitIn = true;
    }
    void loadGameWinnerScene(){
        SceneManager.LoadScene("OfflineGameWon");
    }
    public void setEightInRightPocket(bool correct){
        if(correct){
            OfflineBall.eightInRightPocket = true;
        }
        else{
            OfflineBall.eightInRightPocket = false;
        }
    }
    void setWinnerName(bool playerOne){
        if(playerOne){
            winnerName = userName1;
        }
        else{
            winnerName = userName2;
        }
    }
    void setTurnsFalse(){
        turnManagerRef.player1Turn = false;
        turnManagerRef.player2Turn = false;
    }

}

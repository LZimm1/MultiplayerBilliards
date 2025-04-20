using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class PlayerOne : MonoBehaviour
{
    private Vector3 endPosition;
    private Vector3 startPosition;
    private GameObject line;
    public static Vector3 direction;
    private GameObject cueBall;
    private Vector3 cueStick;
    public static bool ballsStopped;
    [SerializeField]
    public GameObject[] balls= new GameObject[16];
    public PhotonView view;
    public turnManager turnManagerRef;
    public bool myTurn;
    float rotationZ = -45.0f;
    public static bool solids;
    public static bool stripes;
    public static bool ballPlaced;
    public bool scratch;
    private mouseRadius mouseRad;
    public static bool cueBallUnplaced ;
    // Start is called before the first frame update

    void Start()
    {
        ballsStopped = false;
        solids = false;
        stripes = false;
        ballPlaced = false;
        scratch = false;
        cueBallUnplaced = false;
        turnManagerRef = GameObject.FindWithTag("turnManager").GetComponent<turnManager>();
        line = GameObject.FindWithTag("line");
        cueBall = GameObject.FindWithTag("cueBall");
        balls[0] = cueBall;
        for(int i = 1; i < 16; i++){
            balls[i] = GameObject.FindWithTag(i.ToString());
        }
        view = gameObject.GetComponent<PhotonView>();

        mouseRad = GameObject.FindWithTag("mouseRadius").GetComponent<mouseRadius>();
    }
    // Update is called once per frame
    void Update()
    {
        if(view.IsMine&&myTurn){
            turnManagerRef.view.RPC("movingArrowMoveInitial",RpcTarget.All,ballPlaced);
        }
        if(turnManagerRef.player1Turn){
            myTurn = true;
        }
        else{
            myTurn = false;
        }
        if(cueBallUnplaced&&myTurn&&view.IsMine){
            turnManagerRef.view.RPC("moveMovingArrow",RpcTarget.All,scratch);
        
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0f;
            if(pos.x>5.23f){
                pos.x=5.23f;
            }
            if(pos.x<-5.23f){
                pos = new Vector3(-5.23f,pos.y,0f);
            }
            if(pos.y<-3.68f){
                pos = new Vector3(pos.x,-3.68f,0f);
            }
            if(pos.y>2.38f){
                pos = new Vector3(pos.x,2.38f,0f);
            }
            StartCoroutine(moveMouseRad(pos));
            if(Input.GetMouseButtonDown(1)){
                cueBallUnplaced = false;
            }
        }
        else{
            if(!ballPlaced&&view.IsMine&&myTurn&&Input.GetMouseButton(1)){
                Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                pos.z = 0f;
                if(pos.x>-3f){
                    pos.x=-3f;
                }
                if(pos.x<-5.23f){
                    pos = new Vector3(-5.23f,pos.y,0f);
                }
                if(pos.y<-3.68f){
                    pos = new Vector3(pos.x,-3.68f,0f);
                }
                if(pos.y>2.38f){
                    pos = new Vector3(pos.x,2.38f,0f);
                }
                turnManagerRef.view.RPC("moveCueBall",RpcTarget.All,pos);
            }
            
            else if (myTurn && view.IsMine){
                if(scratch){
                    turnManagerRef.view.RPC("moveMovingArrow",RpcTarget.All,scratch);
                    if(Input.GetMouseButton(1)){
                        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        pos.z = 0f;
                        if(pos.x>5.23f){
                            pos.x=5.23f;
                        }
                        if(pos.x<-5.23f){
                            pos = new Vector3(-5.23f,pos.y,0f);
                        }
                        if(pos.y<-3.68f){
                            pos = new Vector3(pos.x,-3.68f,0f);
                        }
                        if(pos.y>2.38f){
                            pos = new Vector3(pos.x,2.38f,0f);
                        }
                        StartCoroutine(moveMouseRad(pos));
                    }
                }
                if(GameManager.playerOne8Ball&&!mouse.placed){
                    mouse.needToPlace = true;
                }
                else if(ballsStopped){
                    Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    
                    if(Input.GetMouseButtonDown(0)){
                        startDrag(worldPosition);
                    }
                    else if(Input.GetMouseButton(0)){
                        continueDrag(worldPosition);
                    }
                    else if(Input.GetMouseButtonUp(0)){
                        endDrag();
                    }
                }
            }
        }
        if(direction.magnitude > 2.2){
            direction.Normalize();
            direction*=2.2f;
        }
        if(ballsStopped&&myTurn&&view.IsMine){
            cueStick = new Vector3(cueBall.transform.position.x+direction.x,cueBall.transform.position.y+direction.y, cueBall.transform.position.z+direction.z);
            turnManagerRef.view.RPC("SyncSticks",RpcTarget.All,cueStick,rotationZ);
        }
        if(view.IsMine){
            view.RPC("setBallStatus",RpcTarget.All);
        }
        if(!turnManagerRef.player1Turn&&!turnManagerRef.player2Turn){
            cueStick = new Vector3(100f,0f,0f);
            turnManagerRef.view.RPC("SyncSticks",RpcTarget.All,cueStick,rotationZ);
            view.RPC("syncLine",RpcTarget.All,new Vector3(100f,0f,0f),rotationZ);
        }
    }
        
        
            
    private void startDrag(Vector3 worldPosition){ // called when the player clicks down initially
        startPosition = worldPosition; // sets start position to the position of the mouse
        ballPlaced = true; // places the ball in case it was not already placed after a scratch
    }
    private void continueDrag(Vector3 worldPosition){ // called when player continues dragging
        ballPlaced = true; // makes the ball is placed (since the player could've cancelled their hit)
        if(Input.GetButtonDown("Cancel")||Input.GetMouseButton(1)){ // if player hits esc key or right clicks, cancel aiming
            direction = Vector3.zero;
            startPosition = worldPosition;
            endPosition = worldPosition;
            endDrag();
        }
        else{
            endPosition = worldPosition; // sets the end position to the position of the mouse
            direction = endPosition - startPosition; // sets the 3D vector direction to the difference between initial and final mouse positions
            rotationZ = (Mathf.Atan2(startPosition.y-endPosition.y, startPosition.x-endPosition.x) * Mathf.Rad2Deg)-45; // sets rotationZ to the angle of the vector using tan^-1(y/x), converts it to degrees
            // -45 is used as an offset 
            view.RPC("syncLine",RpcTarget.All,cueBall.transform.position,rotationZ); // puts the line indicating where a player is aiming at an angle of rotationZ
            cueStick = new Vector3 (cueBall.transform.position.x,cueBall.transform.position.y,cueBall.transform.position.z); // moves the cueStick, which is synced elsewhere
        }
    }
    private void endDrag(){ // called when the player lets go without cancelling
        ballPlaced = true; // now that they're done hitting, the ball should definitely be placed down
        direction = endPosition - startPosition; // sets the 3D vector direction one last time, before it was only for show
        view.RPC("syncLine",RpcTarget.All,new Vector3(100f,0f,0f),rotationZ); // moves the aiming line off screen
        
        if(direction.magnitude > 2.2){
            direction.Normalize();
            direction*=2.2f; // caps the direction vector to a magnitude of 2.2
        } 
        if(direction.magnitude>0.5){ // hits the ball if the magnitude of direction is more than .5
            cueStick = new Vector3 (cueBall.transform.position.x,cueBall.transform.position.y,cueBall.transform.position.z);
            StartCoroutine(clearBoard());
            
        }
        else{ // the magnitude of direction was very little, so it is assumed the player didn't even mean to click, and the hit is cancelled
            direction = Vector3.zero;
        }
    }
    [PunRPC]
    private void setBallStatus(){
        // first check if all balls are stopped
        ballsStopped = true; // assumes that all balls are stopped
        for(int i = 0; i < 16; i++){
            if(balls[i]){
                if(balls[i].GetComponent<Rigidbody2D>().velocity.magnitude>.075||balls[i].GetComponent<Rigidbody2D>().totalForce.magnitude != 0){
                    ballsStopped = false; // iterates through all 16 balls, and makes ballsStopped false if any have non-negligible velocity or an applied force > 0
                }
            }
        } // now, the variable ballsStopped is correct

        if(ballsStopped){
            for(int i = 0; i < 16; i++){
                if(balls[i]){ // balls stopped means velocity is at least negligible, this condition will make the negligible velocity 0
                    balls[i].GetComponent<Rigidbody2D>().velocity = Vector3.zero; // makes velocity 0
                    balls[i].GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY; // Unity-specific command, locks balls from moving
                    balls[i].GetComponent<Rigidbody2D>().freezeRotation = true; // Unity-specific command, locks balls from rotating
                    if(balls[i].transform.position.x == -100f&&!balls[i].Equals(Ball.ballHit)){ 
                        Destroy(balls[i]); // pocketed balls are sent to x = -100, so this destroys the pocketed ball, unless it was the first ball hit
                        // if it was the first ball hit by the cueball, it needs to be saved so other parts of the program can reference it checking for a scratch
                    }
                }
                
            }
        }
        else{
            for(int i = 0; i < 16; i++){
                if(balls[i]){ // since ballsStopped is false, it's in the middle of a turn
                    balls[i].GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;  // Unlocks balls from moving
                    balls[i].GetComponent<Rigidbody2D>().freezeRotation = false; // Unlocks balls from rotating
                }
            }
        }
        
    }
    IEnumerator clearBoard(){ // called when player lets go of cursor, hitting the cue ball
        view.RPC("resetScratch",RpcTarget.All); // sets scratch to false on both game instances
        turnManagerRef.view.RPC("moveMovingArrow",RpcTarget.All,scratch); // moves the arrow that indicates a scratch off the screen
        view.RPC("setTIP",RpcTarget.All); // TIP = Turn in Progress. Sets TIP to true
        turnManagerRef.view.RPC("HitBall", RpcTarget.All,direction); // applies a force of the 3D Vector 'direction' to the cue ball
        direction = Vector3.zero; // since the player just hit the ball, the vector direction is reset to 0 for next turn
        turnManagerRef.view.RPC("SyncSticks",RpcTarget.All,cueStick,rotationZ); // cue sticks are moved to the position of the ball to strike it
        yield return new WaitForSeconds(.2f); // delay so that the cue stick remains on screen for 0.20 seconds
        cueStick = new Vector3(100f,0f,0f); // move the cue stick off the screen
        turnManagerRef.view.RPC("SyncSticks",RpcTarget.All,cueStick,rotationZ); // syncs the cue sticks position on both screens
    }
    [PunRPC]
    public void setSolids(){
        stripes = false;
        solids = true;
    }
    [PunRPC]
    public void setStripes(){
        stripes = true;
        solids = false;
    }
    IEnumerator moveMouseRad(Vector3 position){
        view.RPC("moveMouseRadLoc",RpcTarget.All,position);
        yield return new WaitForSeconds(Time.deltaTime);
        if(mouseRad.canPlace){
            turnManagerRef.view.RPC("moveCueBall",RpcTarget.All,position);
            turnManagerRef.view.RPC("moveMovingArrow",RpcTarget.All,scratch);
        }
    }
    [PunRPC]
    void moveMouseRadLoc(Vector3 position){
        mouseRad.transform.position = position;
    }
    [PunRPC]
    void resetScratch(){
        scratch = false;
    }
    [PunRPC]
    void setTIP(){
        GameManager.playerOneTIP = true;
    }
    [PunRPC]
    void syncLine(Vector3 position,float rotationZ){
        line.transform.position = new Vector3 (position.x,position.y,position.z);
        line.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
    }

}

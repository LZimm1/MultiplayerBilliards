using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class OfflinePlayerOne : MonoBehaviour
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
    public OfflineTurnManager turnManagerRef;
    public bool myTurn;
    float rotationZ = -45.0f;
    public static bool solids;
    public static bool stripes;
    public static bool ballPlaced;
    public bool scratch;
    private OfflineMouseRadius mouseRad;
    public static bool cueBallUnplaced;
    // Start is called before the first frame update
    void Start()
    {
        ballsStopped = false;
        solids = false;
        stripes = false;
        ballPlaced = false;
        scratch = false;
        cueBallUnplaced = false;
        turnManagerRef = GameObject.FindWithTag("turnManager").GetComponent<OfflineTurnManager>();
        line = GameObject.FindWithTag("line");
        cueBall = GameObject.FindWithTag("cueBall");
        balls[0] = cueBall;
        for(int i = 1; i < 16; i++){
            balls[i] = GameObject.FindWithTag(i.ToString());
        }

        mouseRad = GameObject.FindWithTag("mouseRadius").GetComponent<OfflineMouseRadius>();
    }
    // Update is called once per frame
    void Update()
    {
        if(OfflineBall.ballHit){
            Debug.Log(OfflineBall.ballHit.tag);
        }
        if(myTurn){
            turnManagerRef.movingArrowMoveInitial(ballPlaced);
        }
        if(turnManagerRef.player1Turn){
            myTurn = true;
        }
        else{
            myTurn = false;
        }
        if(cueBallUnplaced&&myTurn){
            turnManagerRef.moveMovingArrow(scratch);
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
            if(!ballPlaced&&myTurn&&Input.GetMouseButton(1)){
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
                turnManagerRef.moveCueBall(pos);
            }
            
            else if (myTurn){
                if(scratch){
                    turnManagerRef.moveMovingArrow(scratch);
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
                if(OfflineGameManager.playerOne8Ball&&!OfflineMouse.placed){
                    OfflineMouse.needToPlace = true;
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
        if(ballsStopped&&myTurn){
            cueStick = new Vector3(cueBall.transform.position.x+direction.x,cueBall.transform.position.y+direction.y, cueBall.transform.position.z+direction.z);
            turnManagerRef.SyncSticks(cueStick,rotationZ);
        }

        setBallStatus();
        if(!turnManagerRef.player1Turn&&!turnManagerRef.player2Turn){
            cueStick = new Vector3(100f,0f,0f);
            turnManagerRef.SyncSticks(cueStick,rotationZ);
            syncLine(new Vector3(100f,0f,0f),rotationZ);
        }
    }
        
        
            
    private void startDrag(Vector3 worldPosition){
        startPosition = worldPosition;
        ballPlaced = true;
    }
    private void continueDrag(Vector3 worldPosition){
        ballPlaced = true;
        if(Input.GetButtonDown("Cancel")||Input.GetMouseButton(1)){
            direction = Vector3.zero;
            startPosition = worldPosition;
            endPosition = worldPosition;
            endDrag();
        }
        else{
            endPosition = worldPosition;
            direction = endPosition - startPosition;
            rotationZ = (Mathf.Atan2(startPosition.y-endPosition.y, startPosition.x-endPosition.x) * Mathf.Rad2Deg)-45;
            syncLine(cueBall.transform.position,rotationZ);
            cueStick = new Vector3 (cueBall.transform.position.x,cueBall.transform.position.y,cueBall.transform.position.z);
        }
    }
    private void endDrag(){
        ballPlaced = true;
        direction = endPosition - startPosition;
        syncLine(new Vector3(100f,0f,0f),rotationZ);
        
        if(direction.magnitude > 2.2){
            direction.Normalize();
            direction*=2.2f;
        }
        if(direction.magnitude>0.5){
            cueStick = new Vector3 (cueBall.transform.position.x,cueBall.transform.position.y,cueBall.transform.position.z);
            StartCoroutine(clearBoard());
            
        }
        else{
            direction = Vector3.zero;
        }
    }
    private void setBallStatus(){
        ballsStopped = true;
        for(int i = 0; i < 16; i++){
            if(balls[i]){
                if(balls[i].GetComponent<Rigidbody2D>().velocity.magnitude>.075||balls[i].GetComponent<Rigidbody2D>().totalForce.magnitude != 0){
                    ballsStopped = false;
                }
            }
        }
        if(ballsStopped){
            for(int i = 0; i < 16; i++){
                if(balls[i]){
                    balls[i].GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                    balls[i].GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
                    balls[i].GetComponent<Rigidbody2D>().freezeRotation = true;
                    if(balls[i].transform.position.x == -100f&&!balls[i].Equals(OfflineBall.ballHit)){
                        Destroy(balls[i]);
                    }
                }
                
            }
        }
        else{
            for(int i = 0; i < 16; i++){
                if(balls[i]){
                    balls[i].GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                    balls[i].GetComponent<Rigidbody2D>().freezeRotation = false;
                }
            }
        }
        
    }
    IEnumerator clearBoard(){
        resetScratch();
        turnManagerRef.moveMovingArrow(scratch);
        setTIP();
        turnManagerRef.HitBall(direction);
        direction = Vector3.zero;

        turnManagerRef.SyncSticks(cueStick,rotationZ);
        yield return new WaitForSeconds(.2f);
        
        cueStick = new Vector3(100f,0f,0f);
        turnManagerRef.SyncSticks(cueStick,rotationZ);
    }
    public void setSolids(){
        stripes = false;
        solids = true;
    }
    public void setStripes(){
        stripes = true;
        solids = false;
    }
    IEnumerator moveMouseRad(Vector3 position){
        moveMouseRadLoc(position);
        yield return new WaitForSeconds(Time.deltaTime);
        if(mouseRad.canPlace){
            turnManagerRef.moveCueBall(position);
            turnManagerRef.moveMovingArrow(scratch);
        }
    }
    void moveMouseRadLoc(Vector3 position){
        mouseRad.transform.position = position;
    }
    void resetScratch(){
        scratch = false;
    }
    void setTIP(){
        OfflineGameManager.playerOneTIP = true;
    }
    void syncLine(Vector3 position,float rotationZ){
        line.transform.position = new Vector3 (position.x,position.y,position.z);
        line.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
    }

}

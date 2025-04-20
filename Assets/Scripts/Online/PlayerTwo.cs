using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class PlayerTwo : MonoBehaviour
{
    private Vector3 endPosition;
    private Vector3 startPosition;
    private GameObject line;
    public static Vector3 direction;
    private GameObject cueBall;
    private Vector3 cueStick;
    public static bool ballsStopped = false;
    [SerializeField]
    public GameObject[] balls= new GameObject[16];
    public PhotonView view;
    public bool myTurn;
    public turnManager turnManagerRef;
    float rotationZ = -45.0f;
    public static bool stripes;
    public static bool solids;
    public bool scratch;
    
    private mouseRadius mouseRad;
    public static bool cueBallUnplaced;
    void Awake(){
    }
    // Start is called before the first frame update
    void Start()
    {
        ballsStopped = false;
        stripes = false;
        solids = false;
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
        if(turnManagerRef.player2Turn){
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
        else if(myTurn&&view.IsMine){
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
            if(GameManager.playerTwo8Ball&&!mouse.placed){
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

    private void startDrag(Vector3 worldPosition){
        startPosition = worldPosition;
    }
    private void continueDrag(Vector3 worldPosition){
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
            view.RPC("syncLine",RpcTarget.All,cueBall.transform.position,rotationZ);
            cueStick = new Vector3 (cueBall.transform.position.x,cueBall.transform.position.y,cueBall.transform.position.z);
        }
    }
    private void endDrag(){
        
        direction = endPosition - startPosition;
        view.RPC("syncLine",RpcTarget.All,new Vector3(100f,0f,0f),rotationZ);
        
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
    [PunRPC]
    private void setBallStatus(){
        ballsStopped = true;
        for(int i = 0; i < 16; i++){
            if(balls[i]){
                if(balls[i].GetComponent<Rigidbody2D>().velocity.magnitude>0.075||balls[i].GetComponent<Rigidbody2D>().totalForce.magnitude != 0){
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
                    if(balls[i].transform.position.x == -100f&&!balls[i].Equals(Ball.ballHit)){
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
        
        view.RPC("resetScratch",RpcTarget.All);
        turnManagerRef.view.RPC("moveMovingArrow",RpcTarget.All,scratch);
        view.RPC("setTIP",RpcTarget.All);
        turnManagerRef.view.RPC("HitBall", RpcTarget.All,direction);
        direction = Vector3.zero;
        turnManagerRef.view.RPC("SyncSticks",RpcTarget.All,cueStick,rotationZ);
        yield return new WaitForSeconds(.2f);
        cueStick = new Vector3(100f,0f,0f);
        turnManagerRef.view.RPC("SyncSticks",RpcTarget.All,cueStick,rotationZ);
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
        GameManager.playerTwoTIP = true;
    }
    [PunRPC]
    void syncLine(Vector3 position,float rotationZ){
        line.transform.position = new Vector3 (position.x,position.y,position.z);
        line.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
    }

}

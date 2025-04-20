using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
/*
public class Player : MonoBehaviour
{
    private Vector3 endPosition;
    private Vector3 startPosition;

    private GameObject line;
    public static Vector3 direction;


    private GameObject cueBall;

    private GameObject cueStick;
    private static bool ballsStopped = false;

    [SerializeField]
    private GameObject[] balls= new GameObject[16];

    private PhotonView view;
    private turnManager turnManagerRef;
    public bool myTurn;
    void Awake(){
        turnManagerRef = GameObject.FindWithTag("turnManager").GetComponent<turnManager>();
        if(turnManagerRef.player1 == null){
            // turnManagerRef.player1 = gameObject.GetComponent<Player>();
        }
        else if(turnManagerRef.player2 == null){
            // turnManagerRef.player2 = gameObject.GetComponent<Player>();
        }
        if(turnManagerRef.player1 && turnManagerRef.player2){
            turnManagerRef.player1Turn=true;
            turnManagerRef.player2Turn=false;
        }


    }
    // Start is called before the first frame update
    void Start()
    {
        
        line = GameObject.FindWithTag("line");
        cueBall = GameObject.FindWithTag("cueBall");
        if(gameObject.Equals(turnManagerRef.player1.gameObject)){
            cueStick = GameObject.FindWithTag("cueStick1");
        }
        else if(gameObject.Equals(turnManagerRef.player2.gameObject)){
            cueStick = GameObject.FindWithTag("cueStick2");
        }
        balls[0] = cueBall;
        for(int i = 1; i < 16; i++){
            balls[i] = GameObject.FindWithTag(i.ToString());
        }
        line.transform.position = new Vector3(100f, 0f, 0f);
        cueStick.transform.position = new Vector3(100f, 0f, 0f);
        view = gameObject.GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if(turnManagerRef.player1&&gameObject.Equals(turnManagerRef.player1.gameObject)&&turnManagerRef.player1Turn){
            myTurn=true;
        }
        else if(turnManagerRef.player2&&gameObject.Equals(turnManagerRef.player2.gameObject)&&turnManagerRef.player2Turn){
            myTurn=true;
        }
        else{
            myTurn=false;
        }

        ballsStopped = true;
        for(int i = 0; i < 16; i++){
            if(balls[i]){
                if(balls[i].GetComponent<Rigidbody2D>().velocity.magnitude>0.05){
                    ballsStopped = false;
                }
            }
        }
        if(ballsStopped){
            for(int i = 0;i < 16; i++){
                if(balls[i]){
                    balls[i].GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                }
            }
        }
        
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if(ballsStopped&&myTurn&&view.IsMine){
            cueStick.transform.position = new Vector3(cueBall.transform.position.x,cueBall.transform.position.y,cueBall.transform.position.z);
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
        if(direction.magnitude > 2){
            direction.Normalize();
            direction*=2;
        }
        if(ballsStopped&&myTurn&&view.IsMine){
            cueStick.transform.position = new Vector3(cueBall.transform.position.x+direction.x,cueBall.transform.position.y+direction.y, cueBall.transform.position.z+direction.z);
        }
        if(!myTurn){
            cueStick.transform.position = new Vector3(100f,0f,0f);
        }


    }
    private void startDrag(Vector3 worldPosition){
        startPosition = worldPosition;
    }

    private void continueDrag(Vector3 worldPosition){
        endPosition = worldPosition;
        direction = endPosition - startPosition;
        float rotationZ = (Mathf.Atan2(startPosition.y-endPosition.y, startPosition.x-endPosition.x) * Mathf.Rad2Deg)-45;
        line.transform.position = new Vector3 (cueBall.transform.position.x,cueBall.transform.position.y,cueBall.transform.position.z);
        line.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
        cueStick.transform.position = new Vector3 (cueBall.transform.position.x,cueBall.transform.position.y,cueBall.transform.position.z);
        cueStick.transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ+45);
    }

    private void endDrag(){
        direction = endPosition - startPosition;
        line.transform.position = new Vector3(100f,0f,0f);
        if(direction.magnitude > 2){
            direction.Normalize();
            direction*=2;
        }
        if(direction.magnitude>0.5){
            cueStick.transform.position = new Vector3 (cueBall.transform.position.x,cueBall.transform.position.y,cueBall.transform.position.z);
            cueBall.GetComponent<Rigidbody2D>().AddForce(-direction*600);
            direction =  Vector3.zero;

            ballsStopped = false;
            StartCoroutine(clearBoard());
            turnManagerRef.player1Turn = !turnManagerRef.player1Turn;
            turnManagerRef.player2Turn = !turnManagerRef.player2Turn;
        }
        
    }
    IEnumerator clearBoard(){
        yield return new WaitForSeconds(2f);
        cueStick.transform.position = new Vector3(100f,0f,0f);

    }

}*/

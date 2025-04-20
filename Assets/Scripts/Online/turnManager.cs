using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class turnManager : MonoBehaviourPunCallbacks
{
    public GameObject player1, player2;


    public bool player1Turn;
    public bool player2Turn;
    public PhotonView view;
    private GameObject cueBall;

    public GameObject cueStick;
    
    [SerializeField]
    public GameObject turnCircle;
    private GameObject movingArrow;
    private GameObject mouseRad;
    [SerializeField]
    private AudioSource hitSound;
    // Start is called before the first frame update
    void Start()
    {   
        view = gameObject.GetComponent<PhotonView>();
        cueBall = GameObject.FindWithTag("cueBall");
        cueStick = GameObject.FindWithTag("cueStick1");
        movingArrow = GameObject.FindWithTag("movingArrow");
        mouseRad = GameObject.FindWithTag("mouseRadius");
    }

    // Update is called once per frame
    void Update()
    {
        if(!player1){
            player1 = GameObject.FindWithTag("PlayerOne");
        }
        if(!player2){
            player2 = GameObject.FindWithTag("PlayerTwo");
        }
        if(player1 && player2 && !PlayerOne.ballPlaced&&!player1Turn&&!player2Turn){
            player1Turn = true;
            player2Turn = false;
        }
        if(player1Turn){
            turnCircle.transform.position = new Vector3(-7f,4.25f,0f);
        }
        else if(player2Turn){
            turnCircle.transform.position = new Vector3(7f,4.25f,0f);
        }
        else{
            turnCircle.transform.position = new Vector3(100f,0f,0f);
        }

        // Need to implement autoquit in the event that someone DCs, this will stop issue with turns not syncing
        // when a new player replaces another player
    }

    [PunRPC]
    void SwitchTurns()
    {
        player1Turn = !player1Turn; 
        player2Turn = !player2Turn;
        mouse.needToPlace = false;
        mouse.placed = false;
    }
    [PunRPC]
    void HitBall(Vector3 Force){
        mouseRad.transform.position = new Vector3(100f,0f,0f); // moves the reference to the mouse position off the screen
        cueBall.GetComponent<Rigidbody2D>().AddForce(-Force*600); // adds a force to the cue ball. multiplies by 600 as conversion factor from my script's units to Unity's units
        PlayerOne.ballsStopped = false; // sets ballsStopped to false on both player scripts
        PlayerTwo.ballsStopped = false;
        hitSound.Play(); // plays the audio for hitting the cue ball
    }
    [PunRPC]
    void SyncSticks(Vector3 targetPos,float RotationZ){
        cueStick.transform.position = targetPos; // moves the cue stick to targetPos, either the position of the ball, or a position off screen
        cueStick.transform.rotation = Quaternion.Euler(0f,0f,RotationZ+45); // rotates the cue stick to an angle of rotationZ (+45 degrees as an offset)
    }
    [PunRPC]
    void moveCueBall(Vector3 targetPos){
        if(targetPos.x<-5.23f){
            targetPos = new Vector3(-5.23f,targetPos.y,0f);
        }
        if(targetPos.x>5.23f){
            targetPos = new Vector3(5.23f,targetPos.y,0f);
        }
        if(targetPos.y<-3.68f){
            targetPos = new Vector3(targetPos.x,-3.68f,0f);
        }
        if(targetPos.y>2.38f){
            targetPos = new Vector3(targetPos.x,2.38f,0f);
        }
        cueBall.transform.position = targetPos;
        
    }
    [PunRPC]
    void movingArrowMoveInitial(bool move){
        if(!move){
            movingArrow.transform.position = cueBall.transform.position;
        }
        else{
            movingArrow.transform.position = new Vector3(100f,0f,0f);
        }
    }
    [PunRPC]
     void moveMovingArrow(bool Scratch){
        if(Scratch){
            movingArrow.transform.position = cueBall.transform.position;
        }
        else{
            movingArrow.transform.position = new Vector3(100f,0f,0f);
        }
    }

}

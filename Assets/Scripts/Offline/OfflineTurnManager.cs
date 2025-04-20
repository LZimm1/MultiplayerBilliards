using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class OfflineTurnManager : MonoBehaviour
{
    public GameObject player1, player2;


    public bool player1Turn;
    public bool player2Turn;
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
        cueBall = GameObject.FindWithTag("cueBall");
        cueStick = GameObject.FindWithTag("cueStick1");
        movingArrow = GameObject.FindWithTag("movingArrow");
        mouseRad = GameObject.FindWithTag("mouseRadius");
        player1Turn = true;
        player2Turn = false;
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
    public void SwitchTurns()
    {
        player1Turn = !player1Turn;
        player2Turn = !player2Turn;
        OfflineMouse.needToPlace = false;
        OfflineMouse.placed = false;
    }
    public void HitBall(Vector3 Direction){
        mouseRad.transform.position = new Vector3(100f,0f,0f);
        cueBall.GetComponent<Rigidbody2D>().AddForce(-Direction*600);
        OfflinePlayerOne.ballsStopped = false;
        OfflinePlayerTwo.ballsStopped = false;
        hitSound.Play();
    }
    public void SyncSticks(Vector3 targetPos,float RotationZ){
        cueStick.transform.position = targetPos;
        cueStick.transform.rotation = Quaternion.Euler(0f,0f,RotationZ+45);
    }
    public void moveCueBall(Vector3 targetPos){
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
    public void movingArrowMoveInitial(bool move){
        if(!move){
            movingArrow.transform.position = cueBall.transform.position;
        }
        else{
            movingArrow.transform.position = new Vector3(100f,0f,0f);
        }
    }
    public void moveMovingArrow(bool Scratch){
        if(Scratch){
            movingArrow.transform.position = cueBall.transform.position;
        }
        else{
            movingArrow.transform.position = new Vector3(100f,0f,0f);
        }
    }

}

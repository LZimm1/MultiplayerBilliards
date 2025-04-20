using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Ball : MonoBehaviour
{
    private GameManager gameManagerRef;
    private PlayerOne playerOneRef;
    private PlayerTwo playerTwoRef;

    public static int solidsHit;
    public static int stripesHit;
    public static GameObject ballHit;
    public static bool eightInRightPocket;
    public GameObject cover;
    [SerializeField]
    private AudioSource pocketSound;
    // Start is called before the first frame update
    void Start()
    {
        solidsHit = 0;
        stripesHit = 0;
        ballHit = null;
        eightInRightPocket = false;
        gameManagerRef = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {   
        if(!playerOneRef&&GameObject.FindWithTag("PlayerOne")){
            playerOneRef = GameObject.FindWithTag("PlayerOne").GetComponent<PlayerOne>();
        }
        if(!playerTwoRef&&GameObject.FindWithTag("PlayerTwo")){
            playerTwoRef = GameObject.FindWithTag("PlayerTwo").GetComponent<PlayerTwo>();
        }

    }
    public void OnCollisionStay2D(Collision2D collision){
        if(gameObject.tag=="cueBall"&&ballHit==null){
            if(collision.gameObject.CompareTag("1")||collision.gameObject.CompareTag("2")||collision.gameObject.CompareTag("3")||collision.gameObject.CompareTag("4")||collision.gameObject.CompareTag("5")||collision.gameObject.CompareTag("6")||collision.gameObject.CompareTag("7")||collision.gameObject.CompareTag("8")){
                if(collision.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude!=0f){
                    ballHit = collision.gameObject;
                }
            }
            if(collision.gameObject.CompareTag("9")||collision.gameObject.CompareTag("10")||collision.gameObject.CompareTag("11")||collision.gameObject.CompareTag("12")||collision.gameObject.CompareTag("13")||collision.gameObject.CompareTag("14")||collision.gameObject.CompareTag("15")){
                if(collision.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude!=0f){
                    ballHit = collision.gameObject;
                }
            }
        }
    }
    
    public void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.CompareTag("hole")){
            if(GameManager.playerOneTIP&&!PlayerOne.solids&&!PlayerOne.stripes){
                if(gameObject.tag=="1"||gameObject.tag=="2"||gameObject.tag=="3"||gameObject.tag=="4"||gameObject.tag=="5"||gameObject.tag=="6"||gameObject.tag=="7"){
                    solidsHit++;
                    gameManagerRef.view.RPC("giveExtraTurn",RpcTarget.All,true);
                }
                else if(gameObject.tag=="9"||gameObject.tag=="10"||gameObject.tag=="11"||gameObject.tag=="12"||gameObject.tag=="13"||gameObject.tag=="14"||gameObject.tag=="15"){
                    stripesHit++;
                    gameManagerRef.view.RPC("giveExtraTurn",RpcTarget.All,true);
                }
            }
            else if(GameManager.playerTwoTIP&&!PlayerTwo.solids&&!PlayerTwo.stripes){
                if(gameObject.tag=="1"||gameObject.tag=="2"||gameObject.tag=="3"||gameObject.tag=="4"||gameObject.tag=="5"||gameObject.tag=="6"||gameObject.tag=="7"){
                    solidsHit++;
                    gameManagerRef.view.RPC("giveExtraTurn",RpcTarget.All,false);
                }
                else if(gameObject.tag=="9"||gameObject.tag=="10"||gameObject.tag=="11"||gameObject.tag=="12"||gameObject.tag=="13"||gameObject.tag=="14"||gameObject.tag=="15"){
                    stripesHit++;
                    gameManagerRef.view.RPC("giveExtraTurn",RpcTarget.All,false);
                }
            }

            if(GameManager.playerOneTIP&&PlayerOne.solids){
                if(gameObject.tag=="1"||gameObject.tag=="2"||gameObject.tag=="3"||gameObject.tag=="4"||gameObject.tag=="5"||gameObject.tag=="6"||gameObject.tag=="7"){
                    gameManagerRef.view.RPC("giveExtraTurn",RpcTarget.All,true);
                }
                if(gameObject.tag=="9"||gameObject.tag=="10"||gameObject.tag=="11"||gameObject.tag=="12"||gameObject.tag=="13"||gameObject.tag=="14"||gameObject.tag=="15"){
                    GameManager.awaitingScratch = true;
                }
            }
            else if(GameManager.playerOneTIP&&PlayerOne.stripes){
                if(gameObject.tag=="9"||gameObject.tag=="10"||gameObject.tag=="11"||gameObject.tag=="12"||gameObject.tag=="13"||gameObject.tag=="14"||gameObject.tag=="15"){
                    gameManagerRef.view.RPC("giveExtraTurn",RpcTarget.All,true);
                }
                if(gameObject.tag=="1"||gameObject.tag=="2"||gameObject.tag=="3"||gameObject.tag=="4"||gameObject.tag=="5"||gameObject.tag=="6"||gameObject.tag=="7"){
                    GameManager.awaitingScratch = true;
                }
            }
            else if(GameManager.playerTwoTIP&&PlayerTwo.solids){
                if(gameObject.tag=="1"||gameObject.tag=="2"||gameObject.tag=="3"||gameObject.tag=="4"||gameObject.tag=="5"||gameObject.tag=="6"||gameObject.tag=="7"){
                    gameManagerRef.view.RPC("giveExtraTurn",RpcTarget.All,false);
                }
                if(gameObject.tag=="9"||gameObject.tag=="10"||gameObject.tag=="11"||gameObject.tag=="12"||gameObject.tag=="13"||gameObject.tag=="14"||gameObject.tag=="15"){
                    GameManager.awaitingScratch = true;
                }
            }
            else if(GameManager.playerTwoTIP&&PlayerTwo.stripes){
                if(gameObject.tag=="9"||gameObject.tag=="10"||gameObject.tag=="11"||gameObject.tag=="12"||gameObject.tag=="13"||gameObject.tag=="14"||gameObject.tag=="15"){
                    gameManagerRef.view.RPC("giveExtraTurn",RpcTarget.All,false);
                }
                if(gameObject.tag=="1"||gameObject.tag=="2"||gameObject.tag=="3"||gameObject.tag=="4"||gameObject.tag=="5"||gameObject.tag=="6"||gameObject.tag=="7"){
                    GameManager.awaitingScratch = true;
                }
            }
            
            if(gameObject.tag!="cueBall"){
                gameObject.transform.position = new Vector3(-100f,0f,0f);
                gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                gameObject.GetComponent<Rigidbody2D>().totalForce = Vector3.zero;
                if(cover){
                    Destroy(cover);
                    pocketSound.Play();
                }
                
            }
            else{
                gameObject.transform.position = new Vector3(100f,0f,0f);
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,0f);
                GameManager.awaitingScratch = true;
            }
            if(gameObject.tag == "8"){
                gameObject.transform.position = new Vector3(-99f,0f,0f);
                gameManagerRef.view.RPC("eightBallHitIn",RpcTarget.All);
            }
        }
        if(gameObject.tag=="cueBall"&&ballHit==null){
            if(collision.gameObject.CompareTag("1")||collision.gameObject.CompareTag("2")||collision.gameObject.CompareTag("3")||collision.gameObject.CompareTag("4")||collision.gameObject.CompareTag("5")||collision.gameObject.CompareTag("6")||collision.gameObject.CompareTag("7")||collision.gameObject.CompareTag("8")){
                if(collision.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude!=0f){
                    ballHit = collision.gameObject;
                }
            }
            if(collision.gameObject.CompareTag("9")||collision.gameObject.CompareTag("10")||collision.gameObject.CompareTag("11")||collision.gameObject.CompareTag("12")||collision.gameObject.CompareTag("13")||collision.gameObject.CompareTag("14")||collision.gameObject.CompareTag("15")){
                if(collision.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude!=0f){
                    ballHit = collision.gameObject;
                }
            }
        }
        
    }
    public void OnTriggerStay2D(Collider2D collider) {
        if(gameObject.tag=="8"){
            if(mouse.pocket&&collider.tag.Equals(mouse.pocket.tag)){
                gameManagerRef.view.RPC("setEightInRightPocket",RpcTarget.All,true);
            }
        }
    }
    public void OnTriggerExit2D(Collider2D collider) {
        if(gameObject.tag=="8"){
            if(mouse.pocket&&collider.tag.Equals(mouse.pocket.tag)){
                gameManagerRef.view.RPC("setEightInRightPocket",RpcTarget.All,false);
            }
        }
    }
}

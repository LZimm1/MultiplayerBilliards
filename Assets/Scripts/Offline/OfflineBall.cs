using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class OfflineBall : MonoBehaviour
{
    private OfflineGameManager OfflineGameManagerRef;
    private OfflinePlayerOne playerOneRef;
    private OfflinePlayerTwo playerTwoRef;

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
        OfflineGameManagerRef = GameObject.FindWithTag("GameManager").GetComponent<OfflineGameManager>();
    }

    // Update is called once per frame
    void Update()
    {   
        if(!playerOneRef&&GameObject.FindWithTag("PlayerOne")){
            playerOneRef = GameObject.FindWithTag("PlayerOne").GetComponent<OfflinePlayerOne>();
        }
        if(!playerTwoRef&&GameObject.FindWithTag("PlayerTwo")){
            playerTwoRef = GameObject.FindWithTag("PlayerTwo").GetComponent<OfflinePlayerTwo>();
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
            if(OfflineGameManager.playerOneTIP&&!OfflinePlayerOne.solids&&!OfflinePlayerOne.stripes){
                if(gameObject.tag=="1"||gameObject.tag=="2"||gameObject.tag=="3"||gameObject.tag=="4"||gameObject.tag=="5"||gameObject.tag=="6"||gameObject.tag=="7"){
                    solidsHit++;
                    OfflineGameManagerRef.giveExtraTurn(true);
                }
                else if(gameObject.tag=="9"||gameObject.tag=="10"||gameObject.tag=="11"||gameObject.tag=="12"||gameObject.tag=="13"||gameObject.tag=="14"||gameObject.tag=="15"){
                    stripesHit++;
                    OfflineGameManagerRef.giveExtraTurn(true);
                }
            }
            else if(OfflineGameManager.playerTwoTIP&&!OfflinePlayerTwo.solids&&!OfflinePlayerTwo.stripes){
                if(gameObject.tag=="1"||gameObject.tag=="2"||gameObject.tag=="3"||gameObject.tag=="4"||gameObject.tag=="5"||gameObject.tag=="6"||gameObject.tag=="7"){
                    solidsHit++;
                    OfflineGameManagerRef.giveExtraTurn(false);
                }
                else if(gameObject.tag=="9"||gameObject.tag=="10"||gameObject.tag=="11"||gameObject.tag=="12"||gameObject.tag=="13"||gameObject.tag=="14"||gameObject.tag=="15"){
                    stripesHit++;
                    OfflineGameManagerRef.giveExtraTurn(false);
                }
            }

            if(OfflineGameManager.playerOneTIP&&OfflinePlayerOne.solids){
                if(gameObject.tag=="1"||gameObject.tag=="2"||gameObject.tag=="3"||gameObject.tag=="4"||gameObject.tag=="5"||gameObject.tag=="6"||gameObject.tag=="7"){
                    OfflineGameManagerRef.giveExtraTurn(true);
                }
                if(gameObject.tag=="9"||gameObject.tag=="10"||gameObject.tag=="11"||gameObject.tag=="12"||gameObject.tag=="13"||gameObject.tag=="14"||gameObject.tag=="15"){
                    OfflineGameManager.awaitingScratch = true;
                }
            }
            else if(OfflineGameManager.playerOneTIP&&OfflinePlayerOne.stripes){
                if(gameObject.tag=="9"||gameObject.tag=="10"||gameObject.tag=="11"||gameObject.tag=="12"||gameObject.tag=="13"||gameObject.tag=="14"||gameObject.tag=="15"){
                    OfflineGameManagerRef.giveExtraTurn(true);
                }
                if(gameObject.tag=="1"||gameObject.tag=="2"||gameObject.tag=="3"||gameObject.tag=="4"||gameObject.tag=="5"||gameObject.tag=="6"||gameObject.tag=="7"){
                    OfflineGameManager.awaitingScratch = true;
                }
            }
            else if(OfflineGameManager.playerTwoTIP&&OfflinePlayerTwo.solids){
                if(gameObject.tag=="1"||gameObject.tag=="2"||gameObject.tag=="3"||gameObject.tag=="4"||gameObject.tag=="5"||gameObject.tag=="6"||gameObject.tag=="7"){
                    OfflineGameManagerRef.giveExtraTurn(false);
                }
                if(gameObject.tag=="9"||gameObject.tag=="10"||gameObject.tag=="11"||gameObject.tag=="12"||gameObject.tag=="13"||gameObject.tag=="14"||gameObject.tag=="15"){
                    OfflineGameManager.awaitingScratch = true;
                }
            }
            else if(OfflineGameManager.playerTwoTIP&&OfflinePlayerTwo.stripes){
                if(gameObject.tag=="9"||gameObject.tag=="10"||gameObject.tag=="11"||gameObject.tag=="12"||gameObject.tag=="13"||gameObject.tag=="14"||gameObject.tag=="15"){
                    OfflineGameManagerRef.giveExtraTurn(false);
                }
                if(gameObject.tag=="1"||gameObject.tag=="2"||gameObject.tag=="3"||gameObject.tag=="4"||gameObject.tag=="5"||gameObject.tag=="6"||gameObject.tag=="7"){
                    OfflineGameManager.awaitingScratch = true;
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
                OfflineGameManager.awaitingScratch = true;
            }
            if(gameObject.tag == "8"){
                gameObject.transform.position = new Vector3(-99f,0f,0f);
                OfflineGameManagerRef.eightBallHitIn();
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
            if(OfflineMouse.pocket&&collider.tag.Equals(OfflineMouse.pocket.tag)){
                OfflineGameManagerRef.setEightInRightPocket(true);
            }
        }
    }
    public void OnTriggerExit2D(Collider2D collider) {
        if(gameObject.tag=="8"){
            if(OfflineMouse.pocket&&collider.tag.Equals(OfflineMouse.pocket.tag)){
                OfflineGameManagerRef.setEightInRightPocket(false);
            }
        }
    }
}

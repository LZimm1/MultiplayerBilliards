using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfflineMouse : MonoBehaviour
{
    public static bool needToPlace;
    public static bool placed;
    private OfflineGameManager gameManagerRef;
    private GameObject pocketRef;
    public static GameObject pocket;
    // Start is called before the first frame update
    void Start()
    {
        needToPlace = false;
        placed = false;
        pocket = null;
        gameManagerRef = GameObject.FindWithTag("GameManager").GetComponent<OfflineGameManager>();
        pocketRef = GameObject.FindWithTag("pocketPicker");
    }

    // Update is called once per frame
    void Update()
    {
        if(needToPlace){
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;
            gameObject.transform.position = pos;
        }
        else{
            if(placed&&Input.GetButtonDown("Cancel")){
                needToPlace = true;
                placed = false;
                gameManagerRef.chosePocket(new Vector3(100f,0f,0f));
                pocket=null;
            }
        }
        if(!placed&&!needToPlace){
            gameManagerRef.chosePocket(new Vector3(100f,0f,0f));
        }
    }
    void OnTriggerStay2D(Collider2D collider){
        if(collider.CompareTag("pocket1")){
            if(Input.GetMouseButtonDown(0)){
                needToPlace = false;
                placed = true;
                pocket = GameObject.FindWithTag("pocket1");
                gameObject.transform.position = new Vector3(-100f,0f,0f);
            }
            Vector3 position = new Vector3(-5.625f,2.8f,0f);
            gameManagerRef.chosePocket(position);
            
        }
        if(collider.CompareTag("pocket2")){
            if(Input.GetMouseButtonDown(0)){
                needToPlace = false;
                placed = true;
                pocket = GameObject.FindWithTag("pocket2");
                gameObject.transform.position = new Vector3(-100f,0f,0f);
            }
            Vector3 position = new Vector3(-0.04f,2.8f,0f);
            gameManagerRef.chosePocket(position);
            
        }
        if(collider.CompareTag("pocket3")){
            if(Input.GetMouseButtonDown(0)){
                needToPlace = false;
                placed = true;
                pocket = GameObject.FindWithTag("pocket3");
                gameObject.transform.position = new Vector3(-100f,0f,0f);
            }
            Vector3 position = new Vector3(5.625f,2.8f,0f);
            gameManagerRef.chosePocket(position);
            
        }
        if(collider.CompareTag("pocket4")){
            if(Input.GetMouseButtonDown(0)){
                needToPlace = false;
                placed = true;
                pocket = GameObject.FindWithTag("pocket4");
                gameObject.transform.position = new Vector3(-100f,0f,0f);
            }
            Vector3 position = new Vector3(-5.625f,-4.1f,0f);
            gameManagerRef.chosePocket(position);
            
        }
        if(collider.CompareTag("pocket5")){
            if(Input.GetMouseButtonDown(0)){
                needToPlace = false;
                placed = true;
                pocket = GameObject.FindWithTag("pocket5");
                gameObject.transform.position = new Vector3(-100f,0f,0f);
            }
            Vector3 position = new Vector3(-0.04f,-4.1f,0f);
            gameManagerRef.chosePocket(position);
            
        }
        if(collider.CompareTag("pocket6")){
            if(Input.GetMouseButtonDown(0)){
                needToPlace = false;
                placed = true;
                pocket = GameObject.FindWithTag("pocket6");
                gameObject.transform.position = new Vector3(-100f,0f,0f);
            }
            Vector3 position = new Vector3(5.625f,-4.1f,0f);
            gameManagerRef.chosePocket(position);
            
        }
    }
    void OnTriggerExit2D(Collider2D collider){
        if((collider.CompareTag("pocket1")||collider.CompareTag("pocket2")||collider.CompareTag("pocket3")||collider.CompareTag("pocket4")||collider.CompareTag("pocket5")||collider.CompareTag("pocket6"))&&gameObject.transform.position.x!=-100f){
            Vector3 position = new Vector3(100f,0f,0f);
            gameManagerRef.chosePocket(position);
        }
    }
    void chosePocket(Vector3 position){
        if(gameManagerRef.pocketPicker){
            gameManagerRef.pocketPicker.transform.position = position;
        }
    }

}

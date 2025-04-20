using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseRadius : MonoBehaviour
{
    public bool canPlace;
    private GameObject cueBall;
    // Start is called before the first frame update
    void Start()
    {
        cueBall = GameObject.FindWithTag("cueBall");
    }
    void FixedUpdate(){
        canPlace = true;
    }
    // Update is called once per frame
    void Update()
    {
        
        
    }
    public void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.CompareTag("1")||collision.gameObject.CompareTag("2")||collision.gameObject.CompareTag("3")||collision.gameObject.CompareTag("4")||collision.gameObject.CompareTag("5")||collision.gameObject.CompareTag("6")||collision.gameObject.CompareTag("7")||collision.gameObject.CompareTag("8")){
            canPlace = false;
        }
        else if(collision.gameObject.CompareTag("9")||collision.gameObject.CompareTag("10")||collision.gameObject.CompareTag("11")||collision.gameObject.CompareTag("12")||collision.gameObject.CompareTag("13")||collision.gameObject.CompareTag("14")||collision.gameObject.CompareTag("15")){
            canPlace = false;
        }
    }
    public void OnCollisionStay2D(Collision2D collision){
        if(collision.gameObject.CompareTag("1")||collision.gameObject.CompareTag("2")||collision.gameObject.CompareTag("3")||collision.gameObject.CompareTag("4")||collision.gameObject.CompareTag("5")||collision.gameObject.CompareTag("6")||collision.gameObject.CompareTag("7")||collision.gameObject.CompareTag("8")){
            canPlace = false;
        }
        else if(collision.gameObject.CompareTag("9")||collision.gameObject.CompareTag("10")||collision.gameObject.CompareTag("11")||collision.gameObject.CompareTag("12")||collision.gameObject.CompareTag("13")||collision.gameObject.CompareTag("14")||collision.gameObject.CompareTag("15")){
            canPlace = false;
        }
    }
}

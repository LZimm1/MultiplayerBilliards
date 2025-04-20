using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class playerSpawner : MonoBehaviour
{
    public GameObject playerOnePrefab;
    public GameObject playerTwoPrefab;
    public GameObject playerOneRef;
    
    public GameObject playerTwoRef;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawnPlayers());
        
        
        
    }

    // Update is called once per frame
    void Update()
    {
        playerOneRef = GameObject.FindWithTag("PlayerOne");
        playerTwoRef = GameObject.FindWithTag("PlayerTwo");
    }
    IEnumerator spawnPlayers(){
        playerOneRef = GameObject.FindWithTag("PlayerOne");
        playerTwoRef = GameObject.FindWithTag("PlayerTwo");
        yield return new WaitForSeconds(.2f);
        if(playerOneRef==null){
            playerOneRef = PhotonNetwork.Instantiate(playerOnePrefab.name,Vector3.zero,Quaternion.identity);
        }
        else if(playerTwoRef==null){
            playerTwoRef = PhotonNetwork.Instantiate(playerTwoPrefab.name,Vector3.zero,Quaternion.identity);
        }
    }

}

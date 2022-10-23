using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EagleSpawner : MonoBehaviour
{
    [SerializeField] GameObject eaglePrefab;
    [SerializeField] int spawnZPos;
    [SerializeField] Player player;
    [SerializeField] float timeOut = 5;
    float timer=0;
    int playerLastMaxTravel=0;

    private void Start()
    {
        
        // if(player.IsJumping()==false)
        //     SpawnEagle();
        
    }

    private void SpawnEagle()
    {
        player.enabled=false;
        var position = new Vector3(player.transform.position.x ,1 ,player.CurrentTravel + spawnZPos);
        var rotation = Quaternion.Euler(0,180,0);
        var eagleObject = Instantiate(eaglePrefab,position,rotation);
        var eagle = eagleObject.GetComponent<Eagle>();
        eagle.SetUpTarget(player);

    }

    private void Update()
    {   //Debug.Log(timer);
        //jika player ada kemajuan (gerak?)
        // Debug.Log(player.IsMoving);
        Debug.Log(timer);
        if(player.MaxTravel != playerLastMaxTravel || player.IsMoving==true)
        {
            //reset timer
            timer = 0;
            playerLastMaxTravel=player.MaxTravel;
            return;
        }

        //kalau tidak maju, timer start
        if(timer < timeOut )
        {
            timer += Time.deltaTime;
            return;
        }
        if(timer>=timeOut)
        {
            Debug.Log("STOP");
            player.StopAllCoroutines();
        }
        //jika timeout
        Debug.Log(""+player.IsJumping()+" "+  player.IsDie);
        if(player.IsMoving==false && player.IsDie==false )
        {
            
            SpawnEagle();
        }
    }

}

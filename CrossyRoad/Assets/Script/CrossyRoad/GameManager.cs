using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject buttonHome;
    [SerializeField] GameObject grass;
    [SerializeField] GameObject road;
    [SerializeField] int extent;
    [SerializeField] int frontDistance=10;
    // [SerializeField] int backDistance =10;
    [SerializeField] int backDistance=-5;
    [SerializeField] int maxSameTerrainRepeat =3;

    Dictionary<int,TerrainBlock> map= new Dictionary<int, TerrainBlock>(50);

    TMP_Text gameOverText;
    private void Start()
    {
        //setup gameover panel
        gameOverPanel.SetActive(false);
        gameOverText = gameOverPanel.GetComponentInChildren<TMP_Text>();

        //terrain blkg
        for(int z = backDistance; z <= 0; z++)
        {
            CreateTerrain(grass,z);
        }

        //terrain depan
        for(int z=1;z <= frontDistance;z++)
        {
            var prefab = GetNextRandomTerrainPrefab(z);

            //instantiate block
            CreateTerrain(prefab,z);
        }

        player.SetUp(backDistance,extent);
        // foreach(var treePos in Tree.AllPositions)
        // {
        //     Debug.Log(treePos);
        // }

    }
    private int playerLastMaxTravel;
    private void Update()
    {
        //cek apakah player masih hidup
        if(player.IsDie && gameOverPanel.activeInHierarchy==false)
            StartCoroutine(ShowGameOverPanel());

        //infinite Terrain system
        if(player.MaxTravel==playerLastMaxTravel)
            return;
        
        playerLastMaxTravel=player.MaxTravel;

        //instantiate terrain depan
        var randTBPrefab = 
            GetNextRandomTerrainPrefab(player.MaxTravel+frontDistance);
        CreateTerrain(randTBPrefab,player.MaxTravel+frontDistance);

        //instantiate terrain belakang
        //cara-1
        var lastTB = map[player.MaxTravel-1 + backDistance];

        //cara-2
        // TerrainBlock lastTB = map[player.MaxTravel+frontDistance];
        // int lastPos = player.MaxTravel;
        // foreach(var(pos,tb)in map)
        // {
        //     if(pos<lastPos)
        //     {
        //         lastPos=pos;
        //         lastTB = tb;
        //     }
        // }

        //remove dari dict
        map.Remove(player.MaxTravel-1 + backDistance);

        //destroy (remove) dari scene
        Destroy(lastTB.gameObject);

        //setup-> supaya player tdk bisa ke belakang 
        player.SetUp(player.MaxTravel+1 + backDistance,extent);
    }

    IEnumerator ShowGameOverPanel()
    {
        yield return new WaitForSeconds(3);
        // player.enabled = false;
        Debug.Log("GameOver");
        

        gameOverText.text = "Your Score : "+player.MaxTravel;
        gameOverPanel.SetActive(true);
        buttonHome.SetActive(false);
    }

    private void CreateTerrain(GameObject prefab, int zPos)
    {
        var go = Instantiate(prefab,new Vector3(0,0,zPos),Quaternion.identity);
        var tb = go.GetComponent<TerrainBlock>();
        tb.Build(extent);

        map.Add(zPos,tb);
        // Debug.Log(map[zPos]is Road);
    }

    private GameObject GetNextRandomTerrainPrefab(int nextPos)
    {
        bool isUniform = true;
        var tbRef = map[nextPos-1];
        for(int distance =2; distance <= maxSameTerrainRepeat;distance++)
        {
            if(map[nextPos-distance].GetType()!=tbRef.GetType())
            {
                isUniform=false;
                break;
            }
        }
        if(isUniform)
        {
            if(tbRef is Grass)
                return road;
            else
                return grass;
        }

        //penentuan terrain block dengan prob 50%
        return Random.value >0.5f ? road : grass;
    }

    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using TMPro;
public class Player : MonoBehaviour
{
    [SerializeField]TMP_Text stepText;
    [SerializeField] ParticleSystem dieparticle;
    [SerializeField,Range(0.01f,1f)]float moveDuration=0.2f;
    [SerializeField,Range(0.01f,1f)]float jumpHeight=0.5f;
    private float rightBoundary;
    private float leftBoundary;
    private float backBoundary;
    [SerializeField]private int maxTravel;
    public int MaxTravel{get=> maxTravel;}
    [SerializeField]private int currentTravel;
    public int CurrentTravel { get => currentTravel; }

    public bool IsDie {get => this.enabled==false;}
    public AudioSource audioSource;
    public AudioClip sfxJump;
    private bool isMoving;
    public bool IsMoving{get=> isMoving;}


    public void SetUp(int minZPos, int extent)
    {
        backBoundary = minZPos -1;
        leftBoundary = -(extent+1);
        rightBoundary = extent +1;
    }
    // Start is called before the first frame update
    void Start()
    {
        isMoving=true;
    }

    // Update is called once per frame
    void Update()
    {
        //check moving via coroutine
        StartCoroutine(CheckMoving());
        // Debug.Log(IsMoving);

        var moveDir = Vector3.zero;
        if(Input.GetKey(KeyCode.UpArrow))
            moveDir += new Vector3(0,0,1);
        else if(Input.GetKey(KeyCode.DownArrow))
            moveDir += new Vector3(0,0,-1);
        else if(Input.GetKey(KeyCode.RightArrow))
            moveDir += new Vector3(1,0,0);
        else if(Input.GetKey(KeyCode.LeftArrow))
            moveDir += new Vector3(-1,0,0);
        

        if(moveDir == Vector3.zero)
            return;

        if(IsJumping()==false)
            Jump(moveDir);

        
        
    }
    private IEnumerator CheckMoving()
    {
        Vector3 startPos = transform.position;
        yield return null;
        Vector3 finalPos = transform.position;
        if(startPos.x != finalPos.x ||startPos.z!=finalPos.z)
            isMoving=true;
        else
            isMoving=false;
        
    }
    private void Jump(Vector3 targetDirection)
    {
        //atur rotasi
        var targetPosition = 
            transform.position + targetDirection;
        
        //lihat arah jalan/loncat
        transform.LookAt(targetPosition);
        
        //loncat
        // transform.DOMoveY(0.5f, 0.1f)
        //     .OnComplete(() => transform.DOMoveY(0.2,0.1f));
        // loncat
        var moveSeq = DOTween.Sequence(transform);
        moveSeq.Append(transform.DOMoveY(jumpHeight,moveDuration/2));
        moveSeq.Append(transform.DOMoveY(0,moveDuration/2));

        if(targetPosition.z <= backBoundary||
            targetPosition.x <= leftBoundary||
            targetPosition.x >= rightBoundary)
            {
                // IsMoving=false;
                return;
            }
        
        if(Tree.AllPositions.Contains(targetPosition))
            
            {
                // IsMoving=false;
                return;
            }
        
        //gerak maju/mundur
        transform.DOMoveX(targetPosition.x,moveDuration);
        transform
            .DOMoveZ(targetPosition.z,moveDuration)
            .OnComplete(UpdateTravel);

        //audio Jump
        audioSource.PlayOneShot(sfxJump);

        
    }

    public bool IsJumping()
    {
        return DOTween.IsTweening(transform);
    }

    private void UpdateTravel()
    {

        currentTravel = (int) this.transform.position.z;
        if(currentTravel > maxTravel)
            maxTravel = currentTravel;
        
        stepText.text ="STEP : " + maxTravel.ToString();
        
        
        //alternatif
        // maxTravel = currentTravel > maxTravel ? currentTravel :maxTravel;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(this.enabled==false)
            return;
        //execute sekali tiap frame ketika pertama kali collide
        var car = other.GetComponent<Car>();
        if(car != null)
        {
            // Debug.Log("Hit " + car.name);
            // AnimateDie(car);
        }
        if(other.tag =="Car")
        {
            // Debug.Log(other.name);
            AnimateCarHit();
        }
    }

    private void AnimateCarHit()
    {
        // var isRight = car.transform.rotation.y == 90;

        // transform.DOMoveX(isRight ? 8 : -8,0.2f);
        // transform
        //     .DORotate(Vector3.forward*360,0.2f)
        //     .SetLoops(10,LoopType.Restart);

        //gepeng
        transform.DOScaleY(0.1f,1);
        transform.DOScaleX(2,1);
        transform.DOScaleZ(2,1);
        this.enabled=false;
        dieparticle.Play();
    }

    private void OnTriggerStay(Collider other)
    {
        //execute tiap frame selama masih colliding
    }

    private void OnTriggerExit(Collider other)
    {
        //execute tiap frame ketika stop collide
    }

}

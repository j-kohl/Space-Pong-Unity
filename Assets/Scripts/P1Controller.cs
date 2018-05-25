using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class P1Controller : MonoBehaviour
{
   public float speed;
   private Rigidbody rb;
   public float startZ;
   private Rect rect;
   private int trackedTouchId;
   private bool moving;
   public GameObject laser;
   public GameObject missile;
   public Transform laserSpawn;
   public Transform missileSpawn;
   public float nextFire;
   public float fireRate;
   public RectTransform missileButtonRect;
   public RectTransform laserButtonRect;  
   private bool hitByLaser = false;   
   private bool hitByMissile = false;
   private int tapCount;
   private float doubleTapTimer;
   private bool hasItem;
   private int missileAmmo;
   private int laserAmmo;
   private float shieldDuration;
   private bool shieldActive;
   private enum HitType {LASER, MISSILE};

   void Start()
   {
      rect = new Rect(0, 0, Screen.width, Screen.height / 2);
      rb = gameObject.GetComponent<Rigidbody>();
   }

   void Update()
   {
      if(shieldDuration <= 0){
         shieldDuration = 0;
         shieldActive = false;
         hasItem = false;
      }   
      else
         shieldDuration -= Time.deltaTime;

      if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
      {
         tapCount++;
      }
      if (tapCount > 0)
      {
         doubleTapTimer += Time.deltaTime;
      }
      if (tapCount >= 2)
      {
         doubleTapTimer = 0.0f;
         tapCount = 0;
      }
      if (doubleTapTimer > 0.5f)
      {
         Debug.Log("Double Tap");
         doubleTapTimer = 0f;
         tapCount = 0;
      }
   }

   void FixedUpdate()
   {
      if (GameController.Instance.GameState == GameController.GameStateEnum.Running)
      {
         if(hitByLaser || hitByMissile)
            speed = 4;
         else
            speed = 8;

         if (Input.touchCount > 0)
         {
            foreach (Touch touch in Input.touches)
            {
               if (touch.phase == TouchPhase.Began && !moving)
               {
                  tapCount++;
                  trackedTouchId = touch.fingerId;
                  moving = true;                  
               }
               if ((touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved))
               {
                  Vector3 touchedPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 12.6f));
                  Vector3 diff = (touchedPos - transform.position);
                  if (diff.magnitude > 0.25)
                  {
                     if (rect.Contains(touch.position) && touch.fingerId == trackedTouchId && moving)
                     {
                        trackedTouchId = touch.fingerId;
                        Vector3 direction = (touchedPos - transform.position).normalized;
                        rb.MovePosition(transform.position + direction * speed * Time.deltaTime);
                     }
                  }
               }

               if (touch.phase == TouchPhase.Ended && touch.fingerId == trackedTouchId)
               {
                  moving = false;
               }
            }
         
         }
      }
      else if (GameController.Instance.GameState == GameController.GameStateEnum.P1Scored ||
      GameController.Instance.GameState == GameController.GameStateEnum.P2Scored ||
      GameController.Instance.GameState == GameController.GameStateEnum.GameStart ||
      GameController.Instance.GameState == GameController.GameStateEnum.P1Won ||
      GameController.Instance.GameState == GameController.GameStateEnum.P2Won)
      {
         rb.velocity = Vector3.zero;
         rb.position = new Vector3(0.0f, 0.0f, startZ);
      }
   }

   public void shootMissile()
   {
      if (Time.time > nextFire)
      {
         nextFire = Time.time + fireRate;
         GameObject missileClone = (GameObject)Instantiate(missile, missileSpawn.position, missileSpawn.rotation);
         missileClone.GetComponent<HomingMissile>().SetParent(gameObject);
      }
   }

   public void shootLaser()
   {
      if (Time.time > nextFire)
      {
         nextFire = Time.time + fireRate;
         GameObject laserClone = (GameObject)Instantiate(laser, laserSpawn.position, laserSpawn.rotation);
         laserClone.GetComponent<LaserController>().SetParent(gameObject);
      }
   }   

   private IEnumerator SlowDownShip(float timeAmount, HitType hitType)
   {
      if(hitType == HitType.LASER){
         hitByLaser = true;
         yield return new WaitForSeconds(timeAmount); 
         hitByLaser = false;
      }else if(hitType == HitType.MISSILE){
         hitByMissile = true;
         yield return new WaitForSeconds(timeAmount); 
         hitByMissile = false;
      }
   }

   internal void LaserHit()
   {
      if (!shieldActive)
         StartCoroutine(SlowDownShip(3f, HitType.LASER));
   }
   
   internal void MissileHit()
   {
      if (!shieldActive)
         StartCoroutine(SlowDownShip(5f, HitType.MISSILE));
   }

   public void collectedShield()
   {
      if (!hasItem)
      {
         shieldDuration = 10;
         hasItem = true;
         shieldActive = true;
      }
   }

   public void collectedLaser()
   {
      if (!hasItem)
      {
         laserAmmo = 10;
         hasItem = true;
      }
   }
   public void collectedMissile()
   {
      if (!hasItem)
      {
         missileAmmo = 5;
         hasItem = true;
      }
   }
}

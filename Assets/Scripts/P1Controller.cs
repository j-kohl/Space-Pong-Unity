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
   private float doubleTapTimer;
   private bool hasShield;
   private bool hasLaser;
   private bool hasMissile;
   private int missileAmmo;
   private int laserAmmo;
   private float shieldDuration;
   private bool shieldActive;
   private enum HitType { LASER, MISSILE };
   public enum ItemType { NONE, SHIELD, LASER, MISSILE };
   private float lastTouchStartTime;
   private ItemType currentItem;
   public Text itemText;

   void Start()
   {
      rect = new Rect(0, 0, Screen.width, Screen.height / 2);
      rb = gameObject.GetComponent<Rigidbody>();
      currentItem = ItemType.NONE;
   }

   void Update()
   {
      if (shieldActive)
      {
         if (shieldDuration <= 0)
         {
            shieldDuration = 0;
            shieldActive = false;
            currentItem = ItemType.NONE;
            GetComponent<MeshRenderer>().material.color = Color.red;

         }
         else
            shieldDuration -= Time.deltaTime;
      }
   }

   void FixedUpdate()
   {
      if (GameController.Instance.GameState == GameController.GameStateEnum.Running)
      {
         if (hitByLaser || hitByMissile)
            speed = 4;
         else
            speed = 8;

         if (Input.touchCount > 0)
         {
            foreach (Touch touch in Input.touches)
            {
               if (touch.phase == TouchPhase.Began)
               {
                  if (touch.tapCount == 2)
                  {
                     useItem();
                  }
                  if (!moving)
                  {
                     trackedTouchId = touch.fingerId;
                     moving = true;
                  }
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
      if (missileAmmo > 0)
      {
         if (Time.time > nextFire)
         {
            nextFire = Time.time + fireRate;
            GameObject missileClone = (GameObject)Instantiate(missile, missileSpawn.position, missileSpawn.rotation);
            missileClone.GetComponent<HomingMissile>().SetParent(gameObject);
            missileAmmo--;
         }
      }
      if (missileAmmo == 0)
      {
         currentItem = ItemType.NONE;
         itemText.text = "None";
      }
   }

   public void shootLaser()
   {
      if (laserAmmo > 0)
      {
         if (Time.time > nextFire)
         {
            nextFire = Time.time + fireRate;
            GameObject laserClone = (GameObject)Instantiate(laser, laserSpawn.position, laserSpawn.rotation);
            laserClone.GetComponent<LaserController>().SetParent(gameObject);
            laserAmmo--;
         }
      }
      if (laserAmmo == 0)
      {
         currentItem = ItemType.NONE;
         itemText.text = "None";
      }
   }

   private IEnumerator SlowDownShip(float timeAmount, HitType hitType)
   {
      if (hitType == HitType.LASER)
      {
         hitByLaser = true;
         yield return new WaitForSeconds(timeAmount);
         hitByLaser = false;
      }
      else if (hitType == HitType.MISSILE)
      {
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

   public void collectItem(ItemType itemType)
   {
      if (!hasItem())
      {
         switch (itemType)
         {
            case ItemType.SHIELD:
               collectedShield();
               break;
            case ItemType.MISSILE:
               collectedMissile();
               break;
            case ItemType.LASER:
               collectedLaser();
               break;
         }
      }
   }

   private void collectedShield()
   {
      currentItem = ItemType.SHIELD;
      itemText.text = "Shield";
   }

   private void collectedLaser()
   {
      laserAmmo = 10;
      currentItem = ItemType.LASER;

   }
   private void collectedMissile()
   {
      missileAmmo = 5;
      currentItem = ItemType.MISSILE;
      itemText.text = "Missile";

   }

   private void useItem()
   {
         Debug.Log("current item" + currentItem);
      switch (currentItem)
      {
         case ItemType.SHIELD:
            activateShield();
            break;
         case ItemType.MISSILE:
            shootMissile();
            break;
         case ItemType.LASER:
            shootLaser();
            break;
      }
   }

   private void activateShield()
   {
      GetComponent<MeshRenderer>().material.color = Color.blue;

      shieldDuration = 10;
      shieldActive = true;
      itemText.text = "None";
   }

   private bool hasItem()
   {
      return currentItem != ItemType.NONE;
   }
}

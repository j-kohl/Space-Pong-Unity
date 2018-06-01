using UnityEngine;
using System.Collections;
using System;

public class P2Controller : MonoBehaviour
{

   public float speed;
   private Rigidbody rb;
   public float startZ;
   private Rect rect;
   private int trackedTouchId;
   private bool moving;

   private bool hitByLaser = false;
   private bool hitByMissile = false;
   private enum HitType { LASER, MISSILE };

   internal void LaserHit()
   {
      StartCoroutine(SlowDownShip(3f, HitType.LASER));
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
   void Start()
   {
      rect = new Rect(0, Screen.height / 2, Screen.width, Screen.height);
      rb = gameObject.GetComponent<Rigidbody>();
      // rb.velocity = new Vector3(-1,0,0) * speed;
      // rb.angularVelocity = Vector3.forward * speed;
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
               if (rect.Contains(touch.position) && touch.phase == TouchPhase.Began && !moving)
               {
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
   }

   internal void collectItem(P1Controller.ItemType itemType)
   {
   }

   internal void collectedMissile()
   {
   }

   internal void MissileHit()
   {
      StartCoroutine(SlowDownShip(5f, HitType.MISSILE));
   }
}

using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour
{

   private int lastPlayerHit;
   private Rigidbody rb;
   // Use this for initialization
   void Awake()
   {
      rb = (Rigidbody)gameObject.GetComponent<Rigidbody>();
      lastPlayerHit = 0;
   }

   void Start()
   {
   }

   // Update is called once per frame
   void FixedUpdate()
   {
      if (GameController.Instance.GameState == GameController.GameStateEnum.Running)
      {
         if (rb.velocity.magnitude > 5)
         {
            rb.velocity = rb.velocity.normalized * 5;
         }
      }
      else if (GameController.Instance.GameState == GameController.GameStateEnum.P1Scored ||
      GameController.Instance.GameState == GameController.GameStateEnum.P2Scored ||
      GameController.Instance.GameState == GameController.GameStateEnum.GameStart ||
      GameController.Instance.GameState == GameController.GameStateEnum.P1Won ||
      GameController.Instance.GameState == GameController.GameStateEnum.P2Won)
      {
         rb.velocity = Vector3.zero;
         rb.position = Vector3.zero;
      }
   }

   /// <summary>
   /// OnCollisionEnter is called when this collider/rigidbody has begun
   /// touching another rigidbody/collider.
   /// </summary>
   /// <param name="other">The Collision data associated with this collision.</param>
   void OnCollisionEnter(Collision other)
   {
      switch (other.gameObject.tag)
      {
         case "Player 1":
            lastPlayerHit = 1;
            break;
         case "Player 2":
            lastPlayerHit = 2;
            break;
      }
      /*case "Top Border":
      GameController.Instance.GameState = GameController.GameStateEnum.P1Scored;
      if(GameController.Instance.scores.p1Score == 10)
         GameController.Instance.GameState = GameController.GameStateEnum.P1Won;
      break;
   case "Bottom Border":
      GameController.Instance.GameState = GameController.GameStateEnum.P2Scored;
      if(GameController.Instance.scores.p2Score == 10)
         GameController.Instance.GameState = GameController.GameStateEnum.P2Won;
      break;*/
   }

   /// <summary>
   /// OnTriggerEnter is called when the Collider other enters the trigger.
   /// </summary>
   /// <param name="other">The other Collider involved in this collision.</param>
   void OnTriggerEnter(Collider other)
   {
      switch (other.gameObject.tag)
      {
         case "ShieldCollectible":
            GameController.Instance.CollectibleHit(lastPlayerHit, P1Controller.ItemType.SHIELD);
            break;
         case "LaserCollectible":
            GameController.Instance.CollectibleHit(lastPlayerHit, P1Controller.ItemType.LASER);
            break;
         case "MissileCollectible":
            GameController.Instance.CollectibleHit(lastPlayerHit, P1Controller.ItemType.MISSILE);
            break;
         default:
            break;
      }
   }

   public void SetInitialDirection(int direction)
   {
      rb.velocity = new Vector3(0, 0, direction) * 2f;
   }
}

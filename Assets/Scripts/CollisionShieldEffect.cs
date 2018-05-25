using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionShieldEffect : MonoBehaviour {

   float EffectTime = 0;

   void Update()
   {
      if (EffectTime > 0)
      {
         if (EffectTime < 450 && EffectTime > 400)
         {
            GetComponent<Renderer>().sharedMaterial.SetVector("_ShieldColor", new Vector4(0.7f, 1, 1, 0));
         }

         EffectTime -= Time.deltaTime * 1000;

         GetComponent<Renderer>().sharedMaterial.SetFloat("_EffectTime", EffectTime);
      }
   }

   void OnCollisionEnter(Collision collision)
   {
      foreach (ContactPoint contact in collision.contacts)
      {

         GetComponent<Renderer>().sharedMaterial.SetVector("_ShieldColor", new Vector4(0.7f, 1, 1, 0.05f));

         GetComponent<Renderer>().sharedMaterial.SetVector("_Position", transform.InverseTransformPoint(contact.point));

         EffectTime = 500;
      }
   }
}

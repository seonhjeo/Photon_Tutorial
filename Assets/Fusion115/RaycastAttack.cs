using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.Serialization;

namespace Fusion115
{
    public class RaycastAttack : NetworkBehaviour
    {
        public float damage;

        public PlayerMovement playerMovement;
        

        private void Update()
        {
            if (HasStateAuthority == false)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Debug.Log("Shoot");
                Ray ray = playerMovement.cam.ScreenPointToRay(Input.mousePosition);
                ray.origin += playerMovement.cam.transform.forward;
                
                Debug.DrawRay(ray.origin, ray.direction, Color.red, 1f);
                if (Runner.GetPhysicsScene().Raycast(ray.origin,ray.direction, out var hit))
                {
                    if (hit.transform.TryGetComponent<Health>(out var health))
                    {
                        health.DealDamageRpc(damage);
                    }
                }
            }
        }
    }
}


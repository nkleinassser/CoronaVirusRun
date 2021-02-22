using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleHitScript : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collider) {
        if (collider.tag == "Wall") {
            GameScreenController.particleHitWall(this.gameObject, collider);
        } 
    }
}

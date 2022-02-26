using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCollector : MonoBehaviour
{
    private int cherries = 0;
    [SerializeField] private Text cherriesText;

    [SerializeField] private AudioSource collectSound;
    
    // Cherries are triggers, not regular colliders as we don't want a 'collision'
    // We just want them to trigger a behavior (collection)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Cherry")) return;
        Destroy(collision.gameObject);
        cherries++;
        cherriesText.text = "Cherries: " + cherries;
        collectSound.Play();
    }
}

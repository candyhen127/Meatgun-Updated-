using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapToPlayer : MonoBehaviour
{
    public GameObject player;
    public RectTransform rb;
    public Transform t;
    
    public Vector3 screenPos;
    void Start()
    {
        player = GameObject.Find("Player");
    }
    void FixedUpdate()
    {
        t=player.GetComponent<Transform>();
        screenPos = Camera.main.WorldToScreenPoint(t.position);
        rb.position = screenPos;
    }
}

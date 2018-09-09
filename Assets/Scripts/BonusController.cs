using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusController : MonoBehaviour {
    [SerializeField] float movFactor = 2f;
    [SerializeField] Vector3 Movement = new Vector3(1, 1, 2);
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        transform.Rotate(Movement * movFactor);
    }

    

    //void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.tag == "Player")
    //    {
    //        Debug.Log("Hello");

    //    }
    // }







}

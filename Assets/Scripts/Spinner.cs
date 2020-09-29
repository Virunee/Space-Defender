using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    [SerializeField] float speedOfSpin = 1f;
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, (speedOfSpin + Random.Range(1f, 90f)) * Time.deltaTime);
    }
}

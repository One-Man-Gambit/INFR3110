using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBehaviour : MonoBehaviour
{
    public float RotationSpeed = 5.0f;

    private void Update() 
    {
        transform.Rotate(new Vector3(0, 0, RotationSpeed * Time.deltaTime));
    }
}

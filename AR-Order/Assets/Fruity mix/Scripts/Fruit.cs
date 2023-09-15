using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour {

    public GameObject fruitPart1;
    public GameObject fruitPart2;

    private void OnTriggerEnter(Collider col)
    {
        if(col.GetComponent<Blade>())
        {
            Vector3 direction = (col.transform.position - transform.position).normalized;

            Quaternion rotation = Quaternion.LookRotation(direction);

            Instantiate(fruitPart1, transform.position, rotation).GetComponent<Rigidbody>().AddExplosionForce(200f, transform.position, 1.0f, 0.1f);
            Instantiate(fruitPart2, transform.position, rotation).GetComponent<Rigidbody>().AddExplosionForce(200f, transform.position, 1.0f, 0.1f);



            Destroy(gameObject);
        }
    }
}

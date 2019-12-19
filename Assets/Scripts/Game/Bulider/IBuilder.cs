using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuilder<T> {
    T GetProductClass(GameObject gameObject);

    GameObject GetProduct();

    void GetData(T productClass);

    void GetOtherResource(T productClass); 
}

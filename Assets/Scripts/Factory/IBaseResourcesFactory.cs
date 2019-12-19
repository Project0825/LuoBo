using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 其他资源种类工厂
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IBaseResourcesFactory<T> {
    T GetSingleResources(string resourcePath);
}

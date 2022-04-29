using UniRx.Toolkit;
using UnityEngine;

public class CarrotPool : ObjectPool<Carrot>
{
    private readonly Carrot prefab;
    private readonly Transform parenTransform;
    
    public CarrotPool(Transform parenTransform, Carrot prefab)
    {
        this.parenTransform = parenTransform;
        this.prefab = prefab;
    }
    
    protected override Carrot CreateInstance()
    {
        return Object.Instantiate(prefab, parenTransform, true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order : MonoBehaviour
{
    [SerializeField]
    int originOrder;
    int preOrder;

    public void SetOriginOrder(int originOrder)
    {
        this.originOrder = originOrder;
        preOrder = (originOrder==100 ? preOrder : originOrder);
        GetComponent<Renderer>().sortingOrder = originOrder;
    }
    public void SetMostFrontOrder(bool front)
    {
        SetOriginOrder(front ? 100 : preOrder);
        
    }
}

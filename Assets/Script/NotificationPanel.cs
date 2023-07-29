using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class NotificationPanel : MonoBehaviour
{
    [SerializeField] TMP_Text notifivationTMP;
    
    public void Show(string message)
    {
        notifivationTMP.text = message;
        //�������� 1�̾��ٰ� 0.9f �Ŀ� 0�� ��
        Sequence sequence = DOTween.Sequence()
            .Append(transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutQuad))
            .AppendInterval(0.9f)
            .Append(transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InQuad));
    }

    void Start()
    {
        
    }
    [ContextMenu("ScaleOne")]
    void ScaleOne() => transform.localScale = Vector3.one;
    [ContextMenu("ScaleZero")]
    void ScaleZero() => transform.localScale = Vector3.zero;

    
}

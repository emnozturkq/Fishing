using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Balık : MonoBehaviour
{
    private Balık.BalıkType type;
    private CircleCollider2D coll;
    private SpriteRenderer rend;
    private float screenLeft;
    private Tweener tweener;

    public Balık.BalıkType Type
    {
        get
        {
            return type;
        }
        set
        {
            type = value;
            coll.radius = type.colliderCap;
            rend.sprite = type.sprite;
        }
    }

    void Awake()
    {
        coll = GetComponent<CircleCollider2D>();
        rend = GetComponentInChildren<SpriteRenderer>();
        screenLeft = Camera.main.ScreenToWorldPoint(Vector3.zero).x;
    }

    public void Reset()
    {
        if(tweener != null)
        {
            tweener.Kill(false);
        }

        float num = UnityEngine.Random.Range(type.minUzunluk, type.maxUzunluk);
        coll.enabled = true;

        Vector3 position = transform.position;
        position.y = num;
        position.x = screenLeft;
        transform.position = position;

        float num2 = 1;
        float y = UnityEngine.Random.Range(num - num2, num + num2);
        Vector2 v = new Vector2 (-position.x, y);

        float num3 = 3;
        float ert = UnityEngine.Random.Range(0, 2 * num3);
        tweener = transform.DOMove(v, num3, false).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear).SetDelay(ert).OnStepComplete(delegate
        {
            Vector3 localScale = transform.localScale;
            localScale.x = -localScale.x;
            transform.localScale = localScale;
        });
    }

    public void yakalanma()
    {
        coll.enabled = false;
        tweener.Kill(false);
    }
    [Serializable]
    public class BalıkType
    {
        public int bedel;
        public float balıkSayısı;
        public float minUzunluk;
        public float maxUzunluk;
        public float colliderCap;
        public Sprite sprite;
    }
}

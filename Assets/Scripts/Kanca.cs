using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Kanca : MonoBehaviour
{
    public Transform yakalananTransform;
    private bool canMove;
    public Transform kancaHareket;
    private Collider2D coll;
    private Camera mainCamera;
    private int kuvvet;
    private int sayac;
    private int uzunluk;
    private Tweener cameraTween;
    private List<Balık> yakalananBalık;


    void Awake()
    {
        mainCamera = Camera.main;
        coll = GetComponent<Collider2D>();
        yakalananBalık = new List<Balık>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove && Input.GetMouseButton(0))
        {
            Vector3 vector = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 position = transform.position;
            position.x = vector.x;
            transform.position = position;
        }
    }

    public void startFishing()
    {
        float t = (-uzunluk) * 0.1f;
        uzunluk = -50;
        kuvvet = 3;
        sayac = 0;

        cameraTween = mainCamera.transform.DOMoveY(uzunluk, 1 + t * 0.25f, false).OnUpdate(delegate
        {
            if (mainCamera.transform.position.y <= -11)
                transform.SetParent(mainCamera.transform);
        }).OnComplete(delegate
        {
            coll.enabled = true;
            cameraTween = mainCamera.transform.DOMoveY(0, t * 5, false).OnUpdate(delegate
            {
                if (mainCamera.transform.position.y >= -25f)
                    stopFishing();
            });
        });
        coll.enabled = false;
        canMove = true;
        yakalananBalık.Clear();

    }

    void stopFishing()
    {
        canMove = false;
        cameraTween.Kill(false);
        cameraTween = mainCamera.transform.DOMoveY(0, 2, false).OnUpdate(delegate
        {
            if (mainCamera.transform.position.y >= -11)
            {
                transform.SetParent(null);
                transform.position = new Vector2(transform.position.x, -6);
            }
        }).OnComplete(delegate
        {
            transform.position = Vector2.down * 6;
            coll.enabled = true;
            int num = 0;
            for(int i = 0; i <yakalananBalık.Count; i++)
            {
                yakalananBalık[i].transform.SetParent(null);
                yakalananBalık[i].Reset();
                num += yakalananBalık[i].Type.bedel;
            }
        });
    }

    private void OnTriggerEnter2D(Collider2D target)
    {
        if(target.CompareTag("Balık") && sayac != kuvvet)
        {
            sayac++;
            Balık component = target.GetComponent<Balık>();
            component.yakalanma();
            yakalananBalık.Add(component);
            target.transform.SetParent(transform);
            target.transform.position = yakalananTransform.position;
            target.transform.rotation = yakalananTransform.rotation;
            target.transform.localScale = Vector3.one;

            target.transform.DOShakeRotation(5, Vector3.forward * 45, 10, 90, false).SetLoops(1, LoopType.Yoyo).OnComplete(delegate
            {
                target.transform.rotation = Quaternion.identity;
            });
            if (sayac == kuvvet)
                stopFishing();
        }
    }
}

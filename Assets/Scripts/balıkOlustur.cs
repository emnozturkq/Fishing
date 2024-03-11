using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Balık;

public class balıkOlustur : MonoBehaviour
{
    void Awake()
    {
        for(int i = 0; i < balıkTypes.Length;  i++)
        {
            int num = 0;
            while(num < balıkTypes[i].balıkSayısı)
            {
                Balık balık = UnityEngine.Object.Instantiate<Balık>(balıkPrefab);
                balık.Type = balıkTypes[i];
                balık.Reset();
                num++;
            }
        }    
    }

    public Balık balıkPrefab;
    public Balık.BalıkType[] balıkTypes;

}

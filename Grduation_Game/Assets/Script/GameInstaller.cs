using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Zenject;
/*------------------by 017---------------------- - */
public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        // �j�w PlayerStats ����ҩίS�w���
        Container.Bind<PlayerStats>().FromInstance(new PlayerStats
        {
            Health = 100,
            Defence = 100,
            Speed = 10,
            Power = 100,
            Attack = 100
        }).AsSingle();
    }
}

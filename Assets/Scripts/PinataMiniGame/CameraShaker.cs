using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using PinataMiniGame;
using UnityEngine;

public class CameraShaker : PinataListener
{
    private Camera _camera;
    [SerializeField] private float _hitShakeDuration = 0.1f;
    [SerializeField] private float _hitShakeStrength = 0.1f;
    [SerializeField] private float _explodeShakeDuration = 0.1f;
    [SerializeField] private float _explodeShakeStrength = 0.1f;

    private void Awake()
    {
        _camera = Camera.main;
    }

    protected override void OnExplosion()
    {
        ShakeCamera(_explodeShakeDuration, _explodeShakeStrength);
    }

    protected override void OnPhasePass(int index)
    {
        
    }

    protected override void OnHit(float charge)
    {
        ShakeCamera(_hitShakeDuration, _hitShakeStrength);
    }

    protected override void OnReset()
    {
        
    }

    private void ShakeCamera(float duration, float strength)
    {
        _camera.transform.DOShakePosition(duration, Vector3.one * strength);
    }
}

using System.Collections.Generic;
using UnityEngine;

public class PlayerFX : IObserverClass
{
    [SerializeField] private ParticleSystem jumpFX;
    [SerializeField] private GameObject DashFX;
    [SerializeField] private AnimationCurve hitFXCurve;
    private readonly List<ParticleSystem> _jumpFX = new();

    public override void OnNotify(Task t)
    {
        switch (t)
        {
            case Task.JumpStart:
               JumpFX();
            break;

            case Task.JumpStop:
                JumpFXDelete();
            break;

            case Task.DashStart:
                DashFXOn();
            break;

            case Task.DashStop:
                DashFXStop();
            break;

            case Task.OnHit:
                HitFX();
            break;
        }
    }

    private void JumpFX()
    {
        _jumpFX.Add(Instantiate(jumpFX));
        _jumpFX[^1].transform.position = transform.position;
        _jumpFX[^1].transform.parent = transform;

        AudioManager.i.PlayJump();
    }

    private void JumpFXDelete()
    {
        for (int i = 0; i < _jumpFX.Count; i++)
        {
            if((_jumpFX[i] != null) && !_jumpFX[i].IsAlive())
            {
                Destroy(_jumpFX[i].gameObject);
                _jumpFX.Remove(_jumpFX[i]);
            }
        }
    }

    private float time;
    private void DashFXOn()
    {
        time = 0;
        DashFX.SetActive(true);
    }

    private void DashFXStop()
    {
        time += Time.deltaTime;
        if(time >= 1) DashFX.SetActive(false);
    }

    private void HitFX() {

        AudioManager.i.PlayHit();
    }

}

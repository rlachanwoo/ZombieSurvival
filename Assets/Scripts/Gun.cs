using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public enum State
    {
        Ready, Empty, Reloading
    }

    public State CurrentState {get; private set;}

    public Transform fireTransform;

    public ParticleSystem muzzleFlashEffect;
    public ParticleSystem shellEjectEffect;
    private LineRenderer bulletLineRenderer;
    private AudioSource gunAudioPlayer;
    public GunData gunData;

    private float fireDistance = 50f;

    private PlayerInput playerInput;

    private int ammoRemain;
    private int magAmmo;
    private float lastFireTime;

    public UIManager uiManager;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        bulletLineRenderer =GetComponent<LineRenderer>();
        gunAudioPlayer = GetComponent<AudioSource>();

        bulletLineRenderer.positionCount = 2;
        bulletLineRenderer.enabled = false;
    }

    private void OnEnable()
    {
        ammoRemain = gunData.startAmmoRemain;
        magAmmo = gunData.magCapacity;
        uiManager.SetAmmoText(magAmmo, ammoRemain);
        CurrentState = State.Ready;
        lastFireTime = 0f;
    }

    public void Fire()
    {
        if (CurrentState == State.Ready && Time.time >= lastFireTime + gunData.timeBetFire)
        {
            lastFireTime = Time.time;
            shoot();
        }
    }

    public LayerMask targetLayer;

    private void shoot()    // 히트스캔
    {
        Vector3 hitPosition;

        Ray ray = new Ray(fireTransform.position, fireTransform.forward);
        if(Physics.Raycast(ray, out RaycastHit hit, fireDistance))
        {
            hitPosition = hit.point;

            IDamageable target = hit.collider.GetComponent<IDamageable>();
            if (target != null)
            {
                target.OnDamage(gunData.damage, hit.point, hit.normal);
            }
        }
        else
        {
            hitPosition = fireTransform.position + fireTransform.forward * fireDistance;
        }


        StartCoroutine(ShotEffect(hitPosition));

        magAmmo--;
        uiManager.SetAmmoText(magAmmo, ammoRemain);
        if (magAmmo == 0)
        {           
            CurrentState = State.Empty;
        }
    }

    private IEnumerator ShotEffect(Vector3 hitPosition)
    {
        muzzleFlashEffect.Play();
        shellEjectEffect.Play();
        gunAudioPlayer.PlayOneShot(gunData.shotClip);

        bulletLineRenderer.SetPosition(0, fireTransform.position);
        bulletLineRenderer.SetPosition(1, hitPosition);
        bulletLineRenderer.enabled = true;
        yield return new WaitForSeconds(1f); // yield return: 일시정지

        bulletLineRenderer.enabled = false;
        
    }

    public bool Reload()
    {
        if (playerInput.Reload || magAmmo == 0)
            {
                magAmmo = gunData.magCapacity;
            } 

        return false;
    }
}

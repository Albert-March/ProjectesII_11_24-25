using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Attack_Laser : MonoBehaviour, IAttackType
{
    private GameObject laserPrefab;
    private string assetAddress = "Prefabs/NewLaser";

    private HashSet<Enemy> activeTargets = new HashSet<Enemy>();
    private int activeLaserCount = 0;

    private float lastShotTime = 0f;
    private GameObject laser;
    private Enemy target;

    private Vector2 direction;
    private float angle;
    private float distance;

    private bool isFiringType0 = false;
    private int animatingLasers = 0;
    private bool canFireType2 = true;

    private int laserLoopAudioIndex = 13;

    private int previousTowerLevel = -1;

    private AudioSource myLoopSource = null;


    void Start()
    {
        Addressables.LoadAssetAsync<GameObject>(assetAddress).Completed += OnPrefabLoaded;
    }

    private void OnPrefabLoaded(AsyncOperationHandle<GameObject> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            laserPrefab = handle.Result;
        }
    }

    public void Attack(List<Enemy> e, int TargetAmount, Animator anim, AudioManager audio, int targetType, TargetingManager targetManager)
    {
        Tower tower = GetComponent<Tower>();

        if (previousTowerLevel != tower.currentLevel)
        {
            if (myLoopSource != null)
            {
                AudioManager.instance.StopSFXLoop(myLoopSource);
                myLoopSource = null;
            }
            previousTowerLevel = tower.currentLevel;
        }

        if (tower.type == 0)
        {
            if (laserPrefab == null || e == null || e.Count == 0) 
            {
                StopAllCoroutines();
                return;
            };
            if (Time.time < lastShotTime + 1f / tower.fireRate) return;
            if (isFiringType0) return;

            target = targetManager.GetEnemyTargetFromList(e, 1, targetType)[0];
            if (target == null) return;

            isFiringType0 = true;
            anim.speed = tower.fireRate;
            anim.SetBool("IsAttacking", true);
            audio.PlaySFX(7, 0.2f);
            StartCoroutine(SyncType0Shot(anim, tower.fireRate));
        }
        else if (tower.type == 1)
        {
            if (laserPrefab == null || e == null || anim == null) return;

            int allowed = TargetAmount - activeLaserCount;
            if (allowed <= 0) return;

            int assigned = 0;
            List<Enemy> enemyHolder = targetManager.GetEnemyTargetFromList(e, TargetAmount * 2, targetType);

            for (int i = 0; i < enemyHolder.Count; i++)
            {
                Enemy candidate = enemyHolder[i];
                if (candidate == null || activeTargets.Contains(candidate)) continue;

                activeTargets.Add(candidate);
                activeLaserCount++;
                StartCoroutine(HandleLaserAttack(candidate, anim));
                assigned++;

                if (assigned >= allowed)
                    break;
            }
        }
        else if (tower.type == 2)
        {
            if (!canFireType2) return;

            if (laserPrefab == null || e == null)
            {
                StopAllCoroutines();
                return;
            };
            Enemy candidate = targetManager.GetEnemyTargetFromList(e, 1, targetType)[0];
            if (candidate == null || activeTargets.Contains(candidate)) return;

            activeTargets.Add(candidate);
            activeLaserCount++;
            canFireType2 = false;

            StartCoroutine(HandleLaserRampDamage(candidate, anim));
        }
    }

    IEnumerator SyncType0Shot(Animator anim, float fireRate)
    {
        while (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            yield return null;

        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 40f / 60f)
            yield return null;

        if (target == null)
        {
            anim.SetBool("IsAttacking", false);
            isFiringType0 = false;
            yield break;
        }

        DrawLaser();
        IDamage enemyDmg = target?.GetComponent<IDamage>();
        if (enemyDmg != null)
            enemyDmg.Damage(GetComponent<Tower>().damage);

        yield return new WaitForSeconds(1f / fireRate);

        anim.SetBool("IsAttacking", false);
        isFiringType0 = false;
        lastShotTime = Time.time;
    }

    void DrawLaser()
    {
        if (target == null) return;

        laser = Instantiate(laserPrefab, this.transform.position, Quaternion.identity);
        laser.transform.SetParent(this.transform);
        
        GameObject GOline = laser.transform.Find("Line").gameObject;
        GameObject GOstart = laser.transform.Find("Start").gameObject;
        GameObject GOend = laser.transform.Find("End").gameObject;

        Transform startVFX = GOstart.transform;
        Transform lineObj = GOline.transform;
        Transform endVFX = GOend.transform;

        Color newColor = Color.yellow;

        LineRenderer lr = lineObj.GetComponent<LineRenderer>();
        lr.startColor = newColor;
        lr.endColor = newColor;

        ParticleSystem startParticles = startVFX.GetComponent<ParticleSystem>();
        var startRenderer = startVFX.GetComponent<ParticleSystemRenderer>();
        startRenderer.material = new Material(startRenderer.material);
        startRenderer.material.SetColor("_BaseColor", newColor);

        ParticleSystem endParticles = endVFX.GetComponent<ParticleSystem>();
        var endRenderer = endVFX.GetComponent<ParticleSystemRenderer>();
        endRenderer.material = new Material(endRenderer.material);
        endRenderer.material.SetColor("_BaseColor", newColor);

        Vector3 targetWorld = target.transform.position;

        endVFX.transform.position = targetWorld;

        LineRenderer lineRenderer = GOline.GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = false;
        lineRenderer.SetPosition(0, laser.transform.InverseTransformPoint(transform.position));
        lineRenderer.SetPosition(1, laser.transform.InverseTransformPoint(target.transform.position));

        Vector3 dir = (targetWorld - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        startVFX.localRotation = Quaternion.Euler(-angle, 0f, 0f);
        endVFX.localRotation = Quaternion.Euler(-angle + 180f, 0f, 0f);

        StartCoroutine(DeleteLaser(lineRenderer, laser));
    }

    IEnumerator DeleteLaser(LineRenderer lineRenderer, GameObject laserObj)
    {
        if (lineRenderer == null) yield break;

        float initialWidth = lineRenderer.startWidth * 2f;
        lineRenderer.startWidth = initialWidth;
        lineRenderer.endWidth = initialWidth;

        float shrinkDuration = 1f;
        float elapsed = 0f;

        while (elapsed < shrinkDuration && lineRenderer != null)
        {
            elapsed += Time.deltaTime;
            float width = Mathf.Lerp(initialWidth, 0f, elapsed / shrinkDuration);
            lineRenderer.startWidth = width;
            lineRenderer.endWidth = width;
            yield return null;
        }

        if (laserObj != null)
        {
            ParticleSystem[] particles = laserObj.GetComponentsInChildren<ParticleSystem>();
            for (int i = 0; i < particles.Length; i++)
            {
                particles[i].Stop();
                particles[i].Clear();
            }
            Destroy(laserObj);
        }
    }

    IEnumerator HandleLaserAttack(Enemy target, Animator anim)
    {
        Tower tower = GetComponent<Tower>();

        anim.SetBool("IsAttacking", true);


        if (myLoopSource == null)
        {
            myLoopSource = AudioManager.instance.PlaySFXLoopUnique(laserLoopAudioIndex, 0.5f);
        }
        else
        {
            float newPitch = Mathf.Clamp(1f - (activeLaserCount * 0.05f), 0.5f, 1f);
            if (myLoopSource != null)
            {
                myLoopSource.pitch = newPitch;
            }
        }

        animatingLasers++;

        while (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            yield return null;

        anim.speed = tower.fireRate;

        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < (40f / 60f))
            yield return null;

        anim.speed = 0f;

        Vector3 startWorld = tower.transform.position;
        GameObject laserObj = Instantiate(laserPrefab, startWorld, Quaternion.identity);
        laserObj.transform.SetParent(tower.transform);

        Transform startVFX = laserObj.transform.Find("Start");
        Transform lineObj = laserObj.transform.Find("Line");
        Transform endVFX = laserObj.transform.Find("End");

        Color newColor = Color.yellow;
        if (tower.currentLevel == 2 || tower.currentLevel == 3) newColor = Color.yellow;

        LineRenderer lr = lineObj.GetComponent<LineRenderer>();
        lr.useWorldSpace = false;
        lr.startColor = newColor;
        lr.endColor = newColor;

        var startRenderer = startVFX.GetComponent<ParticleSystemRenderer>();
        startRenderer.material = new Material(startRenderer.material);
        startRenderer.material.SetColor("_BaseColor", newColor);

        var endRenderer = endVFX.GetComponent<ParticleSystemRenderer>();
        endRenderer.material = new Material(endRenderer.material);
        endRenderer.material.SetColor("_BaseColor", newColor);

        Vector3 startLocal = startVFX.localPosition;
        lr.SetPosition(0, startLocal);
        lr.SetPosition(1, startLocal);

        float t = 0f;
        Vector3 velocity = Vector3.zero;
        Vector3 endLocal = Vector3.zero;

        while (t < 1f)
        {
            if (target == null)
            {
                CleanupLaser(anim, laserObj);
                yield break;
            }

            t += Time.deltaTime * 5f;
            Vector3 targetWorld = target.transform.position;
            endLocal = laserObj.transform.InverseTransformPoint(targetWorld);

            Vector3 interpolatedEnd = Vector3.Lerp(startLocal, endLocal, t);
            lr.SetPosition(1, interpolatedEnd);
            endVFX.localPosition = interpolatedEnd;

            float angle = Mathf.Atan2(interpolatedEnd.y - startLocal.y, interpolatedEnd.x - startLocal.x) * Mathf.Rad2Deg;
            startVFX.localRotation = Quaternion.Euler(-angle, 90f, 0f);
            endVFX.localRotation = Quaternion.Euler(angle, -90f, 0f);

            yield return null;
        }

        float fireCooldown = 0f;
        Vector3 currentEnd = lr.GetPosition(1);

        while (true)
        {
            if (target == null)
            {
                CleanupLaser(anim, laserObj);
                yield break;
            }

            fireCooldown -= Time.deltaTime;

            Vector3 targetWorld = target.transform.position;
            endLocal = laserObj.transform.InverseTransformPoint(targetWorld);

            currentEnd = Vector3.SmoothDamp(currentEnd, endLocal, ref velocity, 0.05f);
            lr.SetPosition(1, currentEnd);
            endVFX.localPosition = currentEnd;

            float angle = Mathf.Atan2(currentEnd.y - startLocal.y, currentEnd.x - startLocal.x) * Mathf.Rad2Deg;
            startVFX.localRotation = Quaternion.Euler(-angle, 90f, 0f);
            endVFX.localRotation = Quaternion.Euler(angle, -90f, 0f);

            if (fireCooldown <= 0f)
            {
                IDamage dmg = target.GetComponent<IDamage>();
                if (dmg != null) dmg.Damage(tower.damage);
                fireCooldown = 1f / tower.fireRate;
            }

            yield return null;
        }
    }

    void CleanupLaser(Animator anim, GameObject laserObj)
    {
        activeTargets.RemoveWhere(enemy => enemy == null);
        activeLaserCount--;
        animatingLasers--;

        if (animatingLasers <= 0)
        {
            anim.speed = 1f;
            anim.SetBool("IsAttacking", false);

            if (myLoopSource != null)
            {
                AudioManager.instance.StopSFXLoop(myLoopSource);
                myLoopSource = null;
            }
        }
        else if (myLoopSource != null)
        {
            float newPitch = Mathf.Clamp(1f - (activeLaserCount * 0.01f), 0.5f, 1f);
            myLoopSource.pitch = newPitch;
        }

        if (laserObj != null) Destroy(laserObj);
    }

    IEnumerator HandleLaserRampDamage(Enemy target, Animator anim)
    {
        Tower tower = GetComponent<Tower>();

        anim.SetBool("IsAttacking", true);

        if (myLoopSource == null)
        {
            myLoopSource = AudioManager.instance.PlaySFXLoopUnique(laserLoopAudioIndex, 0.5f);
        }

        while (!anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") &&
               !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack2"))
            yield return null;

        anim.speed = tower.fireRate;

        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 40f / 60f)
            yield return null;

        anim.speed = 0f;

        Vector3 startWorld = tower.transform.position;

        GameObject laserObj = Instantiate(laserPrefab, startWorld, Quaternion.identity);
        laserObj.transform.SetParent(tower.transform);

        Transform startVFX = laserObj.transform.Find("Start");
        Transform lineObj = laserObj.transform.Find("Line");
        Transform endVFX = laserObj.transform.Find("End");

        LineRenderer lr = lineObj.GetComponent<LineRenderer>();
        lr.useWorldSpace = false;
        lr.startColor = LerpLaserColor(0, tower.currentLevel); ;
        lr.endColor = LerpLaserColor(0, tower.currentLevel); ;
        Vector3 startLocal = startVFX.localPosition;
        lr.SetPosition(0, startLocal);
        lr.SetPosition(1, startLocal);

        float t = 0f;
        Vector3 velocity = Vector3.zero;
        Vector3 endLocal = Vector3.zero;

        while (t < 1f)
        {
            if (target == null)
            {
                CleanupLaserRamp(anim, laserObj);
                yield break;
            }

            t += Time.deltaTime * 5f;
            Vector3 targetWorld = target.transform.position;
            endLocal = laserObj.transform.InverseTransformPoint(targetWorld);

            Vector3 interpolatedEnd = Vector3.Lerp(startLocal, endLocal, t);
            lr.SetPosition(1, interpolatedEnd);
            endVFX.localPosition = interpolatedEnd;

            Vector3 dir = (interpolatedEnd - startLocal).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            startVFX.localRotation = Quaternion.Euler(-angle, 0f, 0f);
            endVFX.localRotation = Quaternion.Euler(-angle + 180f, 0f, 0f);

            yield return null;
        }

        float fireCooldown = 0f;
        float damageMultiplier = 1f;
        Vector3 currentEnd = lr.GetPosition(1);

        while (true)
        {
            if (target == null)
            {
                CleanupLaserRamp(anim, laserObj);
                yield break;
            }

            fireCooldown -= Time.deltaTime;

            Vector3 targetWorld = target.transform.position;
            endLocal = laserObj.transform.InverseTransformPoint(targetWorld);

            currentEnd = Vector3.SmoothDamp(currentEnd, endLocal, ref velocity, 0.05f);
            lr.SetPosition(1, currentEnd);
            endVFX.localPosition = currentEnd;

            Vector3 dir = (currentEnd - startLocal).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            startVFX.localRotation = Quaternion.Euler(-angle, 90f, 0f);
            endVFX.localRotation = Quaternion.Euler(angle, -90f, 0f);

            float currentDamage = tower.damage * damageMultiplier;
            float progress = 0f;
            if (tower.currentLevel == 2)
                progress = Mathf.Clamp01(currentDamage / 20f);
            else if (tower.currentLevel == 3)
                progress = Mathf.Clamp01((currentDamage - 20f) / 10f);

            Color currentColor = LerpLaserColor(progress, tower.currentLevel);

            float pitch = Mathf.Lerp(1f, 1.5f, progress);
            AudioManager.instance.SetLoopPitch(laserLoopAudioIndex, pitch);

            lr.startColor = currentColor;
            lr.endColor = currentColor;

            if (fireCooldown <= 0f)
            {
                IDamage enemyDmg = target.GetComponent<IDamage>();
                if (enemyDmg != null)
                    enemyDmg.Damage(tower.damage * damageMultiplier);

                if (tower.currentLevel == 2)
                {
                    if (tower.damage * damageMultiplier <= 50)
                        damageMultiplier *= 1.202264f;
                }
                else if (tower.currentLevel == 3)
                {
                    if (tower.damage * damageMultiplier <= 66)
                        damageMultiplier *= 1.22573f;
                }

                fireCooldown = 1f / tower.fireRate;
            }

            yield return null;
        }
    }

    void CleanupLaserRamp(Animator anim, GameObject laserObj)
    {
        activeTargets.RemoveWhere(enemy => enemy == null);
        activeLaserCount--;

        anim.speed = 1f;
        anim.SetBool("IsAttacking", false);

        canFireType2 = true;

        if (laserObj != null) Destroy(laserObj);

        if (myLoopSource != null)
        {
            AudioManager.instance.StopSFXLoop(myLoopSource);
            myLoopSource = null;
        }
    }

    Color LerpLaserColor(float progress, int level)
    {
        if (level == 2)
            return Color.Lerp(Color.yellow, Color.red, progress);
        else if (level == 3)
            return Color.Lerp(Color.red, Color.green, progress);
        return Color.white;
    }
}




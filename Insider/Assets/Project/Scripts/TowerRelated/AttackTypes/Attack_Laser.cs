using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
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

        if (tower.type == 0)
        {
            if (laserPrefab == null || e == null || e.Count == 0) return;
            if (Time.time < lastShotTime + 1f / tower.fireRate) return;

            target = targetManager.GetEnemyTargetFromList(e, 1, targetType)[0];
            if (target == null) return;


            DrawLaser();

            IDamage enemyDmg = target.GetComponent<IDamage>();
            if (enemyDmg != null)
            {
                enemyDmg.Damage(tower.damage);
            }

            lastShotTime = Time.time;
        }
        else if (tower.type == 1)
        {
            if (laserPrefab == null || e == null) return;

            int allowed = TargetAmount - activeLaserCount;
            if (allowed <= 0) return;

            int assigned = 0;
            List<Enemy> enemyHolder = targetManager.GetEnemyTargetFromList(e, TargetAmount * 2, targetType);

            foreach (Enemy candidate in enemyHolder)
            {
                if (candidate == null || activeTargets.Contains(candidate)) continue;

                activeTargets.Add(candidate);
                activeLaserCount++;
                StartCoroutine(HandleLaserAttack(candidate));
                assigned++;

                if (assigned >= allowed)
                    break;
            }
        }
        else if (tower.type == 2)
        {
            if (laserPrefab == null || e == null) return;

            Enemy candidate = targetManager.GetEnemyTargetFromList(e, 1, targetType)[0];
            if (candidate == null || activeTargets.Contains(candidate)) return;

            activeTargets.Add(candidate);
            activeLaserCount++;
            StartCoroutine(HandleLaserRampDamage(candidate));
        }
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
            foreach (var ps in particles)
            {
                ps.Stop();
                ps.Clear();
            }
            Destroy(laserObj);
        }
    }

    IEnumerator HandleLaserAttack(Enemy target)
    {
        Tower tower = GetComponent<Tower>();
        Vector3 startWorld = tower.transform.position;

        GameObject laserObj = Instantiate(laserPrefab, startWorld, Quaternion.identity);
        laserObj.transform.SetParent(tower.transform);

        GameObject GOstartVFX = laserObj.transform.Find("Start").gameObject;
        GameObject GOlineObj = laserObj.transform.Find("Line").gameObject;
        GameObject GOendVFX = laserObj.transform.Find("End").gameObject;

        Transform startVFX = GOstartVFX.transform;
        Transform lineObj = GOlineObj.transform;
        Transform endVFX = GOendVFX.transform;

        Color newColor = Color.yellow;

        if (tower.currentLevel == 2)
        {
            newColor = Color.yellow;
        }
        else if (tower.currentLevel == 3)
        {
            newColor = Color.yellow;
        }

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

        laserObj.transform.position = startWorld;
        Vector3 startLocal = startVFX.localPosition;

        lr.useWorldSpace = false;
        lr.SetPosition(0, startLocal);
        lr.SetPosition(1, startLocal);

        float t = 0f;
        Vector3 velocity = Vector3.zero;
        Vector3 endLocal = Vector3.zero;

        while (t < 1f && target != null)
        {
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

        if (target == null)
        {
            activeTargets.Remove(target);
            activeLaserCount--;
            Destroy(laserObj);
            yield break;
        }

        float fireCooldown = 0f;
        Vector3 currentEnd = lr.GetPosition(1);

        while (target != null)
        {
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

            if (fireCooldown <= 0f)
            {
                IDamage enemyDmg = target.GetComponent<IDamage>();
                if (enemyDmg != null)
                    enemyDmg.Damage(tower.damage);

                fireCooldown = 1f / tower.fireRate;
            }

            yield return null;
        }

        Vector3 returnStart = currentEnd;
        float returnT = 0f;

        while (returnT < 1f)
        {
            returnT += Time.deltaTime * 5f;
            Vector3 back = Vector3.Lerp(returnStart, startLocal, returnT);
            lr.SetPosition(1, back);
            endVFX.localPosition = back;

            Vector3 dir = (back - startLocal).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            startVFX.localRotation = Quaternion.Euler(-angle, 0f, 0f);
            endVFX.localRotation = Quaternion.Euler(angle, -90f, 0f);

            yield return null;
        }

        activeTargets.Remove(target);
        activeLaserCount--;

        Destroy(laserObj);
    }
    IEnumerator HandleLaserRampDamage(Enemy target)
    {
        Tower tower = GetComponent<Tower>();
        Vector3 startWorld = tower.transform.position;

        GameObject laserObj = Instantiate(laserPrefab, startWorld, Quaternion.identity);
        laserObj.transform.SetParent(tower.transform);

        GameObject GOstartVFX = laserObj.transform.Find("Start").gameObject;
        GameObject GOlineObj = laserObj.transform.Find("Line").gameObject;
        GameObject GOendVFX = laserObj.transform.Find("End").gameObject;

        Transform startVFX = GOstartVFX.transform;
        Transform lineObj = GOlineObj.transform;
        Transform endVFX = GOendVFX.transform;

        LineRenderer lr = lineObj.GetComponent<LineRenderer>();
        lr.useWorldSpace = false;

        Vector3 startLocal = startVFX.localPosition;
        lr.SetPosition(0, startLocal);
        lr.SetPosition(1, startLocal);

        float t = 0f;
        Vector3 velocity = Vector3.zero;
        Vector3 endLocal = Vector3.zero;

        while (t < 1f && target != null)
        {
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

        if (target == null)
        {
            activeTargets.Remove(target);
            activeLaserCount--;
            Destroy(laserObj);
            yield break;
        }

        float fireCooldown = 0f;
        float damageMultiplier = 1f;
        Vector3 currentEnd = lr.GetPosition(1);

        while (target != null)
        {
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

            if (fireCooldown <= 0f)
            {
                IDamage enemyDmg = target.GetComponent<IDamage>();
                if (enemyDmg != null)
                    enemyDmg.Damage(tower.damage * damageMultiplier);

                damageMultiplier *= 1.1f;
                fireCooldown = 1f / tower.fireRate;
            }

            yield return null;
        }

        activeTargets.Remove(target);
        activeLaserCount--;
        Destroy(laserObj);
    }

}




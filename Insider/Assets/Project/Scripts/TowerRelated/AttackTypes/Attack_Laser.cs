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
            // TODO: Implement type 2
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

        Color newColor = new Color32(0, 0, 0, 255);

        if (tower.currentLevel == 2)
        {
            newColor = new Color32(255, 255, 255, 255);
        }
        else if (tower.currentLevel == 3)
        {
            newColor = new Color32(0, 0, 0, 255);
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
}
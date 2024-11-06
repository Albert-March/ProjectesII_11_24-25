using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    public List<Target> allTargets = new List<Target>();
    public List<List<Target>> targetLists = new List<List<Target>>();

    // Start is called before the first frame update
    private void Awake()
    {
        GetAllTargets();
        SortTargetsInToLists();
    }

    private void GetAllTargets()
    {
        GameObject father = GameObject.FindGameObjectWithTag("TargetPoint");

        for (int i = 0; i < father.transform.childCount; i++)
        {
            Target targetHolder = new Target();
            targetHolder.obj = father.transform.GetChild(i).gameObject;
            allTargets.Add(targetHolder);
        }
    }

    private void SortTargetsInToLists()
    {
        foreach (Target target in allTargets)
        {
            string targetName = target.obj.name;
            int groupId;

            if (int.TryParse(targetName.Split('.')[0], out groupId))
            {
                int listIndex = groupId - 1;

                while (targetLists.Count <= listIndex)
                {
                    targetLists.Add(new List<Target>());
                }

                targetLists[listIndex].Add(target);
            }
            else
            {
                Debug.Log("Failed to parse group ID from target name:" + targetName);
            }
        }
    }

    public List<Target> GetRandomPath()
    {
        int randomNum = Random.Range(0, targetLists.Count);
        return targetLists[randomNum];
    }

}

public class Target
{
    public GameObject obj;
}

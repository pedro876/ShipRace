using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Assertions;

public class LevelManager : MonoBehaviour
{
    Transform player;

    [SerializeField] Tunnel initSection;
    [SerializeField] List<Tunnel> allTunnelPrefabs;
    Tunnel[] rectTunnelPrefabs;
    Tunnel[] tunnelCurvePrefabs;
    [SerializeField] Obstacle[] obstaclePrefabs;
    [SerializeField] int numOfSections = 3;
    [SerializeField] int sectionsBetweenCurves = 10;
    [SerializeField] int initialSectionsWithoutObstacles = 3;
    [SerializeField] int delaySectionReplacement = 1;
    //[SerializeField] float distanceToReplaceLast = 60f;
    Queue<Tunnel> sectionsToRemove;
    Queue<Tunnel> sections;
    Tunnel lastSection;
    int sectionIndex = 0;
    int sectionType = 0;
    bool checkRemove = false;

    //List<ObjectPool<Obstacle>> obstaclePools;
    ComponentPool<Obstacle> obstaclePool;
    ComponentPool<Tunnel> curvePool;
    List<ComponentPool<Tunnel>> tunnelPools;

    public Tunnel currentSection => sections.Peek();

    public void SetPlayer(Transform newPlayer)
    {
        Assert.IsNotNull(newPlayer, "Player assigned to level is null");
        player = newPlayer;
    }

    public void ResetLevel()
    {
        if (sections != null)
        {
            while (sections.Count > 0)
                RemoveSection(sections.Dequeue());
        }
        else
            sections = new Queue<Tunnel>();

        if (sectionsToRemove != null)
        {
            while (sectionsToRemove.Count > 0)
                RemoveSection(sectionsToRemove.Dequeue());
        }
        else
            sectionsToRemove = new Queue<Tunnel>();

        for (int i = 0; i < transform.childCount; i++)
            Destroy(transform.GetChild(i).gameObject);
        lastSection = null;
        sectionIndex = 0;
        sectionType = 0;
        for (int i = 0; i < numOfSections; i++)
        {
            AddSection();
        }
    }

    public void CheckRemove()
    {
        checkRemove = true;
    }

    public void Pause()
    {
        foreach(var section in sections)
        {
            section.Pause();
        }
    }

    public void Resume()
    {
        foreach (var section in sections)
        {
            section.Resume();
        }
    }

    private void Awake()
    {
        //player = FindObjectOfType<ShipController>().transform;
        rectTunnelPrefabs = allTunnelPrefabs.FindAll(tunnel => !tunnel.IsCurve()).ToArray();
        tunnelCurvePrefabs = allTunnelPrefabs.FindAll(tunnel => tunnel.IsCurve()).ToArray();
        allTunnelPrefabs = null;
        obstaclePool = new ComponentPool<Obstacle>("obstaclePool", numOfSections + 2, obstaclePrefabs);
        curvePool = new ComponentPool<Tunnel>("tunnelCurvePool", System.Math.Max(4, (numOfSections / sectionsBetweenCurves)), tunnelCurvePrefabs);
        tunnelPools = new List<ComponentPool<Tunnel>>(rectTunnelPrefabs.Length);
        for(int i = 0; i < rectTunnelPrefabs.Length; i++)
        {
            rectTunnelPrefabs[i].sectionIndex = i;
            tunnelPools.Add(new ComponentPool<Tunnel>(rectTunnelPrefabs[i].name + "_pool", numOfSections, rectTunnelPrefabs[i]));
        }

        GameManager.instance.onStateChanged += state =>
        {
            checkRemove = state == GameManager.GameState.Game;
        };
    }
    void Start()
    {
        ResetLevel();
    }

    private void AddSection()
    {
        Tunnel sectionObj;
        //Tunnel section;
        Vector3 pos;
        Quaternion rot;
        if(sectionIndex == 0) //Init
        {
            sectionObj = Instantiate(initSection, transform);
            sectionObj.sectionIndex = -1;
            sectionType = Random.Range(0, rectTunnelPrefabs.Length);
            pos = transform.position;
            rot = transform.rotation;
        }
        else
        {
            if (sectionIndex % sectionsBetweenCurves == 0) //Curve
            {
                //var curve = RandomTunnel(tunnelCurvePrefabs);
                //sectionObj = Instantiate(curve, transform);
                sectionObj = RandomCurve();
                sectionType = Random.Range(0, rectTunnelPrefabs.Length);
            }
            else //Normal section
            {
                //sectionObj = Instantiate(rectTunnelPrefabs[sectionType], transform);
                sectionObj = tunnelPools[sectionType].Get();
            }
            pos = lastSection.tunnelEnd.position;
            rot = lastSection.tunnelEnd.rotation;
        }

        sectionObj.name = "Section_" + sectionIndex;
        //section = sectionObj.GetComponent<Tunnel>();
        bool withObstacles = sectionIndex > initialSectionsWithoutObstacles && sectionIndex % sectionsBetweenCurves != 1;
        sectionObj.Initialize(pos, rot, withObstacles ? RandomObstacle() : null);
        sections.Enqueue(sectionObj);
        lastSection = sectionObj;
        sectionIndex++;
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;
        if (!checkRemove) return;
        //Debug.Log(currentSection.gameObject.name);
        if(currentSection.tunnelEnd.InverseTransformPoint(player.position).z > 0f)
        {
            RemoveFirstSection();
        }
    }

    private void RemoveFirstSection()
    {
        var first = sections.Dequeue();
        //first.Place(lastSection.TunnelEnd.position, lastSection.TunnelEnd.rotation);
        sectionsToRemove.Enqueue(first);
        if(sectionsToRemove.Count > delaySectionReplacement)
        {
            var sectionToRemove = sectionsToRemove.Dequeue();
            //sectionToRemove.obstacle
            RemoveSection(sectionToRemove);
            
            AddSection();
        }
    }

    private void RemoveSection(Tunnel sectionToRemove)
    {
        if (sectionToRemove.obstacle != null)
            obstaclePool.Release(sectionToRemove.obstacle);

        if (sectionToRemove.IsCurve())
            curvePool.Release(sectionToRemove);
        else if (sectionToRemove.sectionIndex >= 0)
        {
            tunnelPools[sectionToRemove.sectionIndex].Release(sectionToRemove);
            //Destroy(sectionToRemove.gameObject);
        }
        else
        {
            Destroy(sectionToRemove.gameObject);
        }
    }

    /*private Tunnel RandomTunnel(Tunnel[] sectionList)
    {
        int rnd = Random.Range(0, sectionList.Length);
        return sectionList[rnd];
    }*/

    private Tunnel RandomCurve()
    {
        return curvePool.GetAtRandom();
    }

    private Obstacle RandomObstacle()
    {
        //int rnd = Random.Range(0, obstaclePrefabs.Length);
        //return Instantiate(obstaclePrefabs[rnd]).GetComponent<Obstacle>();
        //return obstaclePools[rnd].Get();
        return obstaclePool.GetAtRandom();
    }
}

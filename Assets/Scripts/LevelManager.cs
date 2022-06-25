using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    Transform player;

    [SerializeField] GameObject[] sectionPrefabs;
    [SerializeField] int numOfSections = 3;
    [SerializeField] int initialSectionWithoutObstacles = 3;
    [SerializeField] int delaySectionReplacement = 1;
    Queue<TunnelSection> sectionsToReplace;
    //[SerializeField] float distanceToReplaceLast = 60f;
    Queue<TunnelSection> sections;
    TunnelSection lastSection;

    public TunnelSection currentSection => sections.Peek();

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>().transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        sections = new Queue<TunnelSection>();
        sectionsToReplace = new Queue<TunnelSection>();
        for (int i = 0; i < transform.childCount; i++)
            Destroy(transform.GetChild(i).gameObject);
        for(int i = 0; i < numOfSections; i++)
        {
            var section = Instantiate(RandomSection(), transform);
            section.name = section.name + i;
            var tunnelSection = section.GetComponent<TunnelSection>();
            if(i == 0)
            {
                tunnelSection.Place(transform.position, transform.rotation, i >= initialSectionWithoutObstacles);
            }
            else
            {
                tunnelSection.Place(lastSection.TunnelEnd.position, lastSection.TunnelEnd.rotation, i >= initialSectionWithoutObstacles);
            }
            
            sections.Enqueue(tunnelSection);
            lastSection = tunnelSection;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(currentSection.gameObject.name);
        if(currentSection.TunnelEnd.InverseTransformPoint(player.position).z > 0f)
        {
            ReplaceFirstToLast();
        }
    }

    private void ReplaceFirstToLast()
    {
        var first = sections.Dequeue();
        //first.Place(lastSection.TunnelEnd.position, lastSection.TunnelEnd.rotation);
        sectionsToReplace.Enqueue(first);
        if(sectionsToReplace.Count > delaySectionReplacement)
        {
            var sectionToReplace = sectionsToReplace.Dequeue();
            sections.Enqueue(sectionToReplace);
            sectionToReplace.Place(lastSection.TunnelEnd.position, lastSection.TunnelEnd.rotation);
            lastSection = sectionToReplace;
        }
        //lastSection = first;
        //sections.Enqueue(first);
    }

    private GameObject RandomSection()
    {
        int rnd = Random.Range(0, sectionPrefabs.Length);
        return sectionPrefabs[rnd];
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour
{
    private const Single SKY_Y = 0.61f;
    private const Single UNDERGOUND_Y = -2.87f;

    private List<Single> _activeLevels = new List<Single>();

    public Transform trMapParent;

    public GameObject objGroundPrefab;
    public GameObject objSkyPrefab;
    public GameObject objCablePrefab;
    public PowerableActivationNode objPowerableActivationNodePrefab;
    public List<PowerableObject> _powerableObjects = new List<PowerableObject>();

    private Single _lastGroundX = 0;
    private Single _lastSkyX = 0;
    private Single _thisSegmentCableX = -8.19f;

    [HideInInspector] public List<GameObject> GeneratedUnderGrounds = new List<GameObject>();
    [HideInInspector] public List<GameObject> GeneratedSkies = new List<GameObject>();
    [HideInInspector] public List<GameObject> GeneratedCables = new List<GameObject>();
    [HideInInspector] public List<GameObject> GeneratedPowerables = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        _activeLevels = new List<float>()
        {
            -0.5f,
            -2f,
            -3.5f,
        };

        StartCoroutine(MapGeneratorCor());
    }


    private Int32 _generationsCount = 0;
    private IEnumerator MapGeneratorCor()
    {
        _generationsCount++;

        Single thisGenerationSegmentStartXBefore = _lastGroundX;
        Single totalUndergroundSegmentWidth = GenerateUndergrounds();
        thisGenerationSegmentStartXBefore -= (totalUndergroundSegmentWidth / 2f);
        GenerateSkyBackgrounds();
        GenerateCables();
        GeneratePowerables(thisGenerationSegmentStartXBefore, totalUndergroundSegmentWidth);

        yield return new WaitForSeconds(_generationsCount > 3 ? 2.5f : 0f);

        StartCoroutine(MapGeneratorCor());
    }

    private void GenerateSkyBackgrounds()
    {
        GameObject newSky = Instantiate(objSkyPrefab, trMapParent);
        newSky.transform.localPosition = new Vector3(_lastSkyX, SKY_Y, 0);
        Single totalSegmentWidth = newSky.GetComponent<RectTransform>().sizeDelta.x;
        _lastSkyX += totalSegmentWidth;

        GeneratedSkies.Add(newSky);
    }

    private void GeneratePowerables(Single beforeStartX, Single totalSegmentWidth)
    {
        Int32 powerablesInThisSegment = Random.Range(1, 4);

        for (Int32 i = 0; i < powerablesInThisSegment; i++)
        {
            PowerableObject powerable = Instantiate(_powerableObjects[0], trMapParent);
            Single thisPowerableX = beforeStartX + ((totalSegmentWidth / (powerablesInThisSegment + 1)) * (i + 1));
            powerable.transform.localPosition = new Vector3(thisPowerableX,
                powerable.PositionY, 0f);

            PowerableActivationNode[] nodes = new PowerableActivationNode[1];
            for(Int32 n = 0; n < 1; n++)
            {
                PowerableActivationNode newNode = Instantiate(objPowerableActivationNodePrefab, trMapParent);
                newNode.transform.localPosition = new Vector3(thisPowerableX, _activeLevels[Random.Range(0, _activeLevels.Count)]);
                nodes[n] = newNode;
            }

            powerable.AttachPowerableActivationNodes(nodes);

            GeneratedPowerables.Add(powerable.gameObject);
        }
    }

    private void GenerateCables()
    {
        Int32 cableSegmentsInBacgkround = 35;
        Single cableWidthTakingScale = objCablePrefab.GetComponent<RectTransform>().sizeDelta.x * objCablePrefab.transform.localScale.x;
        for (Int32 i = 0; i < cableSegmentsInBacgkround; i++)
        {
            foreach (Single level in _activeLevels)
            {
                GameObject cable = Instantiate(objCablePrefab, trMapParent);
                cable.transform.localPosition = new Vector3(_thisSegmentCableX + i * cableWidthTakingScale, level, 0);
                GeneratedCables.Add(cable);
            }
        }

        Single totalCablesWidthThisSegment = cableSegmentsInBacgkround * cableWidthTakingScale;
        _thisSegmentCableX += totalCablesWidthThisSegment;
    }

    private Single GenerateUndergrounds()
    {
        GameObject newUnderGround = Instantiate(objGroundPrefab, trMapParent);
        newUnderGround.transform.localPosition = new Vector3(_lastGroundX, UNDERGOUND_Y, 0);
        Single totalSegmentWidth = newUnderGround.GetComponent<RectTransform>().sizeDelta.x;
        _lastGroundX += totalSegmentWidth;

        GeneratedUnderGrounds.Add(newUnderGround);

        return totalSegmentWidth;
    }

    // Update is called once per frame
    void Update()
    {

    }
}

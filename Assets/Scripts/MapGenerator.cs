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
    public Transform trCables;

    public GameObject objGroundPrefab;
    public GameObject objSkyPrefab;
    public GameObject objCablePrefab;
    public PowerableActivationNode objPowerableActivationNodePrefab;
    public List<PowerableObject> _powerableObjects = new List<PowerableObject>();
    public List<GameObject> _things = new List<GameObject>();

    private Single _lastGroundX = 0;
    private Single _lastSkyX = 0;
    private Single _thisSegmentCableX = -8.19f;

    [HideInInspector] public List<GameObject> GeneratedUnderGrounds = new List<GameObject>();
    [HideInInspector] public List<GameObject> GeneratedSkies = new List<GameObject>();
    [HideInInspector] public List<GameObject> GeneratedCables = new List<GameObject>();
    [HideInInspector] public List<GameObject> GeneratedPowerables = new List<GameObject>();

    Single oneUndergroundBackgroundWidth;
    Single _nextPowerableX = 20f;
    Single _nextThingX = 24f;

    // Start is called before the first frame update
    void Start()
    {
        _nextPowerableX = 20f;
        oneUndergroundBackgroundWidth = objGroundPrefab.GetComponent<RectTransform>().sizeDelta.x;

        _activeLevels = new List<float>()
        {
            -0.5f,
            -2f,
            -3.5f,
        };

        StartCoroutine(MapGeneratorCor());
        StartCoroutine(FeatureGeneration());
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

        yield return new WaitForSeconds(_generationsCount > 3 ? 2.5f : 0f);

        StartCoroutine(MapGeneratorCor());
        StartCoroutine(FeatureGeneration());
    }

    private IEnumerator FeatureGeneration()
    {
        if(_nextPowerableX < _lastGroundX)
        {
            SpawnPowerable();
            _nextPowerableX += Random.Range(6f, 12f);
            _nextThingX = _nextPowerableX + Random.Range(1.5f, 3f) * (Random.value > 0.5f ? 1f : -1f);
        }

        if (_nextThingX < _lastGroundX)
        {
            SpawnThing();
            _nextThingX = Single.MaxValue;
        }

        yield return new WaitForSeconds(_generationsCount > 3 ? 0.1f : 0f);

        StartCoroutine(FeatureGeneration());
    }

    private void SpawnThing()
    {
        GameObject thing = Instantiate(_things[Random.Range(0, _things.Count)], trMapParent);
        thing.transform.localPosition = new Vector3(_nextThingX,
            PickRandomActiveLevel(), 0f);
    }

    private void GenerateSkyBackgrounds()
    {
        GameObject newSky = Instantiate(objSkyPrefab, trMapParent);
        newSky.transform.localPosition = new Vector3(_lastSkyX, SKY_Y, 0);
        Single totalSegmentWidth = newSky.GetComponent<RectTransform>().sizeDelta.x;
        _lastSkyX += totalSegmentWidth;

        GeneratedSkies.Add(newSky);
    }

    private void SpawnPowerable()
    {
        PowerableObject powerable = Instantiate(_powerableObjects[0], trMapParent);
        powerable.transform.localPosition = new Vector3(_nextPowerableX,
            powerable.PositionY, 0f);

        PowerableActivationNode[] nodes = new PowerableActivationNode[1];
        for (Int32 n = 0; n < 1; n++)
        {
            PowerableActivationNode newNode = Instantiate(objPowerableActivationNodePrefab, trMapParent);
            newNode.transform.localPosition = new Vector3(_nextPowerableX, PickRandomActiveLevel());
            nodes[n] = newNode;
        }

        powerable.AttachPowerableActivationNodes(nodes);

        GeneratedPowerables.Add(powerable.gameObject);
    }

    private void GenerateCables()
    {
        Int32 cableSegmentsInBacgkround = 35;
        Single cableWidthTakingScale = objCablePrefab.GetComponent<RectTransform>().sizeDelta.x * objCablePrefab.transform.localScale.x;
        for (Int32 i = 0; i < cableSegmentsInBacgkround; i++)
        {
            foreach (Single level in _activeLevels)
            {
                GameObject cable = Instantiate(objCablePrefab, trCables);
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

    private Single PickRandomActiveLevel()
    {
        return _activeLevels[Random.Range(0, _activeLevels.Count)];
    }
}

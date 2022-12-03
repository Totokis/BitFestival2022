using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour
{
    private const Single SKY_Y = 0.61f;
    private const Single BACK_CITY_Y = 2.39f;
    private const Single UNDERGOUND_Y = -2.87f;

    private List<Single> _activeLevels = new List<Single>();

    public Transform trMapParent;
    public Transform trCables;
    public Transform trFrontCity;

    public PlayerController PlayerController;

    public GameObject objGroundPrefab;
    public GameObject objSkyPrefab;
    public GameObject objBackCityPrefab;
    public GameObject objCablePrefab;
    public GameObject objCurvedCable;
    public PowerableActivationNode objPowerableActivationNodePrefab;
    public List<PowerableObject> _powerableObjects = new List<PowerableObject>();
    public List<GameObject> _things = new List<GameObject>();
    public List<GameObject> _frontCityBuildings = new List<GameObject>();

    private Single _lastGroundX = 0;
    private Single _lastSkyX = 0;
    private Single _lastBackCityX = 0;
    private Single _thisSegmentCableX = -8.19f;

    [HideInInspector] public List<GameObject> GeneratedUnderGrounds = new List<GameObject>();
    [HideInInspector] public List<GameObject> GeneratedSkies = new List<GameObject>();
    [HideInInspector] public List<GameObject> GeneratedCables = new List<GameObject>();
    [HideInInspector] public List<GameObject> GeneratedPowerables = new List<GameObject>();

    Single oneUndergroundBackgroundWidth;
    Single _nextPowerableX = 20f;
    Single _nextFrontBuildingX = -20f;

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
        //GenerateSkyBackgrounds();
        GenerateCables();
        GenerateBackCity();

        yield return new WaitForSeconds(_generationsCount > 3 ? 12.5f / PlayerController.speed : 0f);
        //yield return new WaitForSeconds(0.1f);

        StartCoroutine(MapGeneratorCor());
        StartCoroutine(FeatureGeneration());
    }

    private IEnumerator FeatureGeneration()
    {
        if (_nextPowerableX < _lastGroundX)
        {
            SpawnPowerable();
            _nextPowerableX += Random.Range(4.5f, 8.6f);
            Single thingX = _nextPowerableX + Random.Range(1.1f, 4f) * (Random.value > 0.5f ? 1f : -1f);
            SpawnThing(thingX);
        }

        if (_nextFrontBuildingX < _lastGroundX)
        {
            //2.631f
            GameObject frontBuilding = Instantiate(_frontCityBuildings[Random.Range(0, _frontCityBuildings.Count)], trFrontCity);
            frontBuilding.transform.localPosition = new Vector3(_nextFrontBuildingX, 2.631f);
            _nextFrontBuildingX += Random.Range(1f, 4f);
        }

        yield return new WaitForSeconds(_generationsCount > 3 ? 0.1f : 0f);

        StartCoroutine(FeatureGeneration());
    }

    private void SpawnThing(Single x)
    {
        GameObject thing = Instantiate(_things[Random.Range(0, _things.Count)], trMapParent);
        thing.transform.localPosition = new Vector3(x,
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

    Single spawnedBCS = 0f;
    private void GenerateBackCity()
    {
        GameObject newbackcity = Instantiate(objBackCityPrefab, trMapParent);
        newbackcity.transform.localPosition = new Vector3(_lastBackCityX, BACK_CITY_Y, 0);
        Single totalSegmentWidth = newbackcity.GetComponent<RectTransform>().sizeDelta.x;
        newbackcity.GetComponent<Parallax>().offset = totalSegmentWidth * spawnedBCS;
        _lastBackCityX += totalSegmentWidth;
        spawnedBCS++;
    }

    private void SpawnPowerable()
    {
        PowerableObject powerable = Instantiate(_powerableObjects[0], trMapParent);
        powerable.transform.localPosition = new Vector3(_nextPowerableX,
            powerable.PositionY, 0f);

        Single curvedCableHeight = 0.1398601f;

        Int32 powerableNodesCount = Random.Range(1, 3);
        PowerableActivationNode[] nodes = new PowerableActivationNode[powerableNodesCount];
        Vector3 curvedCableStart = new Vector3(powerable.transform.localPosition.x, 0.84f);
        List<GameObject> thisCables = new List<GameObject>();
        Single leftRightOffset = 2f;

        if (powerableNodesCount == 1)
        {

            PowerableActivationNode newNode = Instantiate(objPowerableActivationNodePrefab, trMapParent);
            StartCoroutine(Namer(powerable, newNode));
            Single level = PickRandomActiveLevel();
            newNode.transform.localPosition = new Vector3(_nextPowerableX, level);

            Single cableToGen = Mathf.Abs(newNode.transform.localPosition.y - powerable.transform.localPosition.y + (powerable.GetComponent<RectTransform>().sizeDelta.y / 2f));
            for (Single cableGenerated = 0f; cableGenerated < cableToGen; cableGenerated += curvedCableHeight)
            {
                GameObject cableNew = Instantiate(objCurvedCable, trCables);
                cableNew.transform.localPosition = curvedCableStart;
                curvedCableStart = new Vector3(curvedCableStart.x, curvedCableStart.y - curvedCableHeight);
                thisCables.Add(cableNew);
            }

            nodes[0] = newNode;
        }
        else
        {
            Int32 leftOffset = Random.Range(-1, 1);
            Int32 rightOffset = 1;
            if (leftOffset == -1)
                rightOffset = leftOffset + Random.Range(1, 3);
            else if (leftOffset == 0)
                rightOffset = leftOffset + 1;

            Single leftLevel = PickRandomActiveLevel();
            Single rightLevel = PickRandomActiveLevel();

            //leftLevel = -3.5f;
            //rightLevel = -3.5f;

            PowerableActivationNode leftNode = Instantiate(objPowerableActivationNodePrefab, trMapParent);
            StartCoroutine(Namer(powerable, leftNode));
            leftNode.transform.localPosition = new Vector3(_nextPowerableX + (leftOffset * leftRightOffset), leftLevel);

            PowerableActivationNode rightNode = Instantiate(objPowerableActivationNodePrefab, trMapParent);
            StartCoroutine(Namer(powerable, rightNode));
            rightNode.transform.localPosition = new Vector3(_nextPowerableX + (rightOffset * leftRightOffset), rightLevel);

            Single higherCableLevel = MathF.Max(leftLevel, rightLevel);
            Single cableSplitLevel = Mathf.Clamp(higherCableLevel + ((Single)(Random.Range(0, 3)) * 1.5f), -3.5f, -0.5f)
                 + 0.75f;//bo troche nad poziomem jest split

            Single cableToGen = Mathf.Abs((cableSplitLevel) - powerable.transform.localPosition.y + (powerable.GetComponent<RectTransform>().sizeDelta.y / 2f));
            for (Single cableGenerated = 0f; cableGenerated < cableToGen; cableGenerated += curvedCableHeight)
            {
                GameObject cableNew = Instantiate(objCurvedCable, trCables);
                cableNew.transform.localPosition = curvedCableStart;
                curvedCableStart = new Vector3(curvedCableStart.x, curvedCableStart.y - curvedCableHeight);
                thisCables.Add(cableNew);
            }

            {//left cable
                //do boku jak istnieje
                if (leftOffset != 0)//ze split pointa do boku
                {
                    cableToGen = leftRightOffset;
                    curvedCableStart = new Vector3(leftRightOffset * leftOffset + powerable.transform.localPosition.x, cableSplitLevel);
                    for (Single cableGenerated = 0f; cableGenerated < cableToGen; cableGenerated += curvedCableHeight)
                    {
                        GameObject cableNew = Instantiate(objCurvedCable, trCables);
                        cableNew.transform.localPosition = curvedCableStart;
                        curvedCableStart = new Vector3(curvedCableStart.x + curvedCableHeight, curvedCableStart.y);
                        thisCables.Add(cableNew);
                    }
                }

                //from split level to node do do³u
                Single undergroundCableStart = cableSplitLevel;
                cableToGen = Mathf.Abs(cableSplitLevel - leftNode.transform.localPosition.y);
                curvedCableStart = new Vector3(powerable.transform.localPosition.x + (leftOffset * leftRightOffset), undergroundCableStart);
                for (Single cableGenerated = 0f; cableGenerated < cableToGen; cableGenerated += curvedCableHeight)
                {
                    GameObject cableNew = Instantiate(objCurvedCable, trCables);
                    cableNew.transform.localPosition = curvedCableStart;
                    curvedCableStart = new Vector3(curvedCableStart.x, curvedCableStart.y - curvedCableHeight);
                    thisCables.Add(cableNew);
                }
            }

            {//right cable
                //do boku jak istnieje
                if (rightOffset != 0)//ze split pointa do boku
                {
                    cableToGen = leftRightOffset;
                    curvedCableStart = new Vector3(leftRightOffset * rightOffset + powerable.transform.localPosition.x, cableSplitLevel);
                    for (Single cableGenerated = 0f; cableGenerated < cableToGen; cableGenerated += curvedCableHeight)
                    {
                        GameObject cableNew = Instantiate(objCurvedCable, trCables);
                        cableNew.transform.localPosition = curvedCableStart;
                        curvedCableStart = new Vector3(curvedCableStart.x - curvedCableHeight, curvedCableStart.y);
                        thisCables.Add(cableNew);
                    }
                }

                //from split level to node do do³u
                Single undergroundCableStart = cableSplitLevel;
                cableToGen = Mathf.Abs(cableSplitLevel - rightNode.transform.localPosition.y);
                curvedCableStart = new Vector3(powerable.transform.localPosition.x + (rightOffset * leftRightOffset), undergroundCableStart);
                for (Single cableGenerated = 0f; cableGenerated < cableToGen; cableGenerated += curvedCableHeight)
                {
                    GameObject cableNew = Instantiate(objCurvedCable, trCables);
                    cableNew.transform.localPosition = curvedCableStart;
                    curvedCableStart = new Vector3(curvedCableStart.x, curvedCableStart.y - curvedCableHeight);
                    thisCables.Add(cableNew);
                }
            }

            nodes[0] = leftNode;
            nodes[1] = rightNode;
        }

        powerable.AttachCables(thisCables);

        powerable.AttachPowerableActivationNodes(nodes);

        GeneratedPowerables.Add(powerable.gameObject);
    }

    private IEnumerator Namer(PowerableObject powerable, PowerableActivationNode newNode)
    {
        yield return new WaitForSeconds(1f);
        newNode.name = $"Activator for {powerable.name}";
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

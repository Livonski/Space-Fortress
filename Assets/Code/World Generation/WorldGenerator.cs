using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    [SerializeField] Grid mainGrid;

    [SerializeField] private Vector2Int gridSize;

    [SerializeField] private float noisePersistency;
    [SerializeField] private float noiseScale;
    [SerializeField] private int worldSeed;
    [SerializeField] private int octaves;

    [SerializeField] private Vector2Int shapesAmount;
    [SerializeField] private int minimalShapeRadius;
    [SerializeField] private Texture2D asteroidTexture;

    [SerializeField] private AnimationCurve tilesProbability;



    [SerializeField] private GameObject[] possibleTiles;
    [SerializeField] private Sprite playerSprite;

    [SerializeField] private Vector2Int playerPosition;
    [SerializeField] private float playerSpeed;

    [SerializeField] private InputHandler playerInput;

    private Entity player;

    private void Awake()
    {
        mainGrid.CreateGrid(gridSize);
        if (worldSeed == 0)
            worldSeed = Random.Range(1, 999999);
    }

    private void Start()
    {
        CreateWorld();
        
    }

    //==================================================
    // снести нахуй
    private void FillWorld()
    {
        Noise perlisNoise = new Noise(noisePersistency,worldSeed);
        for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                float xCoord = (float)x / (float)gridSize.x;
                float yCoord = (float)y / (float)gridSize.y;


                float perlinNoiseValue = perlisNoise.SampleLayeredNoise(xCoord,yCoord, noiseScale, octaves) + asteroidTexture.GetPixel(x,y).r;
                int TileID = (int)tilesProbability.Evaluate(perlinNoiseValue);

                GameObject newTile = possibleTiles[TileID];
                mainGrid.AddObjectToGrid(x, y, newTile);
            }
        }
    }
    //==================================================
    private void CreateWorld()
    {
        GenerateAsteroidShape();
        FillWorld();
    }
    private void AddPlayer()
    {
        player = new Entity(playerSprite);
        mainGrid.AddEntity(player, playerPosition);
        playerInput.SetupInputHandler(player, playerPosition, playerSpeed);
    }


    private void GenerateAsteroidShape()
    {
        AsteroidShapeGenerator shapeGenerator = new AsteroidShapeGenerator(gridSize.y, gridSize.x);
        int shapes = Random.Range(shapesAmount.x, shapesAmount.y);
        asteroidTexture = shapeGenerator.GenerateTexture(shapes, minimalShapeRadius);
    }

    private void GenerateCaves()
    {

    }

    private void GenerateResources()
    {

    }


}

public class AsteroidShapeGenerator
{
    private int _textureWidth;
    private int _textureHeight;

    public AsteroidShapeGenerator(int textureWidth, int textureHeight)
    {
        _textureWidth = textureWidth;
        _textureHeight = textureHeight;
    }

    public Texture2D GenerateTexture(int shapesAmount, int minimalShapeRadius)
    {
        Vector2Int[] shapeCentres = new Vector2Int[shapesAmount];
        int[] shapeRadiuses = new int[shapesAmount];
        Texture2D texture = new Texture2D(_textureWidth,_textureHeight, TextureFormat.ARGB32, false);
        for (int i = 0; i < shapesAmount; i++)
        {
            shapeCentres[i] = new Vector2Int(Random.Range(0, _textureWidth), Random.Range(0, _textureHeight));
            shapeRadiuses[i] = Random.Range(minimalShapeRadius, _textureWidth / 2);
            Debug.Log("Generating shape " + i + " at " + shapeCentres[i].x + ":" + shapeCentres[i].y + " radius:" + shapeRadiuses[i]);
        }

        AddShapes(texture, shapeCentres, shapeRadiuses);

        return texture;
    }

    private void AddShapes(Texture2D texture, Vector2Int[] shapeCentres, int[] shapeRadiuses)
    {
        for (int x = 0; x < _textureWidth; x++)
        {
            for (int y = 0; y < _textureHeight; y++)
            {
                List<int> ShapeIDs = isWithinRadius(x, y, shapeCentres, shapeRadiuses);

                if (ShapeIDs.Count > 0)
                {
                    Color pointColor = CalculatePointColor(ShapeIDs, shapeCentres, shapeRadiuses, x, y);
                    texture.SetPixel(x, y, pointColor);
                 }
                else
                {
                    texture.SetPixel(x, y, Color.black);
                }
            }
        }
    }

    private Color CalculatePointColor(List<int> ShapeIDs, Vector2Int[] shapeCentres, int[] shapeRadiuses, int x, int y)
    {
        float relativeDistantion = 0;
        if (ShapeIDs.Count == 0)
            return new Color(0, 0, 0, 0);

        foreach (int shapeID in ShapeIDs)
        {
            relativeDistantion += ((float)shapeRadiuses[shapeID] - CalculateHypotenuze(x, y, shapeCentres[shapeID])) / (float)shapeRadiuses[shapeID];
        }

        relativeDistantion /= ShapeIDs.Count;

        Color pointColor = new Color(relativeDistantion, relativeDistantion, relativeDistantion, 1);
        return pointColor;
    }

    private float CalculateHypotenuze(int x, int y, Vector2Int shapeCenter)
    {
        float localX = (float)shapeCenter.x - (float)x;
        float localY = (float)shapeCenter.y - (float)y;
        float hypotenuze = Mathf.Sqrt(localX * localX + localY * localY);

        return hypotenuze;
    }
    private List<int> isWithinRadius(int x, int y, Vector2Int[] shapeCentres, int[] shapeRadiuses)
    {

        List<int> results = new List<int>();

        for (int i = 0; i < shapeCentres.Length; i++)
        {
            if (CalculateHypotenuze(x, y, shapeCentres[i]) < (float)shapeRadiuses[i])
                results.Add(i);    
        }
        return results;
    }
}

public class Noise
{
    private float _persistance;
    private float _seed;

    public Noise (float persistance, float seed)
    {
        _persistance = persistance;
        _seed = seed;
    }

    public float SampleNoise(float x, float y, float scale)
    {
        x = _seed + x * scale;
        y = _seed + y * scale;

        float NoiseSample = Mathf.PerlinNoise(x, y);

        return NoiseSample;
    }

    public float SampleLayeredNoise(float x, float y, float scale, int octaves)
    {
        float noiseSample = 0;
        for (int i = 1; i <= octaves; i++)
        {
            noiseSample += SampleNoise(x, y, scale) * _persistance;
            scale *= 0.2f;
        }
        return noiseSample;
    }
}
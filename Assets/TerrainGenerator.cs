﻿using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public int width = 512;
    public int height = 512;
    public int depth = 20;
    public float scale = 20f;

    public Terrain terrain;

    void Start()
    {
        GenerateTerrain();
    }

    public void GenerateTerrain()
    {
        terrain.terrainData = GenerateTerrainData(terrain.terrainData);
    }

    TerrainData GenerateTerrainData(TerrainData terrainData)
    {
        terrainData.heightmapResolution = width + 1;
        terrainData.size = new Vector3(width, depth, height);
        terrainData.SetHeights(0, 0, GenerateHeights());
        return terrainData;
    }

    float[,] GenerateHeights()
    {
        float[,] heights = new float[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                heights[x, y] = depth;// CalculateHeight(x, y);
            }
        }
        return heights;
    }

    float CalculateHeight(int x, int y)
    {
        float xCoord = (float)x / width * scale;
        float yCoord = (float)y / height * scale;

        return Mathf.PerlinNoise(xCoord, yCoord);
    }
}

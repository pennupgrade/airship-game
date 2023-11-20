using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountainNoise : MonoBehaviour
{
    private float[,,] m_gradients = new float[100, 100, 2];

    // Starting location in x,y (random between 0 and 100)
    private float m_startX = 0.0f;
    private float m_startY = 0.0f;

    // Destination location
    private float m_endX = 0.0f;
    private float m_endY = 0.0f;

    // An array of size 30 of points representing a cross section of the noise function
    private float[] m_crossSection = new float[30];

    // Game object to represent the point
    [SerializeField]
    public GameObject pointRef;

    private GameObject[] m_points = new GameObject[30];
    
    // Applies interpolation to the noise
    private float fade(float t)
    {
        return t * t * t * (t * (t * 6.0f - 15.0f) + 10.0f);
    }

    // Applies the gradient to the noise
    private float lerp(float t, float a, float b)
    {
        return a + t * (b - a);
    }

    // Generates the noise
    public float generateNoise(float x, float y)
    {
        int x0 = (int)Mathf.Floor(x);
        int x1 = x0 + 1;
        int y0 = (int)Mathf.Floor(y);
        int y1 = y0 + 1;

        float sx = x - x0;
        float sy = y - y0;

        float n0, n1, ix0, ix1, value;

        n0 = dotGridGradient(x0, y0, x, y);
        n1 = dotGridGradient(x1, y0, x, y);
        ix0 = lerp(fade(sx), n0, n1);

        n0 = dotGridGradient(x0, y1, x, y);
        n1 = dotGridGradient(x1, y1, x, y);
        ix1 = lerp(fade(sx), n0, n1);

        value = lerp(fade(sy), ix0, ix1);

        return value;
    }

    // Generates the gradient
    private float dotGridGradient(int ix, int iy, float x, float y)
    {
        float dx = x - ix;
        float dy = y - iy;

        float gradient = dx * m_gradients[iy, ix, 0] + dy * m_gradients[iy, ix, 1];

        return gradient;
    }

    // Generates the gradients
    private void generateGradients()
    {
        for (int y = 0; y < m_gradients.GetLength(0); y++)
        {
            for (int x = 0; x < m_gradients.GetLength(1); x++)
            {
                float randomAngle = Random.Range(0.0f, 2.0f * Mathf.PI);
                m_gradients[y, x, 0] = Mathf.Cos(randomAngle);
                m_gradients[y, x, 1] = Mathf.Sin(randomAngle);
            }
        }
    }

    // Generates the noise map
    public float[,] generateNoiseMap(int width, int height, float scale)
    {
        float[,] noiseMap = new float[width, height];

        generateGradients();

        if (scale <= 0)
        {
            scale = 0.0001f;
        }

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float sampleX = x / scale;
                float sampleY = y / scale;

                float noiseHeight = generateNoise(sampleX, sampleY);

                if (noiseHeight > maxNoiseHeight)
                {
                    maxNoiseHeight = noiseHeight;
                }
                else if (noiseHeight < minNoiseHeight)
                {
                    minNoiseHeight = noiseHeight;
                }

                noiseMap[x, y] = noiseHeight;
            }
        }

        // Normalizes the noise map
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
            }
        }

        return noiseMap;
    }

    void UpdateCrossSection(float direction)
    {
        float x = m_endX - m_startX;
        float y = m_endY - m_startY;

        float newX = x * Mathf.Cos(direction * Mathf.Deg2Rad) - y * Mathf.Sin(1.0f * Mathf.Deg2Rad);
        float newY = x * Mathf.Sin(direction * Mathf.Deg2Rad) + y * Mathf.Cos(1.0f * Mathf.Deg2Rad);

        m_endX = newX + m_startX;
        m_endY = newY + m_startY;

        // Recompute the cross section
        for (int i = 0; i < m_crossSection.Length; i++)
        {
            // linearly interpolate between the points
            x = Mathf.Lerp(m_startX, m_endX, (float)i / (float)m_crossSection.Length);
            y = Mathf.Lerp(m_startY, m_endY, (float)i / (float)m_crossSection.Length);

            m_crossSection[i] = generateNoise(x, y);
        }
    }

    // A function that renders the m_crossSection points to the screen as a series of points
    // Where the y value it appears on the screen is the value of the noise function
    // And X is from 0 to 30
    void RenderCrossSection()
    {
        for (int i = 0; i < m_crossSection.Length; i++)
        {
            // Move the location of the circle in m_crossSection[i] to the new location
            // (i, m_crossSection[i])
            m_points[i].transform.position = new Vector3(i, m_crossSection[i], 0);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Set the start location
        m_startX = Random.Range(0.0f, 100.0f);
        m_startY = Random.Range(0.0f, 100.0f);

        // Set the destination location
        m_endX = Random.Range(0.0f, 100.0f);
        m_endY = Random.Range(0.0f, 100.0f);

        // From start point to end point, generate the noise along the line and store it in the cross section array
        for (int i = 0; i < m_crossSection.Length; i++)
        {
            // linearly interpolate between the points
            float x = Mathf.Lerp(m_startX, m_endX, (float)i / (float)m_crossSection.Length);
            float y = Mathf.Lerp(m_startY, m_endY, (float)i / (float)m_crossSection.Length);

            m_crossSection[i] = generateNoise(x, y);
        }

        // Generate 30 circles to represent the points in m_crossSection
        // Set their Y value to the value in m_crossSection and X to be from 0 to 30 with spaces between
        for (int i = 0; i < m_crossSection.Length; i++)
        {
            // Create a 2d circle sprite of radius 5 pixels at the point (i, m_crossSection[i]) in screen space
            GameObject point = GameObject.Instantiate(pointRef, new Vector3(i, m_crossSection[i], 0), Quaternion.identity);
            m_points[i] = point;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Get the keyboard input for left and right arrow keys
        float horizontal = Input.GetAxis("Horizontal");
        if (horizontal != 0)
        {
            UpdateCrossSection(horizontal);
            RenderCrossSection();
        }
    }
}
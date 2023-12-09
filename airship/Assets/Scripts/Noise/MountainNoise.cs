using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountainNoise : MonoBehaviour
{
    private float[,,] m_gradients = new float[100, 100, 2];
    private float[,] m_noiseMap = new float[100, 100];

    // Starting location in x,y (random between 0 and 100)
    private float m_startX = 0.0f;
    private float m_startY = 0.0f;

    // Destination location
    private float m_endX = 0.0f;
    private float m_endY = 0.0f;

    // DEBUG noise function values
    private float m_amplitude = 5.0f;
    private float m_frequency = 0.1f;
    private float m_baselineHeight = -3.0f;
    private float m_distBtwnPoints = 0.6f;

    // An array of size 30 of points representing a cross section of the noise function
    private float[] m_crossSectionHeights = new float[35];

    // Game object to represent the point
    [SerializeField]
    public GameObject pointRef;

    [SerializeField]
    public GameObject lineColliderRef;

    // Keep handles to these game objects so we can modify their locations
    private GameObject[] m_pointObjs = new GameObject[35];
    private Vector2[] m_pointVecs = new Vector2[35];
    private EdgeCollider2D m_lineCollider;
    
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

    // Generates the gradient
    private float dotGridGradient(int ix, int iy, float x, float y)
    {
        float dx = x - ix;
        float dy = y - iy;

        // DEBUG LOG ix and iyi
        Debug.Log("ix: " + ix + " iy: " + iy);
        float gradient = dx * m_gradients[iy, ix, 0] + dy * m_gradients[iy, ix, 1];

        return gradient;
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
    public float[,] generateNoiseMap(int width, int height)
    {
        float[,] noiseMap = new float[width, height];

        generateGradients();

        if (m_frequency <= 0)
        {
            m_frequency = 0.0001f;
        }

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float sampleX = x * m_frequency;
                float sampleY = y * m_frequency;

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
        float scaleFactor = 0.1f;

        float x = m_endX - m_startX;
        float y = m_endY - m_startY;

        float newX = x * Mathf.Cos(direction * scaleFactor * Mathf.Deg2Rad) - y * Mathf.Sin(1.0f * scaleFactor * Mathf.Deg2Rad);
        float newY = x * Mathf.Sin(direction * scaleFactor * Mathf.Deg2Rad) + y * Mathf.Cos(1.0f * scaleFactor * Mathf.Deg2Rad);

        m_endX = newX + m_startX;
        m_endY = newY + m_startY;

        // DEBUG console log
        Debug.Log("Start: (" + m_startX + ", " + m_startY + ")");
        Debug.Log("End: (" + m_endX + ", " + m_endY + ")");

        // Recompute the cross section
        for (int i = 0; i < m_crossSectionHeights.Length; i++)
        {
            // linearly interpolate between the points
            int currx = (int)Mathf.Floor(Mathf.Lerp(m_startX, m_endX, (float)i / (float)m_crossSectionHeights.Length));
            int curry = (int)Mathf.Floor(Mathf.Lerp(m_startY, m_endY, (float)i / (float)m_crossSectionHeights.Length));

            m_crossSectionHeights[i] = m_noiseMap[currx, curry] * m_amplitude + m_baselineHeight;
        }
    }

    // A function that renders the m_crossSection points to the screen as a series of points
    // Where the y value it appears on the screen is the value of the noise function
    // And X is from 0 to 30
    void RenderCrossSection()
    {
        for (int i = 0; i < m_crossSectionHeights.Length; i++)
        {
            // Move the location of the circle in m_crossSection[i] to the new location: (i, m_crossSection[i])
            m_pointObjs[i].transform.position = new Vector3(i * m_distBtwnPoints, m_crossSectionHeights[i], 0);
            m_pointVecs[i] = new Vector2(i * m_distBtwnPoints, m_crossSectionHeights[i]);
        }

        // Update the collider
        m_lineCollider.points = m_pointVecs;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Set the start location
        m_startX = 50.0f;// Random.Range(0.0f, 100.0f);
        m_startY = 50.0f;// Random.Range(0.0f, 100.0f);

        // Set the destination location
        m_endX = Random.Range(0.0f, 100.0f);
        m_endY = Random.Range(0.0f, 100.0f);

        // DEBUG console log
        Debug.Log("Start: (" + m_startX + ", " + m_startY + ")");
        Debug.Log("End: (" + m_endX + ", " + m_endY + ")");

        // Generate the noise map so we can read from it
        m_noiseMap = generateNoiseMap(99, 99);

        // From start point to end point, generate the noise along the line and store it in the cross section array
        for (int i = 0; i < m_crossSectionHeights.Length; i++)
        {
            // linearly interpolate between the points
            int x = (int)Mathf.Floor(Mathf.Lerp(m_startX, m_endX, (float)i / (float)m_crossSectionHeights.Length));
            int y = (int)Mathf.Floor(Mathf.Lerp(m_startY, m_endY, (float)i / (float)m_crossSectionHeights.Length));

            m_crossSectionHeights[i] = m_noiseMap[x,y] * m_amplitude + m_baselineHeight; // generateNoise(x, y);
        }

        // Generate 30 circles to represent the points in m_crossSection
        // Set their Y value to the value in m_crossSection and X to be from 0 to 30 with spaces between
        for (int i = 0; i < m_crossSectionHeights.Length; i++)
        {
            // Create a 2d circle sprite of radius 5 pixels at the point (i, m_crossSection[i]) in screen space
            GameObject point = GameObject.Instantiate(pointRef, new Vector3(i * m_distBtwnPoints, m_crossSectionHeights[i], 0), Quaternion.identity);
            m_pointObjs[i] = point;
            m_pointVecs[i] = new Vector2(i * m_distBtwnPoints, m_crossSectionHeights[i]);
        }
        
        // Create a line collider where each of its points is the point (i, m_crossSection[i])
        GameObject lineCollider = GameObject.Instantiate(lineColliderRef, new Vector3(0, 0, 0), Quaternion.identity);
        m_lineCollider = lineCollider.GetComponent<EdgeCollider2D>();
        if (!m_lineCollider)
        {
            // Throw an error
            Debug.Log("Error: Could not find EdgeCollider2D component on lineColliderRef");
        }
        else
        {
            m_lineCollider.points = m_pointVecs;
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

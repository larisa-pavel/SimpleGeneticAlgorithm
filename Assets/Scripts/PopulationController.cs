using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulationController : MonoBehaviour
{
    List<GeneticPathfinder> population = new List<GeneticPathfinder>();
    public GameObject creaturePrefab;
    public int populationSize = 100;
    public int genomeLenght;
    public int survivorKeep = 5;
    public float cutoff = 0.3f;
    [Range(0f,1f)]
    public float mutationRate = 0.01f;
    public Transform spawnPoint;
    public Transform end;
    public LayerMask obstacleLayer;

    void InitPopulation()
    {
        for(int i = 0; i < populationSize; i++)
        {
            GameObject go = Instantiate(creaturePrefab, spawnPoint.position, Quaternion.identity);
            go.GetComponent<GeneticPathfinder>().InitCreature(new DNA(genomeLenght), end.position);
            population.Add(go.GetComponent<GeneticPathfinder>());
        }
    }
    void NextGeneration()
    {
        int survivorCut = Mathf.RoundToInt(populationSize * cutoff);
        List<GeneticPathfinder> survivors = new List<GeneticPathfinder>();
        for(int i = 0; i < survivorCut; i++)
        {
            survivors.Add(GetFittest());
        }
        for(int i = 0; i < population.Count; i++)
        {
            Destroy(population[i].gameObject);
        }
        population.Clear();

        for (int i = 0; i< survivorKeep;i++)
        {
            GameObject go = Instantiate(creaturePrefab, spawnPoint.position, Quaternion.identity);
            go.GetComponent<GeneticPathfinder>().InitCreature(survivors[i].dna, end.position);
            population.Add(go.GetComponent <GeneticPathfinder>());
        }
        while(population.Count < populationSize)
        {
            for(int i = 0; i < survivors.Count; i++)
            {
                GameObject go = Instantiate(creaturePrefab, spawnPoint.position, Quaternion.identity);
                go.GetComponent<GeneticPathfinder>().InitCreature(new DNA(survivors[i].dna, survivors[Random.Range(0, 10)].dna, mutationRate), end.position);
                population.Add(go.GetComponent<GeneticPathfinder>());
                if(population.Count >= populationSize)
                {
                    break;
                }
            }
        }
        for(int i = 0; i < survivors.Count; i++)
        {
            Destroy(survivors[i].gameObject);
        }
    }
    private void Start()
    {
        Time.timeScale = 5.0f;
        InitPopulation();
    }
    private void Update()
    {
        CheckForResources();
        if (!HasActive())
        {
            NextGeneration();
        }
    }

    void CheckForResources()
    {
        foreach (var agent in population)
        {
            if (Vector2.Distance(agent.transform.position, end.position) < 3f)
            {
                Vector2 newPos = GetValidRandomPosition();
                end.position = newPos;
                break;
            }
        }
    }

    Vector2 GetValidRandomPosition()
    {
        Vector2 potentialPos = Vector2.zero;
        bool isValid = false;
        int attempts = 0;

        while (!isValid && attempts < 100)
        {
            attempts++;
            potentialPos = new Vector2(Random.Range(-18f, 18f), Random.Range(-9f, 9f));
            Collider2D hitObstacle = Physics2D.OverlapCircle(potentialPos, 1f, obstacleLayer);
            float distToSpawn = Vector2.Distance(potentialPos, spawnPoint.position);

            if (hitObstacle == null && distToSpawn > 3f)
            {
                isValid = true;
            }
        }
        return potentialPos;
    }
    GeneticPathfinder GetFittest()
    {
        float maxFitness = float.MinValue;
        int index = 0;
        for(int i = 0; i < population.Count; i++)
        {
            if(population[i].fitness > maxFitness)
            {
                maxFitness = population[i].fitness;
                index = i;
            }
        }
        GeneticPathfinder fittest = population[index];
        population.Remove(fittest);
        return fittest;
    }
    bool HasActive()
    {
        for(int i = 0; i < population.Count; i++)
        {
            if (!population[i].hasFinished)
            {
                return true;
            }
        }
        return false;
    }
}

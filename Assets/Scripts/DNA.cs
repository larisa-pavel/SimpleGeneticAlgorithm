using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNA
{
    public List<float> genes = new List<float>();
    public float speedGene;
    public float sizeGene;
    public DNA(int genomeLenght = 50)
    {
        for(int i = 0; i < genomeLenght; i++)
        {
            genes.Add(Random.Range(-180f, 180f));
        }
        speedGene = Random.Range(5f, 20f);
        sizeGene = Random.Range(0.05f, 0.15f);
    }
    public DNA(DNA parent, DNA partner, float mutationRate=0.01f)
    {
        for (int i = 0; i < parent.genes.Count; i++)
        {
            float mutationChance = Random.Range(0.0f, 1.0f);
            if(mutationChance <= mutationRate)
            {
                genes.Add(Random.Range(-180f, 180f));
            }
            else
            {
                int chance = Random.Range(0, 2);
                if(chance == 0)
                {
                    genes.Add(parent.genes[i]);
                }
                else
                {
                    genes.Add(partner.genes[i]);
                }
                
            }
        }
        speedGene = Random.Range(0, 2) == 0 ? parent.speedGene : partner.speedGene;
        sizeGene = Random.Range(0, 2) == 0 ? parent.sizeGene : partner.sizeGene;

        if (Random.Range(0f, 1f) < mutationRate) speedGene = Random.Range(5f, 20f);
        if (Random.Range(0f, 1f) < mutationRate) sizeGene = Random.Range(0.05f, 0.15f);
    }  
}

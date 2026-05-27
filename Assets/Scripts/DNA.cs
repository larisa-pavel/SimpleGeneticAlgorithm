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
            int chance = Random.Range(0, 2);
            float inheritedAngle = (chance == 0) ? parent.genes[i] : partner.genes[i];
            if (Random.Range(0f, 1f) <= mutationRate)
            {
                inheritedAngle += Random.Range(-90f, 90f);
                if (inheritedAngle > 180f) inheritedAngle -= 360f;
                if (inheritedAngle < -180f) inheritedAngle += 360f;
            }
            
            genes.Add(inheritedAngle);
        }
        speedGene = Random.Range(0, 2) == 0 ? parent.speedGene : partner.speedGene;
        sizeGene = Random.Range(0, 2) == 0 ? parent.sizeGene : partner.sizeGene;

        if (Random.Range(0f, 1f) < mutationRate) speedGene = Random.Range(5f, 20f);
        if (Random.Range(0f, 1f) < mutationRate) sizeGene = Random.Range(0.05f, 0.15f);
    }  
}

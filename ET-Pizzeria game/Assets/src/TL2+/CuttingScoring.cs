
using UnityEngine;

public class CuttingScoring : MonoBehaviour
{
    private int scorePerCorrectCut = 10;
    private int penaltyPerExtraCut = -5;
    //public Pizza pizza;

    public int CutScore(Pizza pizza)
    {
        int idealCuts = 8; //Will call from Order/Endday somthing later
        int cuts = pizza.GetCut();
        int score = 0;

        if (cuts == idealCuts)
        {
            score = idealCuts * scorePerCorrectCut;
            Debug.Log("Perfect cut! Score: " + score);
        }
        else if (cuts < idealCuts)
        {
            score = cuts * scorePerCorrectCut;
            Debug.Log("Not enough cuts. Score: " + score);
        }
        else
        {
            int extraCuts = cuts - idealCuts;
            score = (idealCuts * scorePerCorrectCut) + (extraCuts * penaltyPerExtraCut);
            Debug.Log("Too many cuts! Score: " + score);
        }

        return score;
    }
}
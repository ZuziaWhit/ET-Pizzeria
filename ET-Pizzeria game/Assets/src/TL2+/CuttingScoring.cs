
using UnityEngine;

public class CuttingScoring : MonoBehaviour
{
    private int scorePerCorrectCut = 10;
    private int penaltyPerExtraCut = -5;
    private int idealCuts = 8;
    private int cuts = 0;
    private int score = 0;
    private int extraCuts = 0;

    public int CutScore(Pizza pizza)
    {
        idealCuts = 8; //Will call from Order/Endday somthing later
        cuts = pizza.GetCut();
        score = 0;

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
            extraCuts = cuts - idealCuts;
            score = (idealCuts * scorePerCorrectCut) + (extraCuts * penaltyPerExtraCut);
            Debug.Log("Too many cuts! Score: " + score);
        }

        return score;
    }
}
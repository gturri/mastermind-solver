﻿namespace mastermind_solver;

public class Combination(Token token1, Token token2, Token token3, Token token4)
{
    private readonly Token[] _tokens = [token1, token2, token3, token4];

    public CombinationResult ComputeResult(Combination model)
    {
        var nbAtGoodPosition = ComputeNbSameColorsAtSamePosition(model);
        var nbGoodColorOverall = ComputeNbSameColors(model);
        return new CombinationResult(nbAtGoodPosition: nbAtGoodPosition, nbGoodColorAtBadPosition: nbGoodColorOverall - nbAtGoodPosition);
    }

    public bool IsCandidateSolution(IEnumerable<PlayedCombination> playedCombinations)
    {
        return playedCombinations.All(playedCombination => playedCombination.Result.Equals(ComputeResult(playedCombination.Combination)));
    }


    private int ComputeNbSameColorsAtSamePosition(Combination other)
    {
        var res = 0;
        for (var idx = 0; idx < _tokens.Length; idx++)
        {
            if (_tokens[idx] == other._tokens[idx])
            {
                res++;
            }
        }

        return res;
    }

    private int ComputeNbSameColors(Combination other)
    {
        var res = 0;
        var idxAlreadyTakenIntoAccount = new HashSet<int>();
        for (var thisIdx = 0; thisIdx < _tokens.Length; thisIdx++)
        {
            for (var otherIdx = 0; otherIdx < _tokens.Length; otherIdx++)
            {
                if (idxAlreadyTakenIntoAccount.Contains(otherIdx)) { continue; }

                if (_tokens[thisIdx] == other._tokens[otherIdx])
                {
                    res++;
                    idxAlreadyTakenIntoAccount.Add(otherIdx);
                    break;
                }
            }
        }

        return res;
    }

    public override string ToString() => "[" + string.Join(", ", _tokens.Select(t => Enum.GetName(typeof(Token), t))) + "]";
}
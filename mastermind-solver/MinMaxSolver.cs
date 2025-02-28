namespace mastermind_solver;

internal class MinMaxSolver(bool verbose): Solver {

    public override Combination ComputeNextGuess(List<PlayedCombination> playedCombinations)
    {
        var candidates = AllCombinations
            .Where(c => c.IsCandidateSolution(playedCombinations))
            .ToArray();

        if (candidates.Length == 0)
        {
            throw new Exception("No candidate found");
        }

        var originalPlayedCombinations = playedCombinations.ToArray();

        var nextGuess = candidates
            .Select(c => (c, ComputeWorstCaseRemainingCandidatesIfThisOneIsPicked(c, candidates, originalPlayedCombinations)))
            .MinBy(t => t.Item2);

        if (verbose)
        {
            Console.WriteLine($"Still {candidates.Length} candidates. After this guess we should have at most {nextGuess.Item2} candidates left");
        }

        return nextGuess.Item1;
    }

    private int ComputeWorstCaseRemainingCandidatesIfThisOneIsPicked(Combination consideredCandidate, Combination[] remainingCandidatesSoFar, PlayedCombination[] playedCombinations)
    {
        var worstCase = 0;
        foreach (var solution in remainingCandidatesSoFar)
        {
            var nextPlayedCombination = new List<PlayedCombination>(playedCombinations) { new(consideredCandidate, consideredCandidate.ComputeResult(solution)) };
            var nbRemainingCandidatesNextTurn = AllCombinations.Count(c => c.IsCandidateSolution(nextPlayedCombination));
            worstCase = Math.Max(worstCase, nbRemainingCandidatesNextTurn);
        }

        return worstCase;
    }
}
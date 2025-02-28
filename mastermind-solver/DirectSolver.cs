namespace mastermind_solver;

internal class DirectSolver(bool verbose=true, bool useHardCodedFirstGuess=false) : Solver
{
    public override Combination ComputeNextGuess(List<PlayedCombination> playedCombinations)
    {
        if (useHardCodedFirstGuess && playedCombinations.Count == 0)
        {
            return new Combination(Token.BLACK, Token.BLACK, Token.WHITE, Token.WHITE);
        }

        // It would be cheaper to stop at the first candidate, but I'm interested in the number of candidate left, and it's still pretty cheap anyway
        var candidates = AllCombinations
            .Where(c => c.IsCandidateSolution(playedCombinations))
            .ToArray();
        if (verbose)
        {
            Console.WriteLine($"Still {candidates.Length} candidates");
        }

        if (candidates.Length == 0)
        {
            throw new Exception("No candidate found");
        }

        return candidates[0];
    }
}
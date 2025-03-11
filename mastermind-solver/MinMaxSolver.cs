namespace mastermind_solver;

/**
 * Solver which makes a guess that will minimize the number of possible solutions for the next iteration.
 * <param name="allowGuessKnownToNotBeTheSolution">If set to TRUE then each guess is a possible solution. This reduces a bit the time to compute the next guess, but in the worst case
 * some solutions (54 of them) will require up to 6 guesses. Though the average number of guesses is 4.50.
 * If set to TRUE then some guesses will be known to not be a possible solution. This increases a bit the time to compute the next guess, but in the worst case it can only take up to 5
 * guesses to find the solution. Though the average number of guesses is 4.76
 * </param>
 * <param name="usePrecomputations">Should be left to TRUE in order to take the first guesses from a cache -which speeds up a lot this solver. (The only good reason to set it to FALSE)
 * is to force the re-computation of those guesses, in order to check the content of this cache.
 * </param>
 */
internal class MinMaxSolver(bool verbose=true, bool allowGuessKnownToNotBeTheSolution=false, bool usePrecomputations=true): Solver {

    public override Combination ComputeNextGuess(List<PlayedCombination> playedCombinations)
    {
        if (TryGetPrecomputedGuess(playedCombinations, out var precomputedGuess))
        {
            return precomputedGuess;
        }
        var possibleSolutions = AllCombinations
                .Where(c => c.IsCandidateSolution(playedCombinations))
                .ToArray();
        if (possibleSolutions.Length == 0)
        {
            throw new Exception("No candidate found");
        }


        var candidates = AllCombinations;
        if (!allowGuessKnownToNotBeTheSolution)
        {
            candidates = candidates
                .Where(c => c.IsCandidateSolution(playedCombinations))
                .ToArray();
        }

        var originalPlayedCombinations = new List<PlayedCombination>(playedCombinations);

        var nextGuess = candidates
            .Select(c => (c, ComputeWorstCaseRemainingCandidatesIfThisOneIsPicked(c, possibleSolutions, originalPlayedCombinations)))
            .MinBy(t => t.Item2);

        if (verbose)
        {
            Console.WriteLine($"Still {possibleSolutions.Length} candidates. After this guess we should have at most {nextGuess.Item2} candidates left");
        }

        return nextGuess.Item1;
    }

    private static readonly Combination _placeholderCombination = new(Token.BLACK, Token.BLACK, Token.BLACK, Token.BLACK);
    // TODO: this should probably be replaced with a lookup in a some maps
    private bool TryGetPrecomputedGuess(List<PlayedCombination> playedCombinations, out Combination precomputedGuess)
    {
        precomputedGuess = _placeholderCombination;
        if (!usePrecomputations || playedCombinations.Count > 1)
        {
            return false;

        }
        if (playedCombinations.Count == 0)
        {
            precomputedGuess = new Combination(Token.BLACK, Token.BLACK, Token.WHITE, Token.WHITE);
            return true;
        }

        // Below playedCombinations.Count == 1
        var result = playedCombinations[0].Result;
        if (result.NbAtGoodPosition == 0)
        {
            switch (result.NbGoodColorAtBadPosition)
            {
                case 0:
                    precomputedGuess = new Combination(Token.RED, Token.RED, Token.YELLOW, Token.GREEN);
                    return true;
                case 1:
                    precomputedGuess = new Combination(Token.WHITE, Token.RED, Token.YELLOW, Token.YELLOW);
                    return true;
                case 2:
                    precomputedGuess = allowGuessKnownToNotBeTheSolution ? new Combination(Token.WHITE, Token.RED, Token.YELLOW, Token.YELLOW) : new Combination(Token.WHITE, Token.RED, Token.BLACK, Token.YELLOW);
                    return true;
                case 3:
                    precomputedGuess = allowGuessKnownToNotBeTheSolution ? new Combination(Token.BLACK, Token.WHITE, Token.BLACK, Token.RED) : new Combination(Token.WHITE, Token.WHITE, Token.BLACK, Token.RED);
                    return true;
                case 4:
                    precomputedGuess = new Combination(Token.WHITE, Token.WHITE, Token.BLACK, Token.BLACK);
                    return true;
            }
        }

        if (result.NbAtGoodPosition == 1)
        {
            switch (result.NbGoodColorAtBadPosition)
            {
                case 0:
                    precomputedGuess = new Combination(Token.BLACK, Token.RED, Token.YELLOW, Token.YELLOW);
                    return true;
                case 1:
                    precomputedGuess = allowGuessKnownToNotBeTheSolution ? new Combination(Token.BLACK, Token.BLACK, Token.RED, Token.YELLOW) : new Combination(Token.BLACK, Token.RED, Token.BLACK, Token.YELLOW);
                    return true;
                case 2:
                    precomputedGuess = new Combination(Token.BLACK, Token.WHITE, Token.BLACK, Token.RED);
                    return true;
                case 3:
                    return false;
            }
        }

        if (result.NbAtGoodPosition == 2)
        {
            switch (result.NbGoodColorAtBadPosition)
            {
                case 0:
                    precomputedGuess = allowGuessKnownToNotBeTheSolution ? new Combination(Token.BLACK, Token.WHITE, Token.RED, Token.YELLOW) : new Combination(Token.BLACK, Token.BLACK, Token.RED, Token.YELLOW);
                    return true;
                case 1:
                    precomputedGuess = new Combination(Token.BLACK, Token.WHITE, Token.WHITE, Token.RED);
                    return true;
                case 2:
                    precomputedGuess = allowGuessKnownToNotBeTheSolution ? new Combination(Token.BLACK, Token.WHITE, Token.BLACK, Token.RED) : new Combination(Token.BLACK, Token.WHITE, Token.BLACK, Token.WHITE);
                    return true;
            }
        }

        if (result.NbAtGoodPosition == 3)
        {
            switch (result.NbGoodColorAtBadPosition)
            {
                case 0:
                    precomputedGuess = allowGuessKnownToNotBeTheSolution ? new Combination(Token.BLACK, Token.WHITE, Token.WHITE, Token.RED) : new Combination(Token.BLACK, Token.BLACK, Token.WHITE, Token.RED);
                    return true;
                case 1:
                    return false;
            }
        }


        // Should never occur.
        return false;

    }

    // remainingCandidatesSoFar could be deduced from playedCombinations, but since this method is meant to be used in a loop, we take it as a parameter so it does not need to be
    // recomputed at each iteration
    private int ComputeWorstCaseRemainingCandidatesIfThisOneIsPicked(Combination consideredCandidate, Combination[] remainingCandidatesSoFar, IEnumerable<PlayedCombination> playedCombinations)
    {
        var possibleResults = new HashSet<CombinationResult>(remainingCandidatesSoFar.Select(consideredCandidate.ComputeResult));

        var worstCase = 0;
        foreach (var result in possibleResults)
        {
            var nextPlayedCombinations = new List<PlayedCombination>(playedCombinations) { new(consideredCandidate, result) };
            var nbRemainingCandidatesNextTurn = remainingCandidatesSoFar.Count(c => !c.Equals(consideredCandidate) && c.IsCandidateSolution(nextPlayedCombinations));
            worstCase = Math.Max(worstCase, nbRemainingCandidatesNextTurn);
        }

        return worstCase;
    }
}
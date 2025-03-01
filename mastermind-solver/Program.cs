using mastermind_solver;
using System.Diagnostics;

//Play();
ComputeAndPrintNumberOfGuessesForAllCombinations();



void Play()
{
    var stillPlaying = true;
    var playedCombinations = new List<PlayedCombination>();
    while (stillPlaying)
    {
        var candidate = new DirectSolver().ComputeNextGuess(playedCombinations);
        Console.WriteLine(
            $"Candidate: {candidate} (expected input: \"nbAtTheCorrectPosition nbGoodColorAtInCorrectPosition\"");
        string? read = null;
        while (read == null || read.Trim() == "")
        {
            read = Console.ReadLine();
        }

        // TODO: add some verification on the type of the input
        var splits = read.Trim().Split(' ');
        var result = new CombinationResult(nbAtGoodPosition: Convert.ToInt32(splits[0]),
            nbGoodColorAtBadPosition: Convert.ToInt32(splits[1]));

        if (result.NbAtGoodPosition == 4)
        {
            stillPlaying = false;
        }

        playedCombinations.Add(new PlayedCombination(candidate, result));

    }

    Console.WriteLine("DONE");
}

// TODO: put the following method in a better location
void ComputeAndPrintNumberOfGuessesForAllCombinations()
{
    var stopwatch = new Stopwatch();
    var histogram = new Dictionary<int, int>();
    var cpt = 0;
    var maxSoFar = 0;
    Combination? argMaxSoFar = null;

    stopwatch.Start();
    foreach (var combination in Solver.AllCombinations)
    {
        var nbGuesses = ComputeNbIterationsToSolve(combination);
        cpt++;
        if (cpt % 10 == 0)
        {
            Console.WriteLine($"Done with {cpt} / {6 * 6 * 6 * 6} => {nbGuesses}");
        }

        if (!histogram.TryAdd(nbGuesses, 1))
        {
            histogram[nbGuesses]++;
        }

        if (nbGuesses > maxSoFar)
        {
            maxSoFar = nbGuesses;
            argMaxSoFar = combination;
        }
    }

    stopwatch.Stop();

    Console.WriteLine("Took: " + stopwatch.ElapsedMilliseconds + "ms");
    Console.WriteLine($"Hardest combination to find: {argMaxSoFar}. Requires {maxSoFar} iterations");
    for (var nbGuesses = 1; nbGuesses <= maxSoFar; nbGuesses++)
    {
        Console.WriteLine($"{nbGuesses}: {(histogram.TryGetValue(nbGuesses, out var value) ? value : 0)}");
    }
}

int ComputeNbIterationsToSolve(Combination solution)
{
    var nbIterations = 0;
    var playedCombinations = new List<PlayedCombination>();
    //var solver = new MinMaxSolver(verbose: false, allowGuessKnownToNotBeTheSolution: false);
    var solver = new DirectSolver(verbose: false);

    while (true)
    {
        nbIterations++;
        var candidate = solver.ComputeNextGuess(playedCombinations);
        var result = candidate.ComputeResult(solution);
        if (result.NbAtGoodPosition == 4)
        {
            return nbIterations;
        }
        playedCombinations.Add(new PlayedCombination(candidate, result));
    }
}

using mastermind_solver;

namespace mastermind_solver_tests
{
    public class T_Combination
    {
        [Test]
        public void TestComputeResult()
        {
            var model = new Combination(Token.BLACK, Token.BLUE, Token.BLUE, Token.GREEN);
            var candidate = new Combination(Token.BLACK, Token.GREEN, Token.GREEN, Token.BLUE);

            Assert.That(candidate.ComputeResult(model), Is.EqualTo(
                   new CombinationResult(
                       nbAtGoodPosition: 1,
                       nbGoodColorAtBadPosition: 2
                       )
                    )
            );
        }

        [Test]
        public void TestIsCandidateSolution()
        {
            var playedCombination = new PlayedCombination[]
            {
                new(new Combination(Token.BLACK, Token.BLACK, Token.BLACK, Token.BLACK), new CombinationResult(1, 0)),
                new(new Combination(Token.BLACK, Token.RED, Token.RED, Token.RED), new CombinationResult(2, 0)),
                new(new Combination(Token.BLACK, Token.RED, Token.GREEN, Token.GREEN), new CombinationResult(2, 2))
            };


            var candidate = new Combination(Token.BLACK, Token.GREEN, Token.GREEN, Token.RED);
            Assert.That(candidate.IsCandidateSolution(playedCombination));

            var notCandidate = new Combination(Token.BLACK, Token.GREEN, Token.GREEN, Token.WHITE);
            Assert.That(!notCandidate.IsCandidateSolution(playedCombination));

        }
    }
}

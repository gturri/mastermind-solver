using mastermind_solver;

namespace mastermind_solver_tests
{
    public class T_CombinationResult
    {
        [Test]
        public void TestAreEquals()
        {
            Assert.That(
                new CombinationResult(1, 2),
                Is.EqualTo(new CombinationResult(1, 2))
            );
        }

        [Test]
        public void TestAreNotEquals()
        {
            Assert.That(
                new CombinationResult(1, 2),
                Is.Not.EqualTo(new CombinationResult(2, 1))
            );
        }

        [Test]
        public void TestNullIsNotEquals()
        {
            Assert.That(
                new CombinationResult(1, 2),
                Is.Not.EqualTo(null)
            );
        }
    }
}
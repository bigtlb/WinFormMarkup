namespace UnitTests.Extensions
{
    public static class FloatExtensions
    {
        public static bool AreClose(this float first, float second)
        {
            var delta = first - second;
            return delta is < float.Epsilon and > -float.Epsilon;
        }
    }
}
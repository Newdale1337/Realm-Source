using System;

namespace Externals.Utilities
{
    public static class RandomUtils
    {
        public static uint NextUint(this Random random, int min, int max)
        {
            int number = random.Next(min, Int32.MaxValue);
            uint uintNumber = (uint)(number + (uint)Int32.MaxValue);

            return uintNumber;
        }

        public static uint NextUint(this Random random, int max) => NextUint(random, 0, max);
    }
}
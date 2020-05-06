namespace Externals.Utilities
{
    public sealed class ClientRandom
    {
        public uint Seed { get; set; }

        public ClientRandom(uint seed) => Seed = seed;

        public uint Next(uint min, uint max) => min == max ? min : (min + Sample() % (max - min));

        private uint Sample()
        {
            var lb = 16807 * (Seed & 0xFFFF);
            var hb = 16807 * (Seed >> 16);
            lb = lb + ((hb & 32767) << 16);
            lb = lb + (hb >> 15);
            if (lb > 2147483647)
                lb = lb - 2147483647;
            return Seed = lb;
        }
    }
}
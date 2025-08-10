
namespace OnlineOrderingSystem
{
    internal class OrderingDbContextw : IDisposable
    {
        public object Database { get; internal set; }

        internal void SeedDatabase()
        {
            throw new NotImplementedException();
        }
    }
}
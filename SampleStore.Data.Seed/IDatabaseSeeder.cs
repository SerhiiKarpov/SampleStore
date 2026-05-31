using System.Threading.Tasks;

namespace SampleStore.Data.Seed;

public interface IDatabaseSeeder
{
    Task EnsureSeeded();
}
using System.Threading.Tasks;

namespace SampleStore.Data.Seed.Commands;

internal interface IDatabaseSeederCommand
{
    Task Do();
}
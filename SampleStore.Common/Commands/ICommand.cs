using System.Threading.Tasks;

namespace SampleStore.Common.Commands;

public interface ICommand<TResult>
{
    Task<TResult> Do();
}
using System.Threading.Tasks;

namespace Assets
{
    public interface ITargetService
    {
        Task<ResponseMessage<TargetModel>> Get(string VuforiaId);
    }
}
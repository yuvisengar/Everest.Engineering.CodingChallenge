using Everest.Engineering.Business.Models;
using System.Threading.Tasks;

namespace Everest.Engineering.UI.Services
{
    public interface IUserInteractionService
    {
        Task<CostEstimateInput> GetCostEstimateInput();
        Task PrintCostEstimateOutput(CostEstimateOutput costEstimateOutput);

        Task<TimeEstimateInput> GetCostAndTimeEstimateInput();
        Task PrintCostAndTimeEstimateOutput(TimeEstimateOutput timeEstimateOutput);
    }
}

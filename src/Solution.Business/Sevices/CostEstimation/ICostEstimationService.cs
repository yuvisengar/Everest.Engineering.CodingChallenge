using Everest.Engineering.Business.Models;
using System.Threading.Tasks;

namespace Everest.Engineering.Business.Sevices.CostEstimation
{
    public interface ICostEstimationService
    {
        Task<CostEstimateOutput> EstimateCost(CostEstimateInput costEstimateInput);
    }
}

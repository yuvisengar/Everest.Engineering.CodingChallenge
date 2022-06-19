using Everest.Engineering.Business.Models;
using System.Threading.Tasks;

namespace Everest.Engineering.Business.Sevices.TimeEstimation
{
    public interface ITimeEstimationService
    {
        Task<TimeEstimateOutput> EstimateCostAndTime(TimeEstimateInput costEstimateInput);
    }
}

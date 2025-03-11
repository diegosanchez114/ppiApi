using UPIT.Domain.BusinessLogic.PPI;
using UPIT.Domain.Interfaces.PPI;
using UPIT.Domain.Models.PPI;

namespace UPIT.Application.Services.PPI
{
    public class PPIAvanceService
    {

        private readonly IPPIContratoRepository _repository;
        
        public PPIAvanceService(
          IPPIContratoRepository repository
        )
        {
            _repository = repository;            
        }

        public async Task<PPIAvance> ValidateAvanceBeforeCreate(PPIAvance model)
        {
            try
            {
                model.TieneAlerta = false;
                model.AlertaResuelta = false;
                var contrato = await _repository.GetByIdAsync(model.IdContrato);
                var blAvance = new PPIAvanceBusinessLogic();
                var diffDays = blAvance.CalculateDifferenceDays(contrato, model);
                if (diffDays > 0) model.TieneAlerta = true;                
                return model;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return model;
            }            
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PigWebApplication.Models;

namespace PigWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartController : ControllerBase
    {

        private readonly DbpigsContext _context;

        public ChartController(DbpigsContext context)
        {
            _context = context;
        }

        [HttpGet("JsonDataPig")]
        public JsonResult JsonDataPig()
        {
            var breeds = _context.Breeds.Include(b => b.Pigs).ToList();
            List<object> breedPig = new();
            breedPig.Add(new[] { "Порода", "Кількість свиней" });
            foreach(var breed in breeds)
            {
                breedPig.Add(new object[] { breed.Name, breed.Pigs.Count });
            }

            return new JsonResult(breedPig);
        }

        [HttpGet("JsonDataInjection")]
        public JsonResult JsonDataInjection()
        {
            var medicines = _context.Medicines.Include(m => m.Injections).ToList();
            List<object> medicineInjections = new();
            medicineInjections.Add(new[] { "Препарат", "Кількість ін'єкцій препарату" });
            foreach (var medicine in medicines)
            {
                medicineInjections.Add(new object[] { medicine.Name, medicine.Injections.Count });
            }

            return new JsonResult(medicineInjections);
        }

    }
}

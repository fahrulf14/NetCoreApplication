using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NUNA.Models.BaseApplicationContext;
using NUNA.ViewModels.Home;

namespace NUNA.Components
{
    public class SubheaderViewComponent : ViewComponent
    {
        private readonly BaseApplicationContext _appContext;

        public SubheaderViewComponent(BaseApplicationContext context)
        {
            _appContext = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<SubheaderDto> PopularSearch = new List<SubheaderDto> 
            {
                new SubheaderDto
                {
                    Name = "Lenovo",
                    Url = "#"
                },
                new SubheaderDto
                {
                    Name = "Asus",
                    Url = "#"
                },
                new SubheaderDto
                {
                    Name = "Laptop Desain",
                    Url = "#"
                }
            };

            return await Task.FromResult((IViewComponentResult)View("Default", PopularSearch.ToList()));
        }
    }
}

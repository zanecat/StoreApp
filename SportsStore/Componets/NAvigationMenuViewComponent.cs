using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using SportsStore.Models;
using System.Linq;

namespace SportsStore.Componets
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        private IProductRepository repository;
        public NavigationMenuViewComponent(IProductRepository repo)
        {
            repository = repo;
        }
        public IViewComponentResult Invoke() =>
            View(repository.Products
                .Select(x => x.Category)
                .Distinct()
                .OrderBy(x => x));
    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ThirdPartyOrderingService.Services
{
    public interface IUnderCutterService { }

    public class UnderCuttersService : ThirdPartySupplier, IUnderCutterService
    {
        public UnderCuttersService(HttpClient Client, OrderContext Context) : base("http://undercutters.azurewebsites.net/", Client, Context)
        {
        }
    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ThirdPartyOrderingService.Services
{
    public interface IDodgyDealersService { }

    public class DodgyDealersService : ThirdPartySupplier, IDodgyDealersService
    {
        public DodgyDealersService(HttpClient Client, OrderContext Context) : base("http://dodgydealers.azurewebsites.net/", Client, Context)
        {
            
        }
    }
}

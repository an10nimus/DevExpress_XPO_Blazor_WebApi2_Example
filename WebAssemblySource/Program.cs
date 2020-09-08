using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using DevExpress.Xpo;
using DevExpress.Data;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.Metadata;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;



namespace WebApiClient
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");
            builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            IDataStore DataStore = new WebAPIDataStore(builder.HostEnvironment.BaseAddress);
            XPDictionary Dictionary = new ReflectionDictionary();
            Dictionary.GetDataStoreSchema(typeof(Customer), typeof(Order));
            IDataLayer DataLayer = new SimpleDataLayer(Dictionary, DataStore);
            builder.Services.AddSingleton(DataLayer);

            XpoDefault.DataLayer = null;
            XpoDefault.Session = null;

            builder.Build().RunAsync();
        }

        
    }
}

using System;
using System.Net.Http;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using DevExpress.Xpo.DB;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using WebAssemblySource.Models;

namespace WebAssemblySource {
    public class Program {
        public static void Main(string[] args) {
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
using System;
using System.Net.Http;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using DevExpress.Xpo.DB;
using DevExpress.Xpo;

namespace DxSample.Client
{
    public class Program
    {
        public static void Main(string[] args) {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");
            builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            IDataStore DataStore = new WebAPIDataStore(builder.HostEnvironment.BaseAddress);
            XpoDefault.DataLayer = new SimpleDataLayer(DataStore);

            XpoDefault.Session = null;
            builder.Build().RunAsync();
        }
    }
}
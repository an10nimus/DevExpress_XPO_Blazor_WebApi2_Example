#pragma checksum "D:\Testing\T883562\DxSample\Client\Pages\Customers\EditCustomer.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "7a1f79233ea68cc35be6c44791f1c64f19737f63"
// <auto-generated/>
#pragma warning disable 1591
#pragma warning disable 0414
#pragma warning disable 0649
#pragma warning disable 0169

namespace DxSample.Client.Pages.Customers
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
#nullable restore
#line 1 "D:\Testing\T883562\DxSample\Client\_Imports.razor"
using System.Net.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\Testing\T883562\DxSample\Client\_Imports.razor"
using System.Net.Http.Json;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "D:\Testing\T883562\DxSample\Client\_Imports.razor"
using Microsoft.AspNetCore.Components.Forms;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "D:\Testing\T883562\DxSample\Client\_Imports.razor"
using Microsoft.AspNetCore.Components.Routing;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "D:\Testing\T883562\DxSample\Client\_Imports.razor"
using Microsoft.AspNetCore.Components.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "D:\Testing\T883562\DxSample\Client\_Imports.razor"
using Microsoft.AspNetCore.Components.WebAssembly.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "D:\Testing\T883562\DxSample\Client\_Imports.razor"
using Microsoft.JSInterop;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "D:\Testing\T883562\DxSample\Client\_Imports.razor"
using DxSample.Client;

#line default
#line hidden
#nullable disable
#nullable restore
#line 9 "D:\Testing\T883562\DxSample\Client\_Imports.razor"
using DxSample.Client.Shared;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "D:\Testing\T883562\DxSample\Client\Pages\Customers\EditCustomer.razor"
using DxSample.Client.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "D:\Testing\T883562\DxSample\Client\Pages\Customers\EditCustomer.razor"
using DevExpress.Xpo;

#line default
#line hidden
#nullable disable
    [Microsoft.AspNetCore.Components.RouteAttribute("/customers/{CustomerOid:int}/edit")]
    public partial class EditCustomer : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
        }
        #pragma warning restore 1998
#nullable restore
#line 25 "D:\Testing\T883562\DxSample\Client\Pages\Customers\EditCustomer.razor"
       
    public static UnitOfWork uow = new UnitOfWork();
    [Parameter]
    public int CustomerOid { get; set; }
    public Customer customer;

    private async Task SaveCustomer() {
        await uow.CommitChangesAsync();
        Nav.NavigateTo("/customers");
    }

    protected override async Task OnInitializedAsync() {
        customer = await uow.GetObjectByKeyAsync<Customer>(CustomerOid);
    }

#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private NavigationManager Nav { get; set; }
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private HttpClient Http { get; set; }
    }
}
#pragma warning restore 1591

using HttpClients.ClientInterfaces;
using HttpClients.Implementations;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

//when the Dependency Injection container is requested to provide an
//instance of IUserService, it will create an instance of UserHttpClient and return it
builder.Services.AddScoped<IUserService, UserHttpClient>(); 
//Dependency injection for ITodoService
builder.Services.AddScoped<ITodoService, TodoHttpClient>();

builder.Services.AddScoped(
    sp => 
        new HttpClient { 
            BaseAddress = new Uri("https://localhost:7038") //This is the address WebAPI is running on. We need to contact WebAPI so that we must provide WbeAPIs address
        }
);



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
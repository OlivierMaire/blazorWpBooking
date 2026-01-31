using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using blazorWpBooking.Components.Account;
using Microsoft.EntityFrameworkCore;
using blazorWpBooking.Client.Pages;
using blazorWpBooking.Components;
using blazorWpBooking.Data;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents()
    .AddAuthenticationStateSerialization();


builder.Services.AddHttpContextAccessor();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityRedirectManager>();
// add circuit-scoped token store and the provider implementation
builder.Services.AddScoped<CircuitTokenStore>();
builder.Services.AddScoped<AuthenticationStateProvider, WordPressRevalidatingAuthenticationStateProvider>();
builder.Services.AddScoped<WordPressRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // use Always in production with HTTPS
})
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? ""))
    };
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// builder.Services.AddIdentityCore<ApplicationUser>(options =>
//     {
//         options.SignIn.RequireConfirmedAccount = true;
//         options.Stores.SchemaVersion = IdentitySchemaVersions.Version3;
//     })
//     .AddEntityFrameworkStores<ApplicationDbContext>()
//     .AddSignInManager()
//     .AddDefaultTokenProviders();

// builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

builder.Services.AddTransient<WordPressAuthentication>(sp =>
{
    var wpAuth = new WordPressAuthentication
    {
        WordPressApiUrl = builder.Configuration["WordPress:ApiUrl"] ?? string.Empty
    };
    return wpAuth;
});
builder.Services.AddTransient<ServerTokenHandler>();
builder.Services.AddHttpClient("ApiWithToken")
    .AddHttpMessageHandler<ServerTokenHandler>();

// Application services
builder.Services.AddScoped<blazorWpBooking.Services.LessonService>();
builder.Services.AddScoped<blazorWpBooking.Services.LessonTypeService>();
builder.Services.AddScoped<blazorWpBooking.Services.ScheduleService>();
builder.Services.AddScoped<blazorWpBooking.Services.LocationService>();
builder.Services.AddScoped<blazorWpBooking.Services.CalendarService>();

var app = builder.Build();

// Apply pending migrations on startup in Development only
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await dbContext.Database.MigrateAsync();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute(pathFormat: "/not-found" , createScopeForStatusCodePages: true);

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AllowAnonymous() // AllowAnonymous for all pages, otherwise Authorization cannot be handled when using [Authorize] in Razor components.
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(blazorWpBooking.Client._Imports).Assembly);

// Add additional endpoints required by the Identity /Account Razor components.
// app.MapAdditionalIdentityEndpoints();

app.Run();

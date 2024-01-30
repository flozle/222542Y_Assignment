using Authentication.Model;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using WebApp_Core_Identity.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

//contect for DB
builder.Services.AddDbContext<UserDbContext>();
builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<UserDbContext>();

//google captcha
builder.Services.Configure<GoogleCaptchaConfig>(builder.Configuration.GetSection("GoogleReCaptcha"));
builder.Services.AddTransient(typeof(GoogleCaptchaService));

//account lockout
builder.Services.Configure<IdentityOptions>(options =>
{
	options.Lockout.MaxFailedAccessAttempts = 3;
	options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
	options.Lockout.AllowedForNewUsers = true;
});

//timeout
builder.Services.ConfigureApplicationCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromMinutes(1);
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

//error pages
app.UseStatusCodePagesWithRedirects("/errors/{0}");

app.MapRazorPages();

app.Run();

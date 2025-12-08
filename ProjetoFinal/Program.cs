using ProjetoFinal.Repositorio;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
// INJEÇÃO DE DEPENDENCIA 
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();
builder.Services.AddScoped<CadastroFuncRepositorio>();
builder.Services.AddScoped<CadastroClienteRepositorio>();
builder.Services.AddScoped<CadastroPetRepositorio>();
builder.Services.AddScoped<CadastroPlanoRepositorio>();
builder.Services.AddScoped<PlanoRepositorio>();
builder.Services.AddScoped<FuncionarioRepositorio>();
builder.Services.AddScoped<UsuarioRepositorio>();
builder.Services.AddScoped<PlanoEditarRepositorio>();
builder.Services.AddScoped<ExcluirFuncionarioRepositorio>();
builder.Services.AddScoped<ExcluirClienteRepositorio>();
builder.Services.AddScoped<ExcluirPlanoRepositorio>();
builder.Services.AddScoped<LoginClienteRepositorio>();
builder.Services.AddScoped<AtribuirPetRepositorio>();
builder.Services.AddScoped<AtribuirPlanoRepositorio>();
builder.Services.AddScoped<PetRepositorio>();
builder.Services.AddScoped<EditarPetRepositorio>();
builder.Services.AddScoped<ConsultarPetRepositorio>();
builder.Services.AddScoped<PagamentoRepositorio>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();

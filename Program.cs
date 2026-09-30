using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService();
    db.Database.EnsureCreated();

    if (!db.Usuarios.Any())
    {
        db.Usuarios.Add(new Usuario { Nome = "Enrique", Email = "enrique@teste.com" });
        db.Usuarios.Add(new Usuario { Nome = "Amigo do Deploy", Email = "amigo@teste.com" });
        db.SaveChanges();
        Console.WriteLine("👉 Test data saved successfully!");
    }
}

app.MapGet("/usuarios", async (MeuBancoContext db) =>
{
    return await db.Usuarios.ToListAsync();
});

app.MapPost("/usuarios", async (MeuBancoContext db, Usuario usuario) =>
{
    db.Usuarios.Add(usuario);
    await db.SaveChangesAsync();
    return Results.Created($"/usuarios/{usuario.Id}", usuario);
});

app.Run();

public class Usuario
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public class MeuBancoContext : DbContext
{
    public DbSet Usuarios { get; set; }

    public MeuBancoContext() { }

    public MeuBancoContext(DbContextOptions options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        if (!options.IsConfigured)
        {
            options.UseSqlite("Data Source=meubanco.db");
        }
    }
}
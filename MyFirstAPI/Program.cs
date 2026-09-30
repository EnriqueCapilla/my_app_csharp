using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MeuBancoContext>();
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
    var db = scope.ServiceProvider.GetRequiredService<MeuBancoContext>();
    db.Database.EnsureCreated();

    if (!db.Usuarios.Any())
    {
        db.Usuarios.Add(new Usuario { Nome = "Enrique", Email = "enrique@teste.com" });
        db.SaveChanges();
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
    public DbSet<Usuario> Usuarios { get; set; }

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
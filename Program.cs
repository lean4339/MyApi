using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// agregando contexto de base de datos

var connectionString = "Server=localhost;Integrated security=SSPI; Database=Animales";

//registrando el db context

builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(connectionString));
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();








app.MapGet("/perritos", (ApplicationDbContext db) =>
{
    var animales = db.Animals.ToList();
    IEnumerable<Animal> perros = animales.Where(animal => animal.Tipo == "Perro");

    return Results.Ok(perros);

}).WithName("GetPerros");

app.MapGet("/perrito/{id}", (int id, ApplicationDbContext db) =>
{
    var dbDog = db.Animals.FirstOrDefault(x => x.Id == id);
    if (dbDog == null)
    {
        return Results.NotFound("Perro no existe");
    }
    return Results.Ok(dbDog);

}).WithName("GetPerro");

app.MapPost("/crearPerrito", (string nombre, int patas, int age, ApplicationDbContext db) =>
{
    var perro = new Animal
    {   Tipo = "Perro",
        Name = nombre,
        Patas = patas,
        Age = age,
    };
    db.Animals.Add(perro);
    db.SaveChanges();

    return Results.Ok(perro);
}).WithName("PostPero");

app.MapPut("/actualizarPerrito", (string name, int patas, int age, int id, ApplicationDbContext db) =>
{
    var dbDog = db.Animals.FirstOrDefault(x => x.Id == id);
    if(dbDog == null)
    {
        return Results.NotFound("El perro no existe");
    }
    dbDog.Name = name;
    dbDog.Age = age;
    dbDog.Patas = patas;
    dbDog.Tipo = "Perro";
    db.SaveChanges();
    return Results.Ok(dbDog);

}).WithName("ActualizarPerro");

app.MapDelete("/eliminarPerrito", (int id, ApplicationDbContext db) =>
{
    var dbDog = db.Animals.FirstOrDefault(x => x.Id == id);

    if(dbDog == null)
    {
        return Results.NotFound("El perro no existe");
    }
    db.Remove(dbDog);
    db.SaveChanges();
    return Results.Ok("El perro fue eliminado");

}).WithName("EliminarPerro");

app.Run();

 

public record Animal
{   public int Id { get; set; } 
    public string Name { get; set; }
    public int Age { get; set; }    
    public int Patas { get; set; }
    public string Tipo { get; set; }

}
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :base(options)
    {

    }

    public DbSet<Animal> Animals { get; set; }

}
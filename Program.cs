var builder = WebApplication.CreateBuilder(args);

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

List<Animal> perros = new List<Animal>();

var perro = new Animal
{
    name = "Ambar",
    age = 5,
    patas = 4
};
var perro2 = new Animal
{
    name = "Pichicho",
    age = 2,
    patas = 4,
};

perros.Add(perro2);
perros.Add(perro);

Console.WriteLine(perros);

app.MapGet("/perritos", () =>
{
    
    
    return perros;

}).WithName("GetPerros");

app.MapGet("/perrito/{id}", (int id) =>
{
    if (id < perros.Count)
    {
        return perros[id];
    }
    else { return null; }
    
    

}).WithName("GetPerro");

app.MapPost("/crearPerrito", (string nombre, int patas, int age) =>
{
    var perro = new Animal
    {
        name = nombre,
        patas = patas,
        age = age,
    };
    perros.Add(perro);

    return perro;
}).WithName("PostPero");

app.MapPut("/actualizarPerrito", (string name, int patas, int age, int id) =>
{
    if(id > perros.Count)
    {
        return null;
    }
    else
    {
        perros[id].name = name;
        perros[id].age = age;
        perros[id].patas = patas;

        return perros[id];
       
    }
}).WithName("ActualizarPerro");

app.MapDelete("/eliminarPerrito/{id}", (int id) =>
{
    perros.Remove(perros[id]);
    return perros;

}).WithName("EliminarPerro");

app.Run();

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
public record Animal
{
    public string name { get; set; }
    public int age { get; set; }    
    public int patas { get; set; }

}
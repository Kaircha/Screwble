using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.MapGet("/", ([FromQuery] string word) => {
  using var connection = new SqliteConnection("Data Source=E:\\SQLite\\Mini 135\\Screwble.db");
  connection.Open();
  
  var command = connection.CreateCommand();
  // Escape string avoids SQL injection -> ''; DELETE ALL DATABASES
  command.CommandText = @"SELECT * FROM Thesaurus WHERE Word = $word";
  command.Parameters.AddWithValue("$word", word.ToLower());

  using var reader = command.ExecuteReader();
  while (reader.Read()) {
    var entry = new DatabaseEntry(reader.GetString(1), CultureInfo.InvariantCulture.TextInfo.ToTitleCase(reader.GetString(2)), reader.GetString(3));
    return Results.Ok(entry);
  }

  return Results.NotFound();
});

app.MapPost("/", ([FromBody] DatabaseEntry entry) => {
  using var connection = new SqliteConnection("Data Source=E:\\SQLite\\Mini 135\\Screwble.db");
  connection.Open();

  var command = connection.CreateCommand();
  // Escape string avoids SQL injection -> ''); DELETE ALL DATABASES; --
  command.CommandText = @"INSERT INTO Thesaurus (PlayerName, Word, Definition) VALUES ($playerName, $word, $definition)";
  command.Parameters.AddWithValue("$playerName", entry.PlayerName);
  command.Parameters.AddWithValue("$word", entry.Word.ToLower());
  command.Parameters.AddWithValue("$definition", entry.Definition);

  try {
    command.ExecuteNonQuery();
  } catch (Exception) {
    return Results.BadRequest();
  }

  return Results.Ok();
});

app.Run();

public record DatabaseEntry (
  string PlayerName, 
  string Word,
  string Definition
);
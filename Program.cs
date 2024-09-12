using GameStore.Api.Dtos;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

const string GetGameEndpointName = "GetGame";

List<GameDto> games =[
    new(
        1,
        "Street Fighter II",
        "Fighting",
        19.99M,
        new DateOnly(1992,7,15)),
    new(
        2,
        "Final Fantasy II",
        "Roleplaying",
        59.99M,
        new DateOnly(2002,7,15)),
    new(
        3,
        "IGI II",
        "Fighting",
        75.99M,
        new DateOnly(2004,7,15))
];

// Get /games
app.MapGet("games", () => games );

// Get /games/1
app.MapGet("games/{id}", (int id)=>{
    GameDto? game  =  games.Find(game => game.Id ==id);
    return game is null ? Results.NotFound() : Results.Ok(game); 
    })
    .WithName(GetGameEndpointName);

// Post /games
app.MapPost("games", (CreateGameDto newGame)=>{
    GameDto game = new(
        games.Count +1,
        newGame.Name,
        newGame.Genre,
        newGame.Price,
        newGame.ReleaseDate
    );

    games.Add(game);

    return Results.CreatedAtRoute(GetGameEndpointName, new {id = game.Id}, game);
});

// Put /games/1

app.MapPut("games/{id}", (int id, UpdateGameDto updateGameDto)=>{
    var index = games.FindIndex(game=> game.Id == id);
    if(index == -1){
        return Results.NotFound();
    }

    games[index] = new GameDto(
        id,
        updateGameDto.Name,
        updateGameDto.Genre,
        updateGameDto.Price,
        updateGameDto.ReleaseDate
    );

    return Results.NoContent();

});

// Delete / games/1

app.MapDelete("games/{id}" , (int id) =>{
    games.RemoveAll(game=> game.Id == id);

    return Results.NoContent();
});


app.Run();

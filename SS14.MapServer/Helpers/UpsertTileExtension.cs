﻿using Microsoft.EntityFrameworkCore;
using SS14.MapServer.Models.Entities;

namespace SS14.MapServer.Helpers;

public static class UpsertTileExtension
{
    public static void Upsert(this DbSet<Tile> tiles, Tile tile)
    {
        tiles.FromSql($@"
            Insert Into ""Tiles""  values ({tile.MapId}, {tile.GridId}, {tile.X}, {tile.Y}, {tile.Path}, {tile.Size})
            on conflict on constraint ""PK_Tiles""
            do update set 
            ""Path"" = {tile.Path},
            ""Size"" = {tile.Size};
        ");
    }
}
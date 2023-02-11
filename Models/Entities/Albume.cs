﻿using System;
using System.Collections.Generic;

namespace VentaMusical.Models.Entities;

public partial class Albume
{
    public int AlbumeId { get; set; }

    public int AuthorId { get; set; }

    public int SongId { get; set; }

    public string AlbumeName { get; set; } = null!;

    public DateTime AlbumeYear { get; set; }

    public bool AlbumeState { get; set; }

    public virtual Author Author { get; set; } = null!;

    public virtual Song Song { get; set; } = null!;
}

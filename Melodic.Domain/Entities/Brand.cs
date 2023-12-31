﻿using System.ComponentModel.DataAnnotations;

namespace Melodic.Domain.Entities;

public class Brand
{
    public int Id { get; set; }
    [Required]
    public string? Name { get; set; }
}

﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookRental.Models
{
    public class Genre
    {
        [Required]
        public int GenreId { get; set; }

        [Required]
        [DisplayName("Genre Name")]
        public string Name { get; set; }

    }
}
﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueApp.Data
{
    public class Player
    {
        [Key]
        public int PlayerId { get; set; }
        [Required]
        public Guid OwnerId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string ParentEmail { get; set; }
       
        [ForeignKey(nameof(Team))]
        public int TeamId { get; set; } // many players linked to the one table // took out ? nullable
        //[ForeignKey(nameof(TeamId))]
        public virtual Team Team { get; set; }
    //
    }
}

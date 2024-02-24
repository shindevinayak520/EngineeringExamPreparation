﻿using System.ComponentModel.DataAnnotations;

namespace EngineeringExamPreparation.Models
{
    public class Branch
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Subject>? Subjects { get; set; }
    }
}

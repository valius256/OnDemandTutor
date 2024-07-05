using System;
using System.ComponentModel.DataAnnotations;

namespace OnDemandTutor.Models.Dtos.StudentClass
{
	public class UpdateStudentClassDto
	{
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int ClassId { get; set; }
        public int RatingForTutorId { get; set; }
        [Range(1, 5)]
        public int Rating { get; set; }
    }
}


using CineMasters.Models.Helpers;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace CineMasters.Areas.Shows.Models
{
    public class Movie
    {
        [BsonId(IdGenerator = typeof(CustomIdGenerator))]
        public string InternalId { get; set; }
        public long Id { get; set; }
        public string Title { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime ReleaseDate { get; set; }
        public String CountryOfOrigin { get; set; }
        public Language Language { get; set; } = Language.Engels;
        public Language Subtitle { get; set; } = Language.Nederlands;
        public int Length { get; set; }
        public Classification[] Classification { get; set; }
        public float Rating { get; set; }
        public string Director { get; set; }
        public string[] Actors { get; set; } = new string[] { };
        public Genre[] Genre { get; set; } = new Genre[] { };
        public string Description { get; set; }
    }

    public enum Language
    {
        Engels = 0,
        Nederlands = 1,
        Spaans = 2,
        Frans = 3
    }

    public enum Classification
    {
        Al,
        [Description("6")]
        Zes,
        [Description("9")]
        Negen,
        [Description("12")]
        Twaalf,
        [Description("16")]
        Zestien,
        Geweld,
        Angst,
        Sex,
        [Description("Grof taalgebruik")]
        GrofTaalgebruik,
        [Description("Drugs- en/of alcoholgebruik")]
        Drugs,
        Discriminatie

    }

    public enum Genre
    {
        Actie,
        Comedy,
        Thriller,
        Drama,
        Humor,
        Horror,
        Documentaire,
        Oorlog,
        Western,
        Fantasie,
        [Description("Sci-Fi")]
        SciFi
    }
}
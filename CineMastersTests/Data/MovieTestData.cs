using CineMasters.Areas.Shows.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace CineMastersTests.Data
{
    public class MovieTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { Movies };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public Movie[] Movies => new Movie[]
        {
            new Movie {
                Id = 1,
                Title = "Movie 1",
                ReleaseDate = new DateTime(2019,3,1),
                CountryOfOrigin = "Country 1",
                Language = Language.Engels,
                Subtitle = Language.Nederlands,
                Length = 100,
                Classification = new Classification[]{
                    Classification.Al,
                    Classification.Angst
                },
                Rating = 5.5F,
                Director = "Director",
                Actors = new string[]{ "Actor 1_1", "Actor1_2"},
                Genre = new Genre[]{ Genre.Actie, Genre.Comedy },
                Description = "dsoguhosdjngolnaslfgnaslgas"
            },
            new Movie {
                Id = 2,
                Title = "Movie 2",
                ReleaseDate = new DateTime(2019,3,2),
                CountryOfOrigin = "Country 2",
                Language = Language.Engels,
                Subtitle = Language.Nederlands,
                Length = 100,
                Classification = new Classification[]{
                    Classification.Al,
                    Classification.Angst
                },
                Rating = 5.5F,
                Director = "Director",
                Actors = new string[]{ "Actor2_1", "Actor2_2"},
                Genre = new Genre[]{ Genre.Actie, Genre.Comedy },
                Description = "dsoguhosdjngolnaslfgnaslgas"
            },
            new Movie {
                Id = 3,
                Title = "Movie 3",
                ReleaseDate = new DateTime(2019,3,3),
                CountryOfOrigin = "Country 3",
                Language = Language.Engels,
                Subtitle = Language.Nederlands,
                Length = 100,
                Classification = new Classification[]{
                    Classification.Al,
                    Classification.Angst
                },
                Rating = 5.5F,
                Director = "Director",
                Actors = new string[]{ "Actor 3_1", "Actor3_2"},
                Genre = new Genre[]{ Genre.Actie, Genre.Comedy },
                Description = "dsoguhosdjngolnaslfgnaslgas"
            }
        };
    }
}

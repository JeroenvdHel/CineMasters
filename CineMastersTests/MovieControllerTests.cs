using CineMasters.Areas.Shows.Controllers;
using CineMasters.Areas.Shows.Models;
using CineMasters.Repositories;
using CineMastersTests.Data;
using MongoDB.Driver;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace CineMastersTests
{
    public class MovieControllerTests
    {
        [Theory]
        [ClassData(typeof(MovieTestData))]
        public void CanCreateMovie(Movie[] movies)
        {
            // Arrange
            var movieRepoMock = new Mock<IMovieRepository>();
            movieRepoMock
                .Setup(m => m.Create(It.IsAny<Movie>()))
                .Returns(true);

            //not used
            var showRepoMock = new Mock<IShowRepository>();
            var movie = new Movie { Id = 6, Title = "TestMovie" };
            var controller = new MovieController(movieRepoMock.Object, showRepoMock.Object);

            // Act
            //var result = controller.CreateMovie(movie);


            // Assert

        }
    }
}

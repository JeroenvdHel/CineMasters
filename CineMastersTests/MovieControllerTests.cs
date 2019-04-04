using CineMasters.Areas.Shows.Controllers;
using CineMasters.Areas.Shows.Models;
using CineMasters.Repositories;
using CineMastersTests.Data;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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
            Movie movie = new Movie { Title = "TestMovie" };
            var movieRepoMock = new Mock<IMovieRepository>();
            movieRepoMock
                .Setup(m => m.Create(It.IsAny<Movie>()))
                .Returns(true);

            //not used
            var showRepoMock = new Mock<IShowRepository>();

            var controller = new MovieController(movieRepoMock.Object, showRepoMock.Object);

            // Act
            var result = controller.CreateMovie(movie);

            // Assert
            Assert.NotNull(result);
        }
    }
}

using FitnessApp.API.Entities;

namespace FitnessApp.Test
{
    public class SetTests
    {
        [Fact]
        public void SetConstructor_ConstructSet_NameMustBeSquats()
        {
            // Arrange
            // nothing to arrange

            // Act
            var set = new Set
            {
                Name = "Squats",
            };

            // Assert
            Assert.Equal("Squats", set.Name);
        }
    }
}

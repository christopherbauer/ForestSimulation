using System.Collections.Generic;
using ForestSimulation;
using ForestSimulation.ForestObject;
using ForestSimulation.services;
using Moq;
using NUnit.Framework;

namespace ForestSimulationTests
{
    public static class TimeKeeperTests
    {
        [TestFixture]
        public static class when_starting_to_track_a_forest
        {
            [Test]
            public static void then_should_initialize_year_to_zero()
            {
                // Arrange
                var timeKeeper = new TimeKeeper();

                // Act
                var bound = It.IsAny<int>();
                INumberGeneratorService numberGeneratorService = new Mock<INumberGeneratorService>().Object;
                timeKeeper.StartTrackingForest(new Forest(bound, numberGeneratorService));

                // Assert
                Assert.That(timeKeeper.Year, Is.EqualTo(0));
            }

            [Test]
            public static void then_should_initialize_month_to_one()
            {
                // Arrange
                var timeKeeper = new TimeKeeper();

                // Act
                var bound = It.IsAny<int>();
                var numberGeneratorService = new Mock<INumberGeneratorService>().Object;
                timeKeeper.StartTrackingForest(new Forest(bound, numberGeneratorService));

                // Assert
                Assert.That(timeKeeper.Month, Is.EqualTo(1));
            }
        }

        [TestFixture]
        public static class when_timekeeper_tick
        {
            [Test]
            public static void then_should_add_one_to_month()
            {
                // Arrange
                var timeKeeper = new TimeKeeper();
                var bound = It.IsAny<int>();
                var numberGeneratorService = new Mock<INumberGeneratorService>().Object;
                timeKeeper.StartTrackingForest(new Forest(bound, numberGeneratorService));

                // Act
                timeKeeper.Tick();

                // Assert
                Assert.That(timeKeeper.Month, Is.EqualTo(2));
            }

            [Test]
            public static void then_should_increment_year_if_month_greater_than_twelve()
            {
                // Arrange
                var timeKeeper = new TimeKeeper();
                var forest = new Mock<IForest>();
                forest.Setup(forest1 => forest1.ForestObjects).Returns(new List<IForestObject>());
                forest.Setup(forest1 => forest1.LumberJacks).Returns(new List<ILumberJack>());
                timeKeeper.StartTrackingForest(forest.Object);
                for (var i = 0; i < 11; i++)
                {
                    timeKeeper.Tick();
                }

                // Act
                timeKeeper.Tick();

                // Assert
                Assert.That(timeKeeper.Year, Is.EqualTo(1));
            }
        }

        [TestFixture]
        public static class when_tick_and_checking_lumber_versus_lumberjack_numbers
        {
            [Test]
            public static void then_should_not_hire_new_lumberjacks_if_lumber_less_than_lumberjack_count()
            {
                // Arrange
                var timeKeeper = new TimeKeeper();

                var forest = new Mock<IForest>();
                forest.Setup(forest1 => forest1.ForestObjects).Returns(new List<IForestObject>());
                forest.Setup(forest1 => forest1.Lumber).Returns(2);
                forest.Setup(forest1 => forest1.LumberJacks)
                    .Returns(new List<ILumberJack>()
                    {
                        new Mock<ILumberJack>().Object,
                        new Mock<ILumberJack>().Object,
                        new Mock<ILumberJack>().Object
                    });
                timeKeeper.StartTrackingForest(forest.Object);
                for (var i = 0; i < 11; i++)
                {
                    timeKeeper.Tick();
                }

                // Act
                timeKeeper.Tick();

                // Assert
                forest.Verify(forest1 => forest1.GenerateLumberJack(), Times.Never);
            }

            [Test]
            public static void then_should_remove_a_lumberjack_if_lumber_less_than_lumberjack_count()
            {
                // Arrange
                var timeKeeper = new TimeKeeper();

                var forest = new Mock<IForest>();
                forest.Setup(forest1 => forest1.ForestObjects).Returns(new List<IForestObject>());
                forest.Setup(forest1 => forest1.Lumber).Returns(2);
                var lumberJack = new Mock<ILumberJack>();
                forest.Setup(forest1 => forest1.LumberJacks)
                    .Returns(new List<ILumberJack>()
                    {
                        new Mock<ILumberJack>().Object,
                        new Mock<ILumberJack>().Object,
                        lumberJack.Object
                    });
                var numberGeneratorService = new Mock<INumberGeneratorService>();
                numberGeneratorService.Setup(service => service.GetNextRandomOfBound(It.IsAny<int>())).Returns(2);
                timeKeeper.StartTrackingForest(forest.Object, numberGeneratorService.Object);
                for (var i = 0; i < 11; i++)
                {
                    timeKeeper.Tick();
                }

                // Act
                timeKeeper.Tick();

                // Assert
                forest.Verify(forest1 => forest1.Remove(lumberJack.Object), Times.Once);
            }

            [Test]
            public static void
                then_should_hire_one_new_lumberjacks_if_lumber_greater_than_lumberjack_count_but_less_than_nineteen()
            {
                // Arrange
                var timeKeeper = new TimeKeeper();

                var forest = new Mock<IForest>();
                forest.Setup(forest1 => forest1.ForestObjects).Returns(new List<IForestObject>());
                forest.Setup(forest1 => forest1.Lumber).Returns(3);
                forest.Setup(forest1 => forest1.LumberJacks)
                    .Returns(new List<ILumberJack>()
                    {
                        new Mock<ILumberJack>().Object,
                        new Mock<ILumberJack>().Object
                    });
                timeKeeper.StartTrackingForest(forest.Object);
                for (var i = 0; i < 11; i++)
                {
                    timeKeeper.Tick();
                }

                // Act
                timeKeeper.Tick();

                // Assert
                forest.Verify(forest1 => forest1.GenerateLumberJack(), Times.Once);
            }

            [Test]
            public static void
                then_should_hire_two_lumberjacks_if_lumber_greater_than_lumberjack_count_and_lumber_greater_than_ten_but_less_than_nine_teen
                ()
            {
                // Arrange
                var timeKeeper = new TimeKeeper();

                var forest = new Mock<IForest>();
                forest.Setup(forest1 => forest1.ForestObjects).Returns(new List<IForestObject>());
                forest.Setup(forest1 => forest1.Lumber).Returns(13);
                forest.Setup(forest1 => forest1.LumberJacks)
                    .Returns(new List<ILumberJack>()
                    {
                        new Mock<ILumberJack>().Object,
                        new Mock<ILumberJack>().Object
                    });
                timeKeeper.StartTrackingForest(forest.Object);
                for (var i = 0; i < 11; i++)
                {
                    timeKeeper.Tick();
                }

                // Act
                timeKeeper.Tick();

                // Assert
                forest.Verify(forest1 => forest1.GenerateLumberJack(), Times.Exactly(1));
            }

            [Test]
            public static void
                then_should_hire_three_lumberjacks_if_lumber_greater_than_lumberjack_count_and_lumber_greater_than_twenty_but_less_than_or_equal_to_twenty_nine
                ()
            {
                // Arrange
                var timeKeeper = new TimeKeeper();

                var forest = new Mock<IForest>();
                forest.Setup(forest1 => forest1.ForestObjects).Returns(new List<IForestObject>());
                forest.Setup(forest1 => forest1.Lumber).Returns(23);
                forest.Setup(forest1 => forest1.LumberJacks)
                    .Returns(new List<ILumberJack>()
                    {
                        new Mock<ILumberJack>().Object,
                        new Mock<ILumberJack>().Object
                    });
                timeKeeper.StartTrackingForest(forest.Object);
                for (var i = 0; i < 11; i++)
                {
                    timeKeeper.Tick();
                }

                // Act
                timeKeeper.Tick();

                // Assert
                forest.Verify(forest1 => forest1.GenerateLumberJack(), Times.Exactly(2));
            }

            [Test]
            public static void
                then_should_hire_three_lumberjacks_if_lumber_greater_than_lumberjack_count_and_lumber_greater_than_thirty_but_less_than_or_equal_to_thirty_nine
                ()
            {
                // Arrange
                var timeKeeper = new TimeKeeper();

                var forest = new Mock<IForest>();
                forest.Setup(forest1 => forest1.ForestObjects).Returns(new List<IForestObject>());
                forest.Setup(forest1 => forest1.Lumber).Returns(33);
                forest.Setup(forest1 => forest1.LumberJacks)
                    .Returns(new List<ILumberJack>()
                    {
                        new Mock<ILumberJack>().Object,
                        new Mock<ILumberJack>().Object
                    });
                timeKeeper.StartTrackingForest(forest.Object);
                for (var i = 0; i < 11; i++)
                {
                    timeKeeper.Tick();
                }

                // Act
                timeKeeper.Tick();

                // Assert
                forest.Verify(forest1 => forest1.GenerateLumberJack(), Times.Exactly(3));
            }

            [Test]
            public static void
                then_should_hire_three_lumberjacks_if_lumber_greater_than_lumberjack_count_and_lumber_greater_than_ninety_but_less_than_or_equal_to_ninety_nine
                ()
            {
                // Arrange
                var timeKeeper = new TimeKeeper();

                var forest = new Mock<IForest>();
                forest.Setup(forest1 => forest1.ForestObjects).Returns(new List<IForestObject>());
                forest.Setup(forest1 => forest1.Lumber).Returns(97);
                forest.Setup(forest1 => forest1.LumberJacks)
                    .Returns(new List<ILumberJack>()
                    {
                        new Mock<ILumberJack>().Object,
                        new Mock<ILumberJack>().Object
                    });
                timeKeeper.StartTrackingForest(forest.Object);
                for (var i = 0; i < 11; i++)
                {
                    timeKeeper.Tick();
                }

                // Act
                timeKeeper.Tick();

                // Assert
                forest.Verify(forest1 => forest1.GenerateLumberJack(), Times.Exactly(9));
            }

            [Test]
            public static void then_should_not_remove_the_final_lumberjack()
            {
                // Arrange
                var timeKeeper = new TimeKeeper();

                var forest = new Mock<IForest>();
                forest.Setup(forest1 => forest1.ForestObjects).Returns(new List<IForestObject>());
                forest.Setup(forest1 => forest1.Lumber).Returns(0);
                var lumberJack = new Mock<ILumberJack>();
                forest.Setup(forest1 => forest1.LumberJacks)
                    .Returns(new List<ILumberJack>()
                    {
                        lumberJack.Object
                    });
                timeKeeper.StartTrackingForest(forest.Object);
                for (var i = 0; i < 11; i++)
                {
                    timeKeeper.Tick();
                }

                // Act
                timeKeeper.Tick();

                // Assert
                forest.Verify(forest1 => forest1.Remove(lumberJack.Object), Times.Never);
            }
        }

        [TestFixture]
        public static class when_tick_and_maw_tracking
        {
            [Test]
            public static void then_should_not_add_bear_if_there_is_a_mawling()
            {
                // Arrange
                var timeKeeper = new TimeKeeper();
                timeKeeper.PreviousYearsMaulingCount = 1;

                var forest = new Mock<IForest>();
                var lumberJack = new Mock<ILumberJack>();
                forest.Setup(forest1 => forest1.ForestObjects).Returns(new List<IForestObject>());
                forest.Setup(forest1 => forest1.LumberJacks)
                    .Returns(new List<ILumberJack>
                    {
                        lumberJack.Object
                    });
                forest.Setup(forest1 => forest1.Bears).Returns(new List<IBear>
                {
                    new Mock<IBear>().Object
                });
                forest.Setup(forest1 => forest1.Maulings).Returns(2);
                timeKeeper.StartTrackingForest(forest.Object);
                for (var i = 0; i < 11; i++)
                {
                    timeKeeper.Tick();
                }

                // Act
                timeKeeper.Tick();

                // Assert
                forest.Verify(forest1 => forest1.GenerateBear(), Times.Never);
            }
        }
    }
}
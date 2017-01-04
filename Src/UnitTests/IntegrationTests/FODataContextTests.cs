using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using FishOn.PlatformInterfaces;
using FishOn.Repositories;
using FishOn.Services;
using FishOn.Utils;
using Moq;
using NUnit.Framework;

namespace UnitTests.IntegrationTests
{
    /*
    public class FishOnDataContextTests
    {
        FishOnDataContext _db;

        [SetUp]
        public void Initialize()
        {
            var platform = new SQLite.Net.Platform.Generic.SQLitePlatformGeneric();
            var fileReaderMock = new Mock<IFileHelper>();
            fileReaderMock.Setup(m => m.GetLocalFilePath(It.IsAny<string>())).Returns(@":memory");
            _db = new FishOnDataContext(platform, fileReaderMock.Object);
           
        }

        [Test]
        public void DbCreated_SpeciesSeeded()
        {
            var speciesList = _db.GetSpecies();
            Assert.IsTrue(speciesList.Count > 0);
        }

        [Test]
        public void Reorder_Species()
        {
            var speciesList = _db.GetSpecies();
            var firstSpecies = speciesList[0].Name;
            var secondSpecies = speciesList[1].Name;

            speciesList.MoveDown(speciesList[0]);

            Assert.AreEqual(speciesList[1], firstSpecies);
            Assert.AreEqual(speciesList[0], secondSpecies);

            _db.SaveSpecies(speciesList);
            speciesList = _db.GetSpecies();

            Assert.AreEqual(speciesList[1], firstSpecies);
            Assert.AreEqual(speciesList[0], secondSpecies);

        }

        [Test]
        public void FishingLureCreated()
        {
            
        }

        [Test]
        public void FishingLureUpdated()
        {
            
        }
        
        [Test]
        public void WayPointCreated()
        {
            
        }

        [Test]
        public void FishOn_ExistingWayPoint()
        {
            
        }
    }
    */
}

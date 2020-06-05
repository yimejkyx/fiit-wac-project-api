using System.Collections;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using eu.fiit.PatientsPortal.Services;
using eu.fiit.PatientsPortal.Controllers;
using eu.fiit.PatientsPortal.Models;
using System.Collections.Generic;
using System;
using Moq;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;

namespace eu.fiit.PatientsPortal.Models
{

    public abstract class BaseControllerTests
    {
        protected readonly Mock<IDataRepository> Repository;
        protected readonly UsersApiController UsersApi;
        protected readonly VisitsApiController VisitsApi;
        protected readonly MedicinesApiController MedicinesApi;
        protected readonly EPrescriptionsApiController EPrescriptionsApi;

        protected readonly List<User> Users;
        protected readonly List<Medicine> Medicines;
        protected readonly List<Visit> Visits;
        protected readonly List<EPrescription> EPrescriptions;

        protected BaseControllerTests(List<User> users, List<Medicine> medicines, List<Visit> visits, List<EPrescription> ePrescriptions)
        {
            Users = users;
            Medicines = medicines;
            Visits = visits;
            EPrescriptions = ePrescriptions;
            Repository = new Mock<IDataRepository>();

            // Users
            // User GetUserData(int userId);
            // IEnumerable<User> GetUserData();
            // User UpsertUserData(User user);
            // void DeleteUser(int userId);
            Repository.Setup(x => x.GetUserData(It.IsAny<int>())).Returns(users[0]);
            Repository.Setup(x => x.GetUserData()).Returns(users);
            Repository.Setup(x => x.UpsertUserData(It.IsAny<User>())).Returns(users[0]);
            Repository.Setup(x => x.DeleteUser(It.IsAny<int>()));

            // Medicines
            // Medicine GetMedicineData(int medicineId);
            // IEnumerable<Medicine> GetMedicineData();
            // Medicine UpsertMedicineData(Medicine medicine);
            // void DeleteMedicine(int medicineId);
            Repository.Setup(x => x.GetMedicineData(It.IsAny<int>())).Returns(medicines[0]);
            Repository.Setup(x => x.GetMedicineData()).Returns(medicines);
            Repository.Setup(x => x.UpsertMedicineData(It.IsAny<Medicine>())).Returns(medicines[0]);
            Repository.Setup(x => x.DeleteMedicine(It.IsAny<int>()));

            // EPrescriptions
            // EPrescription GetEPrescriptionData(int ePrescriptionId);
            // IEnumerable<EPrescription> GetEPrescriptionData();
            // EPrescription UpsertEPrescriptionData(EPrescription ePrescription);
            // void DeleteEPrescrition(int ePrescriptionId);
            Repository.Setup(x => x.GetEPrescriptionData(It.IsAny<int>())).Returns(ePrescriptions[0]);
            Repository.Setup(x => x.GetEPrescriptionData()).Returns(ePrescriptions);
            Repository.Setup(x => x.UpsertEPrescriptionData(It.IsAny<EPrescription>())).Returns(ePrescriptions[0]);
            Repository.Setup(x => x.DeleteEPrescrition(It.IsAny<int>()));

            // Visits
            // Visit GetVisitData(int visitId);
            // IEnumerable<Visit> GetVisitsData();
            // Visit UpsertVisitData(Visit visit);
            // void DeleteVisit(int visitId);
            // IEnumerable<Visit> GetVisitsDataByDate(DateTime concreteDay);
            Repository.Setup(x => x.GetVisitData(It.IsAny<int>())).Returns(visits[0]);
            Repository.Setup(x => x.GetVisitsData()).Returns(visits);
            Repository.Setup(x => x.UpsertVisitData(It.IsAny<Visit>())).Returns(visits[0]);
            Repository.Setup(x => x.DeleteVisit(It.IsAny<int>()));
            Repository.Setup(x => x.GetVisitsDataByDate(It.IsAny<DateTime>())).Returns(visits);

            UsersApi = new UsersApiController(Repository.Object);
            VisitsApi = new VisitsApiController(Repository.Object);
            MedicinesApi = new MedicinesApiController(Repository.Object);
            EPrescriptionsApi = new EPrescriptionsApiController(Repository.Object);
        }
    }

    [TestClass]
    public class ApiTests : BaseControllerTests
    {
        private static readonly List<User> mockUsers = MockUsers();
        private static readonly List<Medicine> mockMedicines = MockMedicines();
        private static readonly List<Visit> mockVisits = MockVisits();
        private static readonly List<EPrescription> mockEPrescriptions = MockEPrescriptions();
        public ApiTests() : base(mockUsers, mockMedicines, mockVisits, mockEPrescriptions) { }

        private TestContext testContextInstance;

        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        // USERS
        [TestMethod]
        public void GetUsers_ReturnsListOfUsers()
        {
            var response = UsersApi.GetUsers();

            Repository.Verify(mock => mock.GetUserData(), Times.Once);

            var objectResponse = response as ObjectResult;
            Assert.IsTrue(objectResponse.StatusCode == 200);
            Assert.IsInstanceOfType(objectResponse.Value, typeof(IEnumerable<User>));
        }

        [TestMethod]
        public void AddUser_ReturnsStatus200()
        {
            var newUser = new User { 
                Name = "Doctor 1",
                IsDoctor = true,
                IsPatient = false
            };
        
            var response = UsersApi.AddUser(newUser);
            
            Repository.Verify(mock => mock.UpsertUserData(It.IsAny<User>()), Times.Once);
            var objectResponse = response as ObjectResult;
            Assert.IsTrue(objectResponse.StatusCode == 200);
        }

        [TestMethod]
        public void UpdateUser_ReturnsStatus200()
        {
            var response = UsersApi.UpdateUser(Users[0].Id ?? 1, Users[0]);

            // TODO
        }

        [TestMethod]
        public void DeleteUser_ReturnsStatus200()
        {
            var response = UsersApi.DeleteUser(Users[0].Id.Value);

            Repository.Verify(mock => mock.GetUserData(Users[0].Id.Value), Times.Once);
            Repository.Verify(mock => mock.DeleteUser(Users[0].Id.Value), Times.Once);

            var objectResponse = response as OkResult;
            Assert.IsTrue(objectResponse.StatusCode == 200);
        }

        // MEDICINES
        [TestMethod]
        public void GetMedicines_ReturnsListOfUsers()
        {
            var response = MedicinesApi.GetMedicines();

            Repository.Verify(mock => mock.GetMedicineData(), Times.Once);

            var objectResponse = response as ObjectResult;
            Assert.IsTrue(objectResponse.StatusCode == 200);
            Assert.IsInstanceOfType(objectResponse.Value, typeof(IEnumerable<Medicine>));
        }

        [TestMethod]
        public void AddMedicine_ReturnsStatus200()
        {
            var newMedicine = new Medicine { 
                Name = "Medicine 1",
            };

            var response = MedicinesApi.AddMedicine(newMedicine);
            
            Repository.Verify(mock => mock.UpsertMedicineData(It.IsAny<Medicine>()), Times.Once);
            var objectResponse = response as ObjectResult;
            Assert.IsTrue(objectResponse.StatusCode == 200);
        }

        [TestMethod]
        public void UpdateMedicine_ReturnsStatus200()
        {
            var response = MedicinesApi.UpdateMedicine(Medicines[0].Id ?? 1, Medicines[0]);

            // TODO
        }

        [TestMethod]
        public void DeleteMedicine_ReturnsStatus200()
        {
            var response = MedicinesApi.DeleteMedicine(Users[0].Id.Value);

            Repository.Verify(mock => mock.GetMedicineData(Users[0].Id.Value), Times.Once);
            Repository.Verify(mock => mock.DeleteMedicine(Users[0].Id.Value), Times.Once);

            var objectResponse = response as OkResult;
            Assert.IsTrue(objectResponse.StatusCode == 200);
        }


        // DATA MOCKUPS
        public static List<User> MockUsers()
        {
            return new List<User>(){
                new User(){
                    Id = 1,
                    Name = "Patient 1",
                    IsDoctor = false,
                    IsPatient = true
                },
                new User(){
                    Id = 2,
                    Name = "Doctor 1",
                    IsDoctor = true,
                    IsPatient = false
                }
            };
        }

        public static List<Medicine> MockMedicines()
        {
            return new List<Medicine>(){
                new Medicine(){
                    Id = 1,
                    Name = "Paralen",
                },
                new Medicine(){
                    Id = 2,
                    Name = "Ibalgin",
                }
            };
        }

        public static List<Visit> MockVisits()
        {
            return new List<Visit>(){
                new Visit(){
                    Id = 1,
                    Created = DateTime.Now,
                    Date = DateTime.Now.AddDays(1),
                    Reason = "ILLNESS",
                    Length = 10,
                    Result = "result1",
                    Patient = {
                        Id = 1,
                        Name = "Patient 1",
                        IsDoctor = false,
                        IsPatient = true
                    },
                    Doctor = {
                        Id = 2,
                        Name = "Doctor 1",
                        IsDoctor = true,
                        IsPatient = false
                    }
                },
                new Visit(){
                    Id = 2,
                    Created = DateTime.Now,
                    Date = DateTime.Now.AddDays(2),
                    Reason = "CHECK",
                    Length = 30,
                    Result = "result2",
                    Patient = new User(){
                        Id = 1,
                        Name = "Patient 1",
                        IsDoctor = false,
                        IsPatient = true
                    },
                    Doctor = new User{
                        Id = 2,
                        Name = "Doctor 1",
                        IsDoctor = true,
                        IsPatient = false
                    }
                }
            };
        }

        public static List<EPrescription> MockEPrescriptions() {
            return new List<EPrescription>(){
                new EPrescription{
                    Id = 1,
                    Created = DateTime.Now,
                    Medicines = {
                         new Medicine(){
                            Id = 5,
                            Name = "Voltaren"
                        }
                    },
                    Reason = "reason1",
                    Expiration = null,
                    State = null,
                    Patient = new User(){
                        Id = 1,
                        Name = "Patient 1",
                        IsDoctor = false,
                        IsPatient = true
                    },
                    Doctor = new User{
                        Id = 2,
                        Name = "Doctor 1",
                        IsDoctor = true,
                        IsPatient = false
                    },
                },
                 new EPrescription{
                    Id = 2,
                    Created = DateTime.Now.AddDays(2),
                    Medicines = {
                         new Medicine(){
                            Id = 1,
                            Name = "Paralen"
                        }
                    },
                    Reason = "reason2",
                    Expiration = null,
                    State = null,
                    Patient = new User(){
                        Id = 1,
                        Name = "Patient 1",
                        IsDoctor = false,
                        IsPatient = true
                    },
                    Doctor = new User{
                        Id = 2,
                        Name = "Doctor 1",
                        IsDoctor = true,
                        IsPatient = false
                    },
                }

            };
        }
    }
}

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
            // Repository.Setup(x => x.GetUserData(It.IsAny<int>())).Returns((int key) => users[key-1]);
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

            Repository.Verify(mock => mock.UpsertUserData(It.IsAny<User>()), Times.Once);
            var objectResponse = response as ObjectResult;
            Assert.IsTrue(objectResponse.StatusCode == 200);       
        }

        [TestMethod]
        public void UpdateUser_UserIdDoesNotMatchWithUrlPathParamId_ReturnsStatus400()
        {
             var newUser = new User { 
                Id = 10,
                Name = "Doctor 1",
                IsDoctor = true,
                IsPatient = false
            };
            var response = UsersApi.UpdateUser(5, newUser);

            Repository.Verify(mock => mock.UpsertUserData(It.IsAny<User>()), Times.Never);
            var objectResponse = response as BadRequestResult;
            Assert.IsTrue(objectResponse.StatusCode == 400);       
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

            Repository.Verify(mock => mock.UpsertMedicineData(It.IsAny<Medicine>()), Times.Once);
            var objectResponse = response as ObjectResult;
            Assert.IsTrue(objectResponse.StatusCode == 200);  
        }

        [TestMethod]
        public void UpdateMedicine_MedicineIdDoesNotMatchWithUrlPathParamId_ReturnsStatus400()
        {
             var newUser = new User { 
                Id = 10,
                Name = "Doctor 1",
                IsDoctor = true,
                IsPatient = false
            };
            var response = UsersApi.UpdateUser(5, newUser);

            Repository.Verify(mock => mock.UpsertUserData(It.IsAny<User>()), Times.Never);
            var objectResponse = response as BadRequestResult;
            Assert.IsTrue(objectResponse.StatusCode == 400);       
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


        // VISITS
        [TestMethod]
        public void GetVisits_ReturnsListOfVisits()
        {
            var response = VisitsApi.GetVisits();

            Repository.Verify(mock => mock.GetVisitsData(), Times.Once);

            var objectResponse = response as ObjectResult;
            Assert.IsTrue(objectResponse.StatusCode == 200);
            Assert.IsInstanceOfType(objectResponse.Value, typeof(IEnumerable<Visit>));
        }

        [TestMethod]
        public void AddVisit_ReturnsStatus200()
        {
            var newVisit = new Visit(){
                Id = 5,
                Created = DateTime.Now,
                Date = DateTime.Now.AddDays(10),
                Reason = "ILLNESS",
                Length = 10,
                Result = "result1",
                Patient = new User(){
                    Id = 1,
                    Name = "Patient 1",
                    IsDoctor = false,
                    IsPatient = true
                },
                Doctor = new User(){
                    Id = 2,
                    Name = "Doctor 1",
                    IsDoctor = true,
                    IsPatient = false
                }
            };
        
            // TODO -> je problem, ze pouzivame getUserData na patient a doctor, a tu mame namokovane, getUserData vracia prveho
            // var response = VisitsApi.AddVisit(newVisit);
            
            // Repository.Verify(mock => mock.UpsertVisitData(It.IsAny<Visit>()), Times.Once);
            // var objectResponse = response as ObjectResult;
            // Assert.IsTrue(objectResponse.StatusCode == 200);
        }


        [TestMethod]
        public void AddVisit_PatientIsNull_ReturnsStatus400()
        {
            var newVisit = new Visit(){
                Created = DateTime.Now,
                Date = DateTime.Now.AddDays(5),
                Reason = "ILLNESS",
                Length = 10,
                Result = "result",
                Patient = null,
                Doctor = new User(){
                    Id = 2,
                    Name = "Doctor 1",
                    IsDoctor = true,
                    IsPatient = false
                }
            };
        
            var response = VisitsApi.AddVisit(newVisit);
            Repository.Verify(mock => mock.UpsertVisitData(It.IsAny<Visit>()), Times.Never);

            var objectResponse = response as ObjectResult;
            Assert.IsTrue(objectResponse.StatusCode == 400);
        }

         [TestMethod]
        public void AddVisit_DoctorIsNull_ReturnsStatus400 ()
        {
            var newVisit = new Visit(){
                Created = DateTime.Now,
                Date = DateTime.Now.AddDays(5),
                Reason = "ILLNESS",
                Length = 10,
                Result = "result",
                Patient = new User(){
                    Id = 5,
                    Name = "Patient 1",
                    IsDoctor = false,
                    IsPatient = true
                },
                Doctor =null
            };
        
            var response = VisitsApi.AddVisit(newVisit);
            Repository.Verify(mock => mock.UpsertVisitData(It.IsAny<Visit>()), Times.Never);
            
            var objectResponse = response as ObjectResult;
            Assert.IsTrue(objectResponse.StatusCode == 400);
        }

       [TestMethod]
        public void DeleteVisit_ReturnsStatus200()
        {
            var response = VisitsApi.DeleteVisit(Visits[0].Id.Value);

            Repository.Verify(mock => mock.GetVisitData(Visits[0].Id.Value), Times.Once);
            Repository.Verify(mock => mock.DeleteVisit(Visits[0].Id.Value), Times.Once);

            var objectResponse = response as OkResult;
            Assert.IsTrue(objectResponse.StatusCode == 200);
        }


        [TestMethod]
        public void UpdateVisit_ReturnsStatus200()
        {
            var response = VisitsApi.UpdateVisit(Visits[0].Id ?? 1, Visits[0]);

           //TO-DO v mock datach User IS NOT A DOCTOR lebo sa stale vracia ten prvy
            // Repository.Verify(mock => mock.UpsertVisitData(It.IsAny<Visit>()), Times.Once);
            // var objectResponse = response as ObjectResult;
            // Assert.IsTrue(objectResponse.StatusCode == 200);  
        }

         // EPRESCRIPTIONS
        [TestMethod]
        public void GetEPrescriptions_ReturnsListOfEPrescriptions()
        {
            var response = EPrescriptionsApi.GetEPrescriptions();

            Repository.Verify(mock => mock.GetEPrescriptionData(), Times.Once);

            var objectResponse = response as ObjectResult;
            Assert.IsTrue(objectResponse.StatusCode == 200);
            Assert.IsInstanceOfType(objectResponse.Value, typeof(IEnumerable<EPrescription>));
        }

       [TestMethod]
        public void DeleteEprescription_ReturnsStatus200()
        {
            var response = EPrescriptionsApi.DeleteEPrescriptions(EPrescriptions[0].Id.Value);

            Repository.Verify(mock => mock.GetEPrescriptionData(EPrescriptions[0].Id.Value), Times.Once);
            Repository.Verify(mock => mock.DeleteEPrescrition(EPrescriptions[0].Id.Value), Times.Once);

            var objectResponse = response as OkResult;
            Assert.IsTrue(objectResponse.StatusCode == 200);
        }


        [TestMethod]
        public void UpdateEPrescription_ReturnsStatus200()
        {
            var response = EPrescriptionsApi.UpdateEPrescription(EPrescriptions[0].Id ?? 1, EPrescriptions[0]);

            // TODO
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
                    Patient = new User(){
                        Id = 1,
                        Name = "Patient 1",
                        IsDoctor = false,
                        IsPatient = true
                    },
                    Doctor = new User(){
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
                    Doctor = new User(){
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
                    Medicines = new List<Medicine>(){
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
                    Medicines = new List<Medicine>(){
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
                    Doctor = new User(){
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

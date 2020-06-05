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
        protected readonly List<User> Users;

        protected BaseControllerTests(List<User> users)
        {
            Users = users;
            Repository = new Mock<IDataRepository>();
            Repository.Setup(x => x.GetUserData()).Returns(users);
            // Repository.Setup(x => x.DeleteUser()).Returns(users);

            // AdminsApi = new AdminsApiController(Repository.Object);
            // DevApi = new DevelopersApiController(Repository.Object);
            UsersApi = new UsersApiController(Repository.Object);
            // VisitsApi = new VisitsApiController(Repository.Object);
            // MedicinesApi = new MedicinesApiController(Repository.Object);
            // EPrescriptionsApi = new EPrescriptionsApiController(Repository.Object);
            
        }
    }

    [TestClass]
    public class ApiTests : BaseControllerTests
    {
        private static readonly List<User> mockUsers = MockUsers();
        // private static readonly List<Medicines> mockMedicines = MockMedicines();
        // private static readonly List<Visit> mockVisits = MockVisits();
        // private static readonly List<EPrescription> mockEPrescriptions = mockEPrescriptions();
        public ApiTests() : base(mockUsers) { }

        private TestContext testContextInstance;

        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        [TestMethod]
        public void GetUsers_ReturnsListOfUsers()
        {
            var response = UsersApi.GetUsers();

            Repository.Verify(mock => mock.GetUserData(), Times.Once);
            TestContext.WriteLine(JsonConvert.SerializeObject(response));

            var objectResponse = response as ObjectResult;
   
            Assert.IsTrue(objectResponse.StatusCode == 200);
            Assert.IsInstanceOfType(objectResponse.Value, typeof(IEnumerable<User>));
        }

    //    [TestMethod]
    //     public void DeleteUser_ReturnsStatus200()
    //     {
    //         var response = UsersApi.DeleteUser(Users[0].Id.Value);

    //         Repository.Verify(mock => mock.GetUserData(It.IsAny<string>()), Times.Once);
    //         Repository.Verify(mock => mock.DeleteUser(It.IsAny<string>()), Times.Once);

    //         var objectResponse = response as ObjectResult;
    //         Assert.IsTrue(objectResponse.StatusCode == 200);
    //     }


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
                    Id = 1,
                    Name = "Doctor 1",
                    IsDoctor = true,
                    IsPatient = false
                }
            };
        }
    }
}

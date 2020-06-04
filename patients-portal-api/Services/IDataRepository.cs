using System;
using eu.fiit.PatientsPortal.Models;
using System.Collections.Generic;
using LiteDB;
namespace eu.fiit.PatientsPortal.Services
{
    /// <summary>
    /// Abstraction of the data repository keeping data model persistent
    ///
    /// Responsibilities:
    ///     * CRUD operations over data maodel
    ///     * Searching and filtering durring data retrieval
    /// </summary> 

    public interface IDataRepository
    {
        /// <summary>
        /// Provides details about specific visit
        /// </summary>
        /// <param name="visitId">id of the visit</param>
        /// <returns>visit details</returns>
        Visit GetVisitData(int visitId);

        /// <summary>
        /// Provides details about all visits
        /// </summary>
        /// <returns>visits list</returns>
        IEnumerable<Visit> GetVisitsData();

        /// <summary>
        /// Updates or inserts details about specific/new visit
        /// </summary>
        /// <param name="visit">visit data</param>
        /// <returns>visit instance with correct id, if inserting</returns>
        Visit UpsertVisitData(Visit visit);

        /// <summary>
        /// Deletes details about specific visit
        /// </summary>
        /// <param name="visitId">id of the visit</param>
        void DeleteVisit(int visitId);

        /// <summary>
        /// Provides details about all visits for a concrete Date
        /// </summary>
        /// <returns>visits list</returns>
        IEnumerable<Visit> GetVisitsDataByDate(DateTime concreteDay);

        /// <summary>
        /// Provides details about specific ePrescription
        /// </summary>
        /// <param name="ePrescriptionId">id of the ePrescription</param>
        /// <returns>ePrescription details</returns>
        EPrescription GetEPrescriptionData(int ePrescriptionId);

        /// <summary>
        /// Provides details about all ePrescriptions
        /// </summary>
        /// <returns>ePrescriptions list</returns>
        IEnumerable<EPrescription> GetEPrescriptionData();

        /// <summary>
        /// Updates or inserts details about specific/new ePrescription
        /// </summary>
        /// <param name="ePrescription">ePrescription data</param>
        /// <returns>ePrescription instance with correct id, if inserting</returns>
        EPrescription UpsertEPrescriptionData(EPrescription ePrescription);

        /// <summary>
        /// Deletes details about specific ePrescription
        /// </summary>
        /// <param name="ePrescriptionId">id of the ePrescription</param>
        void DeleteEPrescrition(int ePrescriptionId);

        /// <summary>
        /// Provides details about specific user
        /// </summary>
        /// <param name="userId">id of the user</param>
        /// <returns>user details</returns>
        User GetUserData(int userId);

        /// <summary>
        /// Provides details about all users
        /// </summary>
        /// <returns>users details</returns>
        IEnumerable<User> GetUserData();

        /// <summary>
        /// Updates or inserts details about specific/new user
        /// </summary>
        /// <param name="user">user data</param>
        /// <returns>user instance with correct id, if inserting</returns>
        User UpsertUserData(User user);

        /// <summary>
        /// Deletes details about specific user
        /// </summary>
        /// <param name="userId">id of the user</param>
        void DeleteUser(int userId);

        /// <summary>
        /// Provides details about specific medicine
        /// </summary>
        /// <param name="medicineId">id of the medicine</param>
        /// <returns>medicine details</returns>
        Medicine GetMedicineData(int medicineId);

        /// <summary>
        /// Provides details about all medicines
        /// </summary>
        /// <returns>medicines list</returns>
        IEnumerable<Medicine> GetMedicineData();

        /// <summary>
        /// Updates or inserts details about specific/new medicine
        /// </summary>
        /// <param name="medicine">medicine data</param>
        /// <returns>medicine instance with correct id, if inserting</returns>
        Medicine UpsertMedicineData(Medicine medicine);

        /// <summary>
        /// Deletes details about specific medicine
        /// </summary>
        /// <param name="medicineId">id of the medicine</param>
        void DeleteMedicine(int medicineId);
    }
}
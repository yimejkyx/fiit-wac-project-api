using System;
using System.IO;
using eu.fiit.PatientsPortal.Models;
using LiteDB;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace eu.fiit.PatientsPortal.Services
{
    class DataRepository : IDataRepository
    {
        private readonly LiteDatabase liteDb;
        private static readonly string VISITS_COLLECTION = "visits";
        private static readonly string EPRESCRIPTIONS_COLLECTION = "eprescriptions";
        private static readonly string MEDICINES_COLLECTION = "medicines";
        private static readonly string USERS_COLLECTION = "users";

        public DataRepository(
            IHostEnvironment environment, IConfiguration configuration)
        {
            string dbPath = configuration["DATA_REPOSITORY_DB"];
            if (dbPath == null || dbPath.Length == 0)
            {
                dbPath = Path.Combine(
                    environment.ContentRootPath, "data", "data-repository.litedb");
            }
            this.liteDb = new LiteDatabase(dbPath);
        }

        public Visit GetVisitData(int visitId)
        {
            var collection = this.liteDb.GetCollection<Visit>(VISITS_COLLECTION);
            return collection
                .Include(x => x.Doctor)
                .Include(x => x.Patient)
                .FindById(visitId);
        }

        public IEnumerable<Visit> GetVisitsData()
        {
            var collection = this.liteDb.GetCollection<Visit>(VISITS_COLLECTION);
            return collection
                .Include(x => x.Doctor)
                .Include(x => x.Patient)
                .FindAll();
        }

        public Visit UpsertVisitData(Visit visit)
        {
            var collection = this.liteDb.GetCollection<Visit>(VISITS_COLLECTION);
            if (visit.Id == null)
            {
                visit.Id = 0;
                var idValue = collection.Insert(visit);
                visit.Id = idValue.AsInt32;
            }
            else
            {
                var existing = collection.FindById(visit.Id);
                if (existing != null)
                {
                    collection.Update(visit);
                }
            }
            return GetVisitData(visit.Id ?? default(int)); // id always exists
        }

        public void DeleteVisit(int visitId)
        {
            var collection = this.liteDb.GetCollection<Visit>(VISITS_COLLECTION);
            collection.Delete(visitId);
        }

        public IEnumerable<Visit> GetVisitsDataByDate(DateTime concreteDay)
        {
            var collection = this.liteDb.GetCollection<Visit>(VISITS_COLLECTION);
            return collection
            .Include(x => x.Doctor)
            .Include(x => x.Patient)
            .Find(x => x.Date.Value.Date == concreteDay.Date);
        }

        public EPrescription GetEPrescriptionData(int ePrescriptionId)
        {
            var collection = this.liteDb.GetCollection<EPrescription>(EPRESCRIPTIONS_COLLECTION);
            return collection
                .Include(x => x.Doctor)
                .Include(x => x.Patient)
                .Include(x => x.Medicines)
                .FindById(ePrescriptionId);
        }

        public IEnumerable<EPrescription> GetEPrescriptionData()
        {
            var collection = this.liteDb.GetCollection<EPrescription>(EPRESCRIPTIONS_COLLECTION);
            var result = collection
                .Include(x => x.Doctor)
                .Include(x => x.Patient)
                .Include(x => x.Medicines)
                .FindAll();
            return result;
        }

        public EPrescription UpsertEPrescriptionData(EPrescription ePrescription)
        {
            var collection = this.liteDb.GetCollection<EPrescription>(EPRESCRIPTIONS_COLLECTION);
            if (ePrescription.Id == null)
            {
                ePrescription.Id = 0;
                var idValue = collection.Insert(ePrescription);
                ePrescription.Id = idValue.AsInt32;
            }
            else
            {
                var existing = collection.FindById(ePrescription.Id);
                if (existing != null)
                {
                    collection.Update(ePrescription);
                }
            }
            return GetEPrescriptionData(ePrescription.Id ?? default(int)); // id always exists
        }

        public void DeleteEPrescrition(int ePrescriptionId)
        {
            var collection = this.liteDb.GetCollection<EPrescription>(EPRESCRIPTIONS_COLLECTION);
            collection.Delete(ePrescriptionId);
        }

        public User GetUserData(int userId)
        {
            var collection = this.liteDb.GetCollection<User>(USERS_COLLECTION);
            return collection.FindById(userId);
        }

        public IEnumerable<User> GetUserData()
        {
            var collection = this.liteDb.GetCollection<User>(USERS_COLLECTION);
            var result = collection.FindAll();
            return result;
        }

        public User UpsertUserData(User user)
        {
            var collection = this.liteDb.GetCollection<User>(USERS_COLLECTION);
            if (user.Id == null)
            {
                user.Id = 0;
                var idValue = collection.Insert(user);
                user.Id = idValue.AsInt32;
            }
            else
            {
                var existing = collection.FindById(user.Id);
                if (existing != null)
                {
                    collection.Update(user);
                }
            }
            return user;
        }

        public void DeleteUser(int userId)
        {
            var collection = this.liteDb.GetCollection<User>(USERS_COLLECTION);
            collection.Delete(userId);
        }

        public Medicine GetMedicineData(int medicineId)
        {
            var collection = this.liteDb.GetCollection<Medicine>(MEDICINES_COLLECTION);
            return collection.FindById(medicineId);
        }

        public IEnumerable<Medicine> GetMedicineData()
        {
            var collection = this.liteDb.GetCollection<Medicine>(MEDICINES_COLLECTION);
            var result = collection.FindAll();
            return result;
        }

        public Medicine UpsertMedicineData(Medicine medicine)
        {
            var collection = this.liteDb.GetCollection<Medicine>(MEDICINES_COLLECTION);
            if (medicine.Id == null)
            {
                medicine.Id = 0;
                var idValue = collection.Insert(medicine);
                medicine.Id = idValue.AsInt32;
            }
            else
            {
                var existing = collection.FindById(medicine.Id);
                if (existing != null)
                {
                    collection.Update(medicine);
                }
            }
            return medicine;
        }
        public void DeleteMedicine(int medicineId)
        {
            var collection = this.liteDb.GetCollection<Medicine>(MEDICINES_COLLECTION);
            collection.Delete(medicineId);
        }
    }
}
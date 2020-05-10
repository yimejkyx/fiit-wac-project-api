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

        public DataRepository(
            IHostEnvironment environment, IConfiguration configuration)
        {
            string dbPath = configuration["DATA_REPOSITORY_DB"];
            if (dbPath == null || dbPath.Length == 0)
            {
                dbPath = Path.Combine(
                    environment.ContentRootPath, "data-repository.litedb");
            }
            this.liteDb = new LiteDatabase(dbPath);
        }

        public Visit GetVisitData(int visitId)
        {
            var collection = this.liteDb.GetCollection<Visit>(VISITS_COLLECTION);
            return collection.FindById(visitId);
        }


        public IEnumerable<Visit> GetVisitsData()
        {
            var collection = this.liteDb.GetCollection<Visit>(VISITS_COLLECTION);
            return collection.FindAll();
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
                if (existing == null)
                {
                    collection.Update(visit);
                }
            }
            return visit;
        }

        public void DeleteVisit(int visitId)
        {
            var collection = this.liteDb.GetCollection<Visit>(VISITS_COLLECTION);
            collection.Delete(visitId);
        }

        public EPrescription GetEPrescriptionData(int ePrescriptionId)
        {
            var collection = this.liteDb.GetCollection<EPrescription>(EPRESCRIPTIONS_COLLECTION);
            return collection.FindById(ePrescriptionId);
        }

        public IEnumerable<EPrescription> GetEPrescriptionData()
        {
            var collection = this.liteDb.GetCollection<EPrescription>(EPRESCRIPTIONS_COLLECTION);
            var result = collection.FindAll();
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
                if (existing == null)
                {
                    collection.Update(ePrescription);
                }
            }
            return ePrescription;
        }

        public void DeleteEPrescrition(int ePrescriptionId)
        {
            var collection = this.liteDb.GetCollection<EPrescription>(EPRESCRIPTIONS_COLLECTION);
            collection.Delete(ePrescriptionId);
        }
    }
}
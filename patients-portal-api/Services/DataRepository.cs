using System;
using System.IO;
using eu.fiit.PatientsPortal.Models;
using LiteDB;
using Microsoft.AspNetCore.Hosting;
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
            IHostingEnvironment environment, IConfiguration configuration)
        {
            string dbPath = configuration["DATA_REPOSITORY_DB"];
            if (dbPath == null || dbPath.Length == 0)
            {
                dbPath = Path.Combine(
                    environment.ContentRootPath, "data-repository.litedb");
            }
            this.liteDb = new LiteDatabase(dbPath);
        }

        public Visit GetVisitData(string visitId)
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
            var existing = collection.FindById(visit.Id);
            if(existing == null)
            {
                var idValue = collection.Insert(visit);
                visit.Id = idValue.AsString;
            }
            else
            {

                collection.Update(visit);
                System.Console.WriteLine(existing.Reason);
                System.Console.WriteLine("LOG pri update:");
                System.Console.WriteLine(visit.Reason);
                System.Console.WriteLine(collection.ToString());
            }
            return visit;
        }

        public void DeleteVisit(string visitId)
        {
            var collection = this.liteDb.GetCollection<Visit>(VISITS_COLLECTION);
            collection.Delete(visitId);
        }

        public EPrescription GetEPrescriptionData(string ePrescriptionId)
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

            var idValue = collection.Insert(ePrescription);
            ePrescription.Id = idValue.AsString;
            return ePrescription;
        }

        public void DeleteEPrescrition(string ePrescriptionId)
        {
            var collection = this.liteDb.GetCollection<EPrescription>(EPRESCRIPTIONS_COLLECTION);
            collection.Delete(ePrescriptionId);
        }
    }
}
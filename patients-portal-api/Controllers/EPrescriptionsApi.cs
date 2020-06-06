/*
 * Patients Portal
 *
 * Best Medical Portal API ever
 *
 * OpenAPI spec version: 1.0.0
 * Contact: nikolas.tsk@gmail.com
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.SwaggerGen;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using eu.fiit.PatientsPortal.Attributes;

using Microsoft.AspNetCore.Authorization;
using eu.fiit.PatientsPortal.Models;
using eu.fiit.PatientsPortal.Services;

namespace eu.fiit.PatientsPortal.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    public class EPrescriptionsApiController : ControllerBase
    {
        private readonly IDataRepository repository;

        /// <summary/>
        public EPrescriptionsApiController(IDataRepository repository)
            => this.repository = repository;

        /// <summary>
        /// Add a new E-Prescriptions
        /// </summary>
        /// <param name="body">E-Prescriptions object</param>
        /// <response code="400">Invalid input</response>
        /// <response code="200">successful operation</response>
        [HttpPost]
        [Route("/api/eprescription")]
        [ValidateModelState]
        [SwaggerOperation("AddEPrescriptions")]
        public virtual IActionResult AddEPrescriptions([FromBody] EPrescription body)
        {
            if (body.Patient == null) return StatusCode(400, "Patient is null");
            if (body.Doctor == null) return StatusCode(400, "Doctor is null");
            if (!body.Doctor.Id.HasValue) return StatusCode(400, "DoctorId is null");
            if (!body.Patient.Id.HasValue) return StatusCode(400, "PatientId is null");
            var newEPrescription = body;

            var doctor = this.repository.GetUserData(newEPrescription.Doctor.Id.Value);
            if (!doctor.IsDoctor.Value) return StatusCode(400, "User is not a doctor");
            var patient = this.repository.GetUserData(newEPrescription.Patient.Id.Value);
            if (!patient.IsPatient.Value) return StatusCode(400, "User is not a patient");

            newEPrescription.Expiration = DateTime.Now.AddDays(30);
            newEPrescription.Created = DateTime.Now;
            newEPrescription.State = "PENDING";

            newEPrescription = this.repository.UpsertEPrescriptionData(newEPrescription);
            return StatusCode(200, newEPrescription);
        }

        /// <summary>
        /// Update an existing E-Prescriptions 
        /// </summary>
        /// <param name="ePrescriptionId">E-Prescriptions id to update</param>
        /// <param name="body">E-Prescriptions  object</param>
        /// <response code="400">Invalid input</response>
        /// <response code="200">successful operation</response>
        [HttpPut]
        [Route("/api/eprescription/{ePrescriptionId}")]
        [ValidateModelState]
        [SwaggerOperation("UpdateEPrescription")]
        public virtual IActionResult UpdateEPrescription([FromRoute][Required] int ePrescriptionId, [FromBody] EPrescription body)
        {
            if (!ePrescriptionId.Equals(body.Id)) { return StatusCode(400, "Eprescription.Id from Path does not correspond with Eprescription.Id in body"); }
            var exists = this.repository.GetEPrescriptionData(ePrescriptionId);
            if (exists == null) { return StatusCode(400, "Invalid Eprescription.Id, Eprescription does not exist"); }
            if (body.Patient == null) return StatusCode(400, "Patient is null");
            if (body.Doctor == null) return StatusCode(400, "Doctor is null");
            if (!body.Doctor.Id.HasValue) return StatusCode(400, "DoctorId is null");
            if (!body.Patient.Id.HasValue) return StatusCode(400, "PatientId is null");
            var newEPrescription = body;

            var doctor = this.repository.GetUserData(newEPrescription.Doctor.Id.Value);
            if (!doctor.IsDoctor.Value) return StatusCode(400, "User is not a doctor");
            var patient = this.repository.GetUserData(newEPrescription.Patient.Id.Value);
            if (!patient.IsPatient.Value) return StatusCode(400, "User is not a patient");

            newEPrescription = this.repository.UpsertEPrescriptionData(newEPrescription);
            return StatusCode(200, newEPrescription);
        }

        /// <summary>
        /// Deletes a E-Prescriptions
        /// </summary>
        /// <param name="ePrescriptionId">E-Prescriptions  id to delete</param>
        /// <response code="400">E-Prescriptions  not found</response>
        /// <response code="200">successful operation</response>
        [HttpDelete]
        [Route("/api/eprescription/{ePrescriptionId}")]
        [ValidateModelState]
        [SwaggerOperation("DeleteEPrescriptions")]
        public virtual IActionResult DeleteEPrescriptions([FromRoute][Required] int ePrescriptionId)
        {
            var eprescription = this.repository.GetEPrescriptionData(ePrescriptionId);
            if (eprescription == null) { return StatusCode(400, "Invalid Eprescription.Id, Eprescription does not exist"); }
            this.repository.DeleteEPrescrition(ePrescriptionId);
            return StatusCode(200);
        }

        /// <summary>
        /// Get all E-Prescriptions 
        /// </summary>
        /// <response code="200">successful operation</response>
        [HttpGet]
        [Route("/api/eprescription")]
        [ValidateModelState]
        [SwaggerOperation("GetEPrescriptions")]
        [SwaggerResponse(statusCode: 200, type: typeof(IEnumerable<EPrescription>), description: "successful operation")]
        public virtual IActionResult GetEPrescriptions()
        {
            IEnumerable<EPrescription> ePrescriptions = this.repository.GetEPrescriptionData();
            return StatusCode(200, ePrescriptions);
        }
    }
}

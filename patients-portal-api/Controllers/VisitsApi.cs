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
    public class VisitsApiController : ControllerBase
    {
        private readonly IDataRepository repository;

        /// <summary/>
        public VisitsApiController(IDataRepository repository)
            => this.repository = repository;

        /// <summary>
        /// Add a new Visit
        /// </summary>
        /// <param name="body">Visit object</param>
        /// <response code="400">Invalid length supplied</response>
        /// <response code="200">successful operation</response>
        [HttpPost]
        [Route("/api/visits")]
        [ValidateModelState]
        [SwaggerOperation("AddVisit")]
        public virtual IActionResult AddVisit([FromBody] Visit body)
        {
            if (body.Length <= 0) return StatusCode(400, "Length of visit must be greater than 0 minutes");
            if (!body.Date.HasValue) return StatusCode(400, "Date is null");
            if (body.Patient == null) return StatusCode(400, "Patient is null");
            if (body.Doctor == null) return StatusCode(400, "Doctor is null");
            if (!body.Doctor.Id.HasValue) return StatusCode(400, "DoctorId is null");
            if (!body.Patient.Id.HasValue) return StatusCode(400, "PatientId is null");

            var newVisit = body;

            var doctor = this.repository.GetUserData(newVisit.Doctor.Id.Value);
            if (!doctor.IsDoctor.Value) return StatusCode(400, "User is not a doctor");
            var patient = this.repository.GetUserData(newVisit.Patient.Id.Value);
            if (!patient.IsPatient.Value) return StatusCode(400, "User is not a patient");


            IEnumerable<Visit> visitsForConcereteDay = this.repository.GetVisitsDataByDate(newVisit.Date.Value);

            foreach (var existingVisit in visitsForConcereteDay)
            {
                if (existingVisit.Doctor.Id == newVisit.Doctor.Id)
                {
                    DateTime existingVisitStart = existingVisit.Date.Value.ToUniversalTime();
                    DateTime existingVisitEnd = existingVisitStart.AddMinutes(existingVisit.Length.Value);
                    DateTime newVisitStart = newVisit.Date.Value.ToUniversalTime();
                    DateTime newVisitEnd = newVisitStart.AddMinutes(newVisit.Length.Value);

                    bool overlap = existingVisitStart < newVisitEnd && newVisitStart < existingVisitEnd;
                    if (overlap)
                    {
                        return StatusCode(400, "Doctor is already booked for this time period");
                    }
                }
            }
            var parsed = this.repository.UpsertVisitData(newVisit);
            return StatusCode(200, parsed);
        }

        /// <summary>
        /// Deletes a Visit
        /// </summary>
        /// <param name="visitId">Visit id to delete</param>
        /// <response code="400">Invalid ID supplied</response>
        /// <response code="200">successful operation</response>
        [HttpDelete]
        [Route("/api/visits/{visitId}")]
        [ValidateModelState]
        [SwaggerOperation("DeleteVisit")]
        public virtual IActionResult DeleteVisit([FromRoute][Required] int visitId)
        {
            var visit = this.repository.GetVisitData(visitId);
            if (visit == null) { return StatusCode(400, "Invalid ID supplied. Visit not found."); }
            this.repository.DeleteVisit(visitId);
            return StatusCode(200);
        }

        /// <summary>
        /// Get all visits
        /// </summary>
        /// <response code="200">successful operation</response>
        [HttpGet]
        [Route("/api/visits")]
        [ValidateModelState]
        [SwaggerOperation("GetVisits")]
        [SwaggerResponse(statusCode: 200, type: typeof(IEnumerable<Visit>), description: "successful operation")]
        public virtual IActionResult GetVisits()
        {
            IEnumerable<Visit> visits = this.repository.GetVisitsData();
            return StatusCode(200, visits);
        }

        /// <summary>
        /// Update an existing Visit
        /// </summary>
        /// <param name="visitId">Visit id to update</param>
        /// <param name="body">Visit object</param>
        /// <response code="400">Invalid ID supplied</response>
        /// <response code="200">successful operation</response>
        [HttpPut]
        [Route("/api/visits/{visitId}")]
        [ValidateModelState]
        [SwaggerOperation("UpdateVisit")]
        public virtual IActionResult UpdateVisit([FromRoute][Required] int visitId, [FromBody] Visit body)
        {
            if (!visitId.Equals(body.Id)) { return StatusCode(400, "Visit.Id from Path does not correspond with Visit.Id in body"); }
            var exists = this.repository.GetVisitData(visitId);
            if (exists == null) { return StatusCode(400, "Invalid ID supplied. Visit not found."); }
            if (body.Length <= 0) return StatusCode(400, "Length of visit must be greater than 0 minutes");
            if (!body.Date.HasValue) return StatusCode(400, "Date is null");
            if (body.Patient == null) return StatusCode(400, "Patient is null");
            if (body.Doctor == null) return StatusCode(400, "Doctor is null");
            if (!body.Doctor.Id.HasValue) return StatusCode(400, "DoctorId is null");
            if (!body.Patient.Id.HasValue) return StatusCode(400, "PatientId is null");

            var updateVisit = body;
            var doctor = this.repository.GetUserData(updateVisit.Doctor.Id.Value);
            if (!doctor.IsDoctor.Value) return StatusCode(400, "User is not a doctor");
            var patient = this.repository.GetUserData(updateVisit.Patient.Id.Value);
            if (!patient.IsPatient.Value) return StatusCode(400, "User is not a patient");


            IEnumerable<Visit> visitsForConcereteDay = this.repository.GetVisitsDataByDate(updateVisit.Date.Value);

            foreach (var existingVisit in visitsForConcereteDay)
            {
                if ((existingVisit.Doctor.Id == updateVisit.Doctor.Id) && (existingVisit.Id.Value != updateVisit.Id.Value))
                {
                    DateTime existingVisitStart = existingVisit.Date.Value.ToUniversalTime();
                    DateTime existingVisitEnd = existingVisitStart.AddMinutes(existingVisit.Length.Value);
                    DateTime updateVisitStart = updateVisit.Date.Value.ToUniversalTime();
                    DateTime updateVisitEnd = updateVisitStart.AddMinutes(updateVisit.Length.Value);

                    bool overlap = existingVisitStart < updateVisitEnd && updateVisitStart < existingVisitEnd;
                    if (overlap)
                    {
                        return StatusCode(400, "Doctor is already booked for this time period");
                    }
                }
            }

            var parsed = this.repository.UpsertVisitData(body);
            return StatusCode(200, parsed);
        }
    }
}

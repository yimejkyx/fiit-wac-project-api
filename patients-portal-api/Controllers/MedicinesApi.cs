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
    public class MedicinesApiController : ControllerBase
    {
        private readonly IDataRepository repository;

        /// <summary/>
        public MedicinesApiController(IDataRepository repository)
            => this.repository = repository;

        /// <summary>
        /// Get all medicines
        /// </summary>
        /// <response code="200">successful operation</response>
        [HttpGet]
        [Route("/api/medicines")]
        [ValidateModelState]
        [SwaggerOperation("GetMedicines")]
        [SwaggerResponse(statusCode: 200, type: typeof(IEnumerable<Medicine>), description: "successful operation")]
        public virtual IActionResult GetMedicines()
        {
            IEnumerable<Medicine> medicines = this.repository.GetMedicineData();
            return StatusCode(200, medicines);
        }

        /// <summary>
        /// Deletes a medicine
        /// </summary>
        /// <param name="medicineId">Medicine id to delete</param>
        /// <response code="400">Invalid ID supplied</response>
        /// <response code="404">Medicine not found</response>
        [HttpDelete]
        [Route("/api/medicines/{medicineId}")]
        [ValidateModelState]
        [SwaggerOperation("DeleteMedicine")]
        public virtual IActionResult DeleteMedicine([FromRoute][Required] int medicineId)
        {
            var medicine = this.repository.GetMedicineData(medicineId);
            if (medicine == null) { return new NotFoundResult(); }
            this.repository.DeleteMedicine(medicineId);
            return new OkResult();
        }
    }
}

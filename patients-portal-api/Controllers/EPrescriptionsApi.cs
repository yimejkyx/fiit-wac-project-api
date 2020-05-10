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
        /// <response code="405">Invalid input</response>
        /// <response code="200">successful operation</response>
        [HttpPost]
        [Route("/api/eprescription")]
        [ValidateModelState]
        [SwaggerOperation("AddEPrescriptions")]
        public virtual IActionResult AddEPrescriptions([FromBody]EPrescription body)
        { 
            if(body.Id == null) { return new BadRequestResult(); }

            var exist = this.repository.GetEPrescriptionData(body.Id);

            if (exist!= null){ 
                 return new BadRequestResult();
            }

            this.repository.UpsertEPrescriptionData(body);
            return StatusCode(200, body);
        }

        /// <summary>
        /// Deletes a E-Prescriptions
        /// </summary>
        /// <param name="ePrescriptionId">EPrescriptions id to delete</param>
        /// <response code="404">EPrescriptions not found</response>
        /// <response code="200">successful operation</response>
        [HttpDelete]
        [Route("/api/eprescription/{ePrescriptionId}")]
        [ValidateModelState]
        [SwaggerOperation("DeleteEPrescriptions")]
        public virtual IActionResult DeleteEPrescriptions([FromRoute][Required]string? ePrescriptionId)
        { 
            var eprescription = this.repository.GetEPrescriptionData(ePrescriptionId);
            if( eprescription == null) { return new NotFoundResult(); }
            this.repository.DeleteEPrescrition(ePrescriptionId);

            return new OkResult();
        }

        /// <summary>
        /// Get all e-prescriptions
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

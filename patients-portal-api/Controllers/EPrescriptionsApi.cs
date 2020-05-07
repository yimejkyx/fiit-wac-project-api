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

namespace eu.fiit.PatientsPortal.Controllers
{ 
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    public class EPrescriptionsApiController : ControllerBase
    { 
        /// <summary>
        /// Add a new E-Prescriptions
        /// </summary>
        /// <param name="body">E-Prescriptions object</param>
        /// <response code="405">Invalid input</response>
        [HttpPost]
        [Route("/yimejky/PatientsPortal/1.0.0/eprescription")]
        [ValidateModelState]
        [SwaggerOperation("AddEPrescriptions")]
        public virtual IActionResult AddEPrescriptions([FromBody]EPrescription body)
        { 
            //TODO: Uncomment the next line to return response 405 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(405);

            throw new NotImplementedException();
        }

        /// <summary>
        /// Deletes a E-Prescriptions
        /// </summary>
        /// <param name="ePrescriptionId">EPrescriptions id to delete</param>
        /// <response code="400">Invalid ID supplied</response>
        /// <response code="404">EPrescriptions not found</response>
        [HttpDelete]
        [Route("/yimejky/PatientsPortal/1.0.0/eprescription/{ePrescriptionId}")]
        [ValidateModelState]
        [SwaggerOperation("DeleteEPrescriptions")]
        public virtual IActionResult DeleteEPrescriptions([FromRoute][Required]long? ePrescriptionId)
        { 
            //TODO: Uncomment the next line to return response 400 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(400);

            //TODO: Uncomment the next line to return response 404 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(404);

            throw new NotImplementedException();
        }

        /// <summary>
        /// Get all e-prescriptions
        /// </summary>
        /// <response code="200">successful operation</response>
        [HttpGet]
        [Route("/yimejky/PatientsPortal/1.0.0/eprescription")]
        [ValidateModelState]
        [SwaggerOperation("GetEPrescriptions")]
        [SwaggerResponse(statusCode: 200, type: typeof(List<EPrescription>), description: "successful operation")]
        public virtual IActionResult GetEPrescriptions()
        { 
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200, default(List<EPrescription>));
            string exampleJson = null;
            exampleJson = "[ {\n  \"id\" : \"id\"\n}, {\n  \"id\" : \"id\"\n} ]";
            
                        var example = exampleJson != null
                        ? JsonConvert.DeserializeObject<List<EPrescription>>(exampleJson)
                        : default(List<EPrescription>);            //TODO: Change the data returned
            return new ObjectResult(example);
        }
    }
}

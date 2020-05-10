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
    public class MedicinesApiController : ControllerBase
    { 

        private IList<Medicine> _medicines;

        ///<Summary>
        /// Gets the answer
        ///</Summary>
        public MedicinesApiController() =>
            this._medicines = new List<Medicine>{
                new Medicine{
                    Id = "1",
                    Name = "Paralen"               
                },
                new Medicine{
                    Id = "2",
                    Name = "Ibalgin"               
                },
                new Medicine{
                    Id = "3",
                    Name = "Voltaren"               
                },
                new Medicine{
                    Id = "4",
                    Name = "Valetol"               
                }};
        
        /// <summary>
        /// Get all medicines
        /// </summary>
        /// <response code="200">successful operation</response>
        [HttpGet]
        [Route("/api/medicines")]
        [ValidateModelState]
        [SwaggerOperation("GetMedicines")]
        [SwaggerResponse(statusCode: 200, type: typeof(List<Medicine>), description: "successful operation")]
        public virtual IActionResult GetMedicines()
        { 
            return StatusCode(200, _medicines);
        }
    }
}

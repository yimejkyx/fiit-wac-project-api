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
    public class UsersApiController : ControllerBase
    { 
        private IList<User> _users;

        ///<Summary>
        /// Gets the answer
        ///</Summary>
        public UsersApiController() =>
            this._users = new List<User>{
                new User{
                    Id = "1",
                    Name = "Kamil Dzurman",
                    IsDoctor = true,
                    IsPatient = false               
                },
                new User{
                    Id = "2",
                    Name = "Matej Cief",
                    IsDoctor = true,
                    IsPatient = false              
                },
                new User{
                    Id = "3",
                    Name = "Ivan Hrozny",
                    IsDoctor = false,
                    IsPatient = true              
                },};

        /// <summary>
        /// Get all users
        /// </summary>
        /// <response code="200">successful operation</response>
        [HttpGet]
        [Route("/api/users")]
        [ValidateModelState]
        [SwaggerOperation("GetUsers")]
        [SwaggerResponse(statusCode: 200, type: typeof(List<User>), description: "successful operation")]
        public virtual IActionResult GetUsers()
        { 
            return StatusCode(200, _users);
        }
    }
}

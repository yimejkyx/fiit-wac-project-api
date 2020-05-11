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
    public class UsersApiController : ControllerBase
    {
        private readonly IDataRepository repository;

        /// <summary/>
        public UsersApiController(IDataRepository repository)
            => this.repository = repository;

        /// <summary>
        /// Get all users
        /// </summary>
        /// <response code="200">successful operation</response>
        [HttpGet]
        [Route("/api/users")]
        [ValidateModelState]
        [SwaggerOperation("GetUsers")]
        [SwaggerResponse(statusCode: 200, type: typeof(IEnumerable<User>), description: "successful operation")]
        public virtual IActionResult GetUsers()
        {
            IEnumerable<User> users = this.repository.GetUserData();
            return StatusCode(200, users);
        }

        /// <summary>
        /// Deletes a user
        /// </summary>
        /// <param name="userId">User id to delete</param>
        /// <response code="400">Invalid ID supplied</response>
        /// <response code="404">User not found</response>
        [HttpDelete]
        [Route("/api/users/{userId}")]
        [ValidateModelState]
        [SwaggerOperation("DeleteUser")]
        public virtual IActionResult DeleteUser([FromRoute][Required] int userId)
        {
            var user = this.repository.GetMedicineData(userId);
            if (user == null) { return new NotFoundResult(); }
            this.repository.DeleteUser(userId);
            return new OkResult();
        }
    }
}

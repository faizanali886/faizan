﻿using AzureNamingTool.Models;
using AzureNamingTool.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AzureNamingTool.Services;
using AzureNamingTool.Attributes;

namespace AzureNamingTool.Controllers
{
    /// <summary>
    /// Controller for managing resource environments.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [ApiKey]
    public class ResourceEnvironmentsController : ControllerBase
    {
        private ServiceResponse serviceResponse = new();
        // GET: api/<ResourceEnvironmentsController>
        /// <summary>
        /// This function will return the environments data. 
        /// </summary>
        /// <returns>json - Current environments data</returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                serviceResponse = await ResourceEnvironmentService.GetItems();
                if (serviceResponse.Success)
                {
                    return Ok(serviceResponse.ResponseObject);
                }
                else
                {
                    return BadRequest(serviceResponse.ResponseObject);
                }
            }
            catch (Exception ex)
            {
                AdminLogService.PostItem(new AdminLogMessage() { Title = "ERROR", Message = ex.Message });
                return BadRequest(ex);
            }
        }

        // GET api/<ResourceEnvironmentsController>/5
        /// <summary>
        /// This function will return the specifed environment data.
        /// </summary>
        /// <param name="id">int - Environment id</param>
        /// <returns>json - Environment data</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                serviceResponse = await ResourceEnvironmentService.GetItem(id);
                if (serviceResponse.Success)
                {
                    return Ok(serviceResponse.ResponseObject);
                }
                else
                {
                    return BadRequest(serviceResponse.ResponseObject);
                }
            }
            catch (Exception ex)
            {
                AdminLogService.PostItem(new AdminLogMessage() { Title = "ERROR", Message = ex.Message });
                return BadRequest(ex);
            }
        }

        // POST api/<ResourceEnvironmentsController>
        /// <summary>
        /// This function will create/update the specified environment data.
        /// </summary>
        /// <param name="item">ResourceEnvironment (json) - Environment data</param>
        /// <returns>bool - PASS/FAIL</returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ResourceEnvironment item)
        {
            try
            {
                serviceResponse = await ResourceEnvironmentService.PostItem(item);
                if (serviceResponse.Success)
                {
                    AdminLogService.PostItem(new AdminLogMessage() { Source = "API", Title = "INFORMATION", Message = "Resource Environment (" + item.Name + ") added/updated." });
                    CacheHelper.InvalidateCacheObject("ResourceEnvironment");
                    return Ok(serviceResponse.ResponseObject);
                }
                else
                {
                    return BadRequest(serviceResponse.ResponseObject);
                }
            }
            catch (Exception ex)
            {
                AdminLogService.PostItem(new AdminLogMessage() { Title = "ERROR", Message = ex.Message });
                return BadRequest(ex);
            }
        }

        // POST api/<ResourceEnvironmentsController>
        /// <summary>
        /// This function will update all environments data.
        /// </summary>
        /// <param name="items">List - ResourceEnvironment (json) - All environments data</param>
        /// <returns>bool - PASS/FAIL</returns>
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> PostConfig([FromBody] List<ResourceEnvironment> items)
        {
            try
            {
                serviceResponse = await ResourceEnvironmentService.PostConfig(items);
                if (serviceResponse.Success)
                {
                    AdminLogService.PostItem(new AdminLogMessage() { Source = "API", Title = "INFORMATION", Message = "Resource Environments added/updated." });
                    CacheHelper.InvalidateCacheObject("ResourceEnvironment");
                    return Ok(serviceResponse.ResponseObject);
                }
                else
                {
                    return BadRequest(serviceResponse.ResponseObject);
                }
            }
            catch (Exception ex)
            {
                AdminLogService.PostItem(new AdminLogMessage() { Title = "ERROR", Message = ex.Message });
                return BadRequest(ex);
            }
        }

        // DELETE api/<ResourceEnvironmentsController>/5
        /// <summary>
        /// This function will delete the specifed environment data.
        /// </summary>
        /// <param name="id">int - Environment id</param>
        /// <returns>bool - PASS/FAIL</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                // Get the item details
                serviceResponse = await ResourceEnvironmentService.GetItem(id);
                if (serviceResponse.Success)
                {
                    ResourceEnvironment item = (ResourceEnvironment)serviceResponse.ResponseObject!;
                    serviceResponse = await ResourceEnvironmentService.DeleteItem(id);
                    if (serviceResponse.Success)
                    {
                        AdminLogService.PostItem(new AdminLogMessage() { Source = "API", Title = "INFORMATION", Message = "Resource Environment (" + item.Name + ") deleted." });
                        CacheHelper.InvalidateCacheObject("ResourceEnvironment");
                        return Ok("Resource Environment (" + item.Name + ") deleted.");
                    }
                    else
                    {
                        return BadRequest(serviceResponse.ResponseObject);
                    }
                }
                else
                {
                    return BadRequest(serviceResponse.ResponseObject);
                }
            }
            catch (Exception ex)
            {
                AdminLogService.PostItem(new AdminLogMessage() { Title = "ERROR", Message = ex.Message });
                return BadRequest(ex);
            }
        }
    }
}

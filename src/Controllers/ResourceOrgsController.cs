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
    /// Controller for managing Resource Orgs.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [ApiKey]
    public class ResourceOrgsController : ControllerBase
    {
        private ServiceResponse serviceResponse = new();

        // GET: api/<ResourceOrgsController>
        /// <summary>
        /// This function will return the orgs data. 
        /// </summary>
        /// <returns>json - Current orgs data</returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                // Get list of items
                serviceResponse = await ResourceOrgService.GetItems();
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

        // GET api/<ResourceOrgsController>/5
        /// <summary>
        /// This function will return the specifed org data.
        /// </summary>
        /// <param name="id">int - Org id</param>
        /// <returns>json - Org data</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                // Get list of items
                serviceResponse = await ResourceOrgService.GetItem(id);
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

        // POST api/<ResourceOrgsController>
        /// <summary>
        /// This function will create/update the specified org data.
        /// </summary>
        /// <param name="item">ResourceOrg (json) - Org data</param>
        /// <returns>bool - PASS/FAIL</returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ResourceOrg item)
        {
            try
            {
                serviceResponse = await ResourceOrgService.PostItem(item);
                if (serviceResponse.Success)
                {
                    AdminLogService.PostItem(new AdminLogMessage() { Source = "API", Title = "INFORMATION", Message = "Resource Org (" + item.Name + ") added/updated." });
                    CacheHelper.InvalidateCacheObject("ResourceOrg");
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

        // POST api/<ResourceOrgsController>
        /// <summary>
        /// This function will update all orgs data.
        /// </summary>
        /// <param name="items">List - ResourceOrg (json) - All orgs data</param>
        /// <returns>bool - PASS/FAIL</returns>
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> PostConfig([FromBody] List<ResourceOrg> items)
        {
            try
            {
                serviceResponse = await ResourceOrgService.PostConfig(items);
                if (serviceResponse.Success)
                {
                    AdminLogService.PostItem(new AdminLogMessage() { Source = "API", Title = "INFORMATION", Message = "Resource Orgs added/updated." });
                    CacheHelper.InvalidateCacheObject("ResourceOrg");
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

        // DELETE api/<ResourceOrgsController>/5
        /// <summary>
        /// This function will delete the specifed org data.
        /// </summary>
        /// <param name="id">int - Org id</param>
        /// <returns>bool - PASS/FAIL</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                // Get the item details
                serviceResponse = await ResourceOrgService.GetItem(id);
                if (serviceResponse.Success)
                {
                    ResourceOrg item = (ResourceOrg)serviceResponse.ResponseObject!;
                    serviceResponse = await ResourceOrgService.DeleteItem(id);
                    if (serviceResponse.Success)
                    {
                        AdminLogService.PostItem(new AdminLogMessage() { Source = "API", Title = "INFORMATION", Message = "Resource Org (" + item.Name + ") deleted." });
                        CacheHelper.InvalidateCacheObject("ResourceOrg");
                        return Ok("Resource Org (" + item.Name + ") deleted.");
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
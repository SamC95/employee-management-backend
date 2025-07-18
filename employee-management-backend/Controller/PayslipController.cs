﻿using employee_management_backend.Model;
using employee_management_backend.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace employee_management_backend.Controller;

[ApiController]
[Route("api/v1/payslip")]
public class PayslipController(IPayslipService payslipService) : ControllerBase
{
    [HttpPost("create")]
    public async Task<IActionResult> CreatePayslip([FromBody] Payslip payslip)
    {
        try
        {
            await payslipService.CreatePayslip(payslip);

            return Ok(new { message = $"Payslip successfully created for {payslip.EmployeeName}" });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = $"An error occurred while creating Payslip: {ex.Message}" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"An unexpected error occurred. {ex.Message}" });
        }
    }
}

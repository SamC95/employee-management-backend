﻿using employee_management_backend.Model;

namespace employee_management_backend.Service.Interface;

public interface IPayslipService
{
    Task CreatePayslip(Payslip payslip);
}

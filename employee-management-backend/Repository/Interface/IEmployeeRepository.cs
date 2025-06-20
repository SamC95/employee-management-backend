﻿using employee_management_backend.Model;

namespace employee_management_backend.Repository.Interface;

public interface IEmployeeRepository
{
    Task CreateEmployee(Employee employee);
    
    Task UpdateEmployeeDetails(Employee employee);
    
    Task<bool> DeleteEmployee(string employeeId);
    
    Task<Employee?> GetEmployeeById(string? employeeId);
    
    Task<List<Employee>> GetEmployeesByJobTitle(string jobTitle);
    
    Task<bool?> CheckClockIdExists(string clockId);
}

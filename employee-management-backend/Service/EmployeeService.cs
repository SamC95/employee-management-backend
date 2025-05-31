using employee_management_backend.Model;
using employee_management_backend.Repository.Interface;
using employee_management_backend.Service.Interface;

namespace employee_management_backend.Service;

public class EmployeeService(IEmployeeRepository employeeRepository) : IEmployeeService
{
    public async Task CreateEmployee(Employee employee)
    {
        await employeeRepository.CreateEmployee(employee);
    }

    public async Task<bool> UpdateEmployeeDetails(EmployeeUpdater patch)
    {
        var employee = await GetEmployeeById(patch.EmployeeId);

        if (employee == null)
            return false;
        
        if (patch.FirstName is not null) employee.FirstName = patch.FirstName;
        if (patch.LastName is not null) employee.LastName = patch.LastName;
        if (patch.Email is not null) employee.Email = patch.Email;
        if (patch.PhoneNum is not null) employee.PhoneNum = patch.PhoneNum;
        if (patch.Address is not null) employee.Address = patch.Address;
        if (patch.City is not null) employee.City = patch.City;
        if (patch.PostCode is not null) employee.PostCode = patch.PostCode;
        if (patch.Country is not null) employee.Country = patch.Country;
        if (patch.Gender is not null) employee.Gender = patch.Gender;
        if (patch.DateOfBirth is not null) employee.DateOfBirth = (DateOnly)patch.DateOfBirth;
        if (patch.DateHired is not null) employee.DateHired = (DateOnly)patch.DateHired;
        if (patch.IsAdmin is not null) employee.IsAdmin = (bool)patch.IsAdmin;
        if (patch.IsManager is not null) employee.IsManager = (bool)patch.IsManager;
        if (patch.IsActive is not null) employee.IsActive = (bool)patch.IsActive;
        if (patch.JobTitle is not null) employee.JobTitle = patch.JobTitle;
        
        await employeeRepository.UpdateEmployeeDetails(employee);
        return true;
    }

    public async Task<bool> DeleteEmployee(string employeeId)
    {
        return await employeeRepository.DeleteEmployee(employeeId);
    }

    public async Task<Employee?> GetEmployeeById(string employeeId)
    {
        return await employeeRepository.GetEmployeeById(employeeId);
    }

    public async Task<List<Employee>> GetEmployeesByJobTitle(string jobTitle)
    {
        jobTitle = string.Join(" ", jobTitle.Split(" ").Select(word => char.ToUpper(word[0]) + word[1..]));
        
        return await employeeRepository.GetEmployeesByJobTitle(jobTitle);
    }

    public async Task<bool?> CheckClockIdExists(string clockId)
    {
        return await employeeRepository.CheckClockIdExists(clockId);
    }
}

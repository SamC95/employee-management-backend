using System.Reflection;
using employee_management_backend.Model;
using employee_management_backend.Repository.Interface;
using employee_management_backend.Service.Interface;
using employee_management_backend.Service.Utils.Passwords;

namespace employee_management_backend.Service;

public class EmployeeService(IEmployeeRepository employeeRepository) : IEmployeeService
{
    public async Task CreateEmployee(Employee employee)
    {
        var hashedPassword = HashPasswordUtil.PerformPasswordHash(employee.Password ?? string.Empty);
        employee.Password = hashedPassword;

        employee.HasSetOwnPassword = false;

        await employeeRepository.CreateEmployee(employee);
    }

    public async Task<bool> UpdateEmployeeDetails(EmployeeUpdater patch)
    {
        var employee = await GetEmployeeById(patch.EmployeeId);

        if (employee == null)
            return false;

        if (patch.Password != null)
        {
            var hashedPassword = HashPasswordUtil.PerformPasswordHash(patch.Password ?? string.Empty);
            patch.Password = hashedPassword;
            
            patch.HasSetOwnPassword = true;
        }

        var patchProperties = patch.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var employeeType = employee.GetType();

        foreach (var property in patchProperties)
        {
            var value = property.GetValue(patch);

            if (value == null) continue;
            var employeeProperty = employeeType.GetProperty(property.Name);

            if (employeeProperty == null || !employeeProperty.CanWrite) continue;
            var targetType = Nullable.GetUnderlyingType(property.PropertyType) ?? employeeProperty.PropertyType;
            var convertedValue = Convert.ChangeType(value, targetType);
                    
            employeeProperty.SetValue(employee, convertedValue);
        }

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
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManager.Helpers;
using EmployeeManager.Models;
using EmployeeManager.Services;
using Prism.Commands;

namespace EmployeeManager.ViewModels
{
    public class MainViewModel : BindableBase
    {
        #region Private Fields

        private readonly IApiService _apiService;
        private ObservableCollection<Employee> _employees;
        private Employee _newUser = new Employee();
        private string _searchQuery;
        private string _errorMessage;

        #endregion

        #region Constructor

        public MainViewModel(IApiService apiService)
        {
            _apiService = apiService;
            LoadEmployees();
        }

        #endregion

        #region Properties

        public ObservableCollection<Employee> Employees
        {
            get => _employees;
            set => SetProperty(ref _employees, value);
        }

        public Employee NewUser
        {
            get => _newUser;
            set => SetProperty(ref _newUser, value);
        }

        public List<string> GenderOptions { get; } = new List<string> { "male", "female" };
        public List<string> StatusOptions { get; } = new List<string> { "active", "inactive" };

        public string SearchQuery
        {
            get => _searchQuery;
            set => SetProperty(ref _searchQuery, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        #endregion

        #region Commands

        private DelegateCommand _searchCommand;
        public DelegateCommand SearchCommand =>
            _searchCommand ??= new DelegateCommand(ExecuteSearchCommand);

        private DelegateCommand<int?> _deleteCommand;
        public DelegateCommand<int?> DeleteCommand =>
            _deleteCommand ??= new DelegateCommand<int?>(ExecuteDeleteCommand);

        private DelegateCommand _createUserCommand;
        public DelegateCommand CreateUserCommand =>
            _createUserCommand ??= new DelegateCommand(async () => await ExecuteCreateUserCommand());

        public DelegateCommand ExportToCsvCommand =>
            new DelegateCommand(ExecuteExportToCsvCommand);

        #endregion

        #region Command Methods

        private async void ExecuteSearchCommand()
        {
            try
            {
                var results = await _apiService.SearchUsersByName(SearchQuery);
                Employees.Clear();
                if (results.IsSuccessful)
                {
                    foreach (var emp in results.Data)
                    {
                        Employees.Add(emp);
                    }
                    ErrorMessage = null;
                }
                else
                {
                    ErrorMessage = results.ErrorMessage;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ErrorHandler.HandleException(ex);
            }
        }

        private async void LoadEmployees()
        {
            try
            {
                var employeesResult = await _apiService.GetEmployees();
                if (employeesResult.IsSuccessful)
                {
                    Employees = new ObservableCollection<Employee>(employeesResult.Data);
                    ErrorMessage = null;
                }
                else
                {
                    ErrorMessage = employeesResult.ErrorMessage;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ErrorHandler.HandleException(ex);
            }
        }

        private async Task ExecuteCreateUserCommand()
        {
            try
            {
                var createdUserResult = await _apiService.CreateUser(NewUser);
                if (createdUserResult.IsSuccessful)
                {
                    Employees.Add(createdUserResult.Data);
                    NewUser = new Employee();
                    RaisePropertyChanged(nameof(NewUser));
                    ErrorMessage = null;
                }
                else
                {
                    ErrorMessage = createdUserResult.ErrorMessage;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ErrorHandler.HandleException(ex);
            }
        }

        private async void ExecuteDeleteCommand(int? userId)
        {
            if (!userId.HasValue) return;

            try
            {
                var deleteResult = await _apiService.DeleteUser(userId.Value);
                if (deleteResult.IsSuccessful)
                {
                    var employeeToRemove = Employees.FirstOrDefault(e => e.Id == userId.Value);
                    if (employeeToRemove != null)
                    {
                        Employees.Remove(employeeToRemove);
                    }
                    ErrorMessage = null;
                }
                else
                {
                    ErrorMessage = deleteResult.ErrorMessage;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ErrorHandler.HandleException(ex);
            }
        }

        private void ExecuteExportToCsvCommand()
        {
            CsvExportHelper.ExportToCsv(Employees);
        }

        #endregion
    }
}

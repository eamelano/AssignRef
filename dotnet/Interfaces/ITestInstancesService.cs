using Sabio.Models;
using Sabio.Models.Domain.TestInstances;
using Sabio.Models.Requests.TestAnswers;
using Sabio.Models.Requests.TestInstances;
using System;
using System.Collections.Generic;

namespace Sabio.Services.Interfaces
{
    public interface ITestInstancesService
    {
        int Add(TestInstanceAddRequest model, int userId);
        Paged<BaseTestInstance> SelectAllInstances(int pageIndex, int pageSize);
        Paged<BaseTestInstance> SelectByTestId(int id, int pageIndex, int pageSize);
        TestInstanceDetailed SelectByInstanceIdDetailed(int id);
        Paged<BaseTestInstance> SelectByUserId(int userId, int pageIndex, int pageSize);
        void UpdateStatus(int id, int statusId);
        void Update(TestInstanceUpdateRequest model, int id, int userId);
        Paged<TestInstanceAnswerCount> Search(int pageIndex, int pageSize, string query, DateTime startDate, DateTime endDate);
    }
}
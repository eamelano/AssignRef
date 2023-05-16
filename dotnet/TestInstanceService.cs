using AssignRef.Data.Providers;
using AssignRef.Models.Requests.TestInstances;
using AssignRef.Services.Interfaces;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using AssignRef.Models.Domain.TestInstances;
using AssignRef.Models;
using AssignRef.Data;
using System;
using AssignRef.Models.Requests.TestAnswers;

namespace AssignRef.Services
{
    public class TestInstanceService : ITestInstancesService
    {
        IBaseUserMapper _userMapper = null;
        IDataProvider _data = null;
        ILookUpService _lookUpService = null;
        ITestService _testService = null;
        public TestInstanceService(IDataProvider data, IBaseUserMapper userMapper, ITestService testService, ILookUpService lookUpService)
        {
            _userMapper = userMapper;
            _data = data;
            _lookUpService = lookUpService;
            _testService = testService;
        }

        public int Add(TestInstanceAddRequest model, int userId)
        {
            int id = 0;
            string procName = "[dbo].[Tests_Create]";

            _data.ExecuteNonQuery(procName, delegate (SqlParameterCollection col)
            {
                AddCommonParams(model, col);
                col.AddWithValue("UserId", userId);
                if (model.TestAnswers != null)
                {
                    DataTable table = MapAnswersToTable(model.TestAnswers);
                    col.AddWithValue("@BatchAnswers", table);
                }

                SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                idOut.Direction = ParameterDirection.Output;
                col.Add(idOut);

            }, delegate (SqlParameterCollection returnCol)
            {
                object oId = returnCol["@Id"].Value;
                int.TryParse(oId.ToString(), out id);
            });

            return id;
        }

        public Paged<TestInstanceAnswerCount> Search(int pageIndex, int pageSize, string query, DateTime startDate, DateTime endDate)
        {
            List<TestInstanceAnswerCount> list = null;
            Paged<TestInstanceAnswerCount> pagedList = null;
            int totalCount = 0;

            string procName = "[dbo].[Tests_Get_Query]";

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@PageIndex", pageIndex);
                col.AddWithValue("@PageSize", pageSize);
                col.AddWithValue("@Query", query);
                col.AddWithValue("@StartDate", startDate == default ? null : startDate);
                col.AddWithValue("@EndDate", endDate == default ? null : endDate);


            }, delegate (IDataReader reader, short set)
            {
                int indx = 0;
                TestInstanceAnswerCount instance = MapTestInstanceRow(reader, ref indx);

                if (totalCount == 0)
                {
                    totalCount = reader.GetSafeInt32(indx++);
                }

                list ??= new List<TestInstanceAnswerCount>();

                list.Add(instance);
            });
            if (list != null)
            {
                pagedList = new Paged<TestInstanceAnswerCount>(list, pageIndex, pageSize, totalCount);
            }

            return pagedList;
        }

        public Paged<BaseTestInstance> SelectAllInstances(int pageIndex, int pageSize)
        {
            List<BaseTestInstance> list = null;
            Paged<BaseTestInstance> pagedList = null;
            int totalCount = 0;

            string procName = "[dbo].[Tests_GetAll]";

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@PageIndex", pageIndex);
                col.AddWithValue("PageSize", pageSize);

            }, delegate (IDataReader reader, short set)
            {
                int indx = 0;
                BaseTestInstance instance = MapSingleTestInstance(reader, ref indx);

                if (totalCount == 0)
                {
                    totalCount = reader.GetSafeInt32(indx++);
                }

                list ??= new List<BaseTestInstance>();

                list.Add(instance);
            });
            if (list != null)
            {
                pagedList = new Paged<BaseTestInstance>(list, pageIndex, pageSize, totalCount);
            }

            return pagedList;
        }

        public Paged<BaseTestInstance> SelectByTestId(int id, int pageIndex, int pageSize)
        {
            List<BaseTestInstance> list = null;
            Paged<BaseTestInstance> pagedList = null;
            int totalCount = 0;

            string procName = "[Tests_Get_ByTestId]";

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection col)
            {
                col.AddWithValue("Id", id);
                col.AddWithValue("@PageIndex", pageIndex);
                col.AddWithValue("PageSize", pageSize);

            }, delegate (IDataReader reader, short set)
            {
                int indx = 0;
                BaseTestInstance instance = MapSingleTestInstance(reader, ref indx);

                if (totalCount == 0)
                {
                    totalCount = reader.GetSafeInt32(indx++);
                }

                list ??= new List<BaseTestInstance>();

                list.Add(instance);
            });
            if (list != null)
            {
                pagedList = new Paged<BaseTestInstance>(list, pageIndex, pageSize, totalCount);
            }

            return pagedList;
        }

        public Paged<BaseTestInstance> SelectByUserId(int userId, int pageIndex, int pageSize)
        {
            List<BaseTestInstance> list = null;
            Paged<BaseTestInstance> pagedList = null;
            int totalCount = 0;

            string procName = "[dbo].[Tests_Get_ByCreatedBy]";

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection col)
            {
                col.AddWithValue("UserId", userId);
                col.AddWithValue("@PageIndex", pageIndex);
                col.AddWithValue("PageSize", pageSize);

            }, delegate (IDataReader reader, short set)
            {
                int indx = 0;
                BaseTestInstance instance = MapSingleTestInstance(reader, ref indx);

                if (totalCount == 0)
                {
                    totalCount = reader.GetSafeInt32(indx++);
                }

                list ??= new List<BaseTestInstance>();

                list.Add(instance);
            });
            if (list != null)
            {
                pagedList = new Paged<BaseTestInstance>(list, pageIndex, pageSize, totalCount);
            }

            return pagedList;
        }

        public TestInstanceDetailed SelectByInstanceIdDetailed(int id)
        {
            TestInstanceDetailed instance = null;

            string procName = "[dbo].[Tests_Get_ByIdDetails]";

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection col)
            {
                col.AddWithValue("Id", id);

            }, delegate (IDataReader reader, short set)
            {
                int indx = 0;

                instance = MapSingleTestInstanceDetailed(reader, ref indx);
            });

            return instance;
        }

        public void UpdateStatus(int id, int statusId)
        {
            string procName = "[dbo].[Tests_Delete_ById]";

            _data.ExecuteNonQuery(procName, delegate (SqlParameterCollection col)
            {
                col.AddWithValue("Id", id);
                col.AddWithValue("StatusId", statusId);

            }, null);
        }

        public void Update(TestInstanceUpdateRequest model, int id, int userId)
        {
            string procName = "[dbo].[Tests_Update]";

            _data.ExecuteNonQuery(procName, delegate (SqlParameterCollection col)
            {
                AddCommonParams(model, col);
                col.AddWithValue("@Id", id);
                col.AddWithValue("UserId", userId);

            }, null);
        }

        private static void AddCommonParams(TestInstanceAddRequest model, SqlParameterCollection col)
        {
            col.AddWithValue("TestId", model.TestId);
            col.AddWithValue("StatusId", model.StatusId);
        }

        private BaseTestInstance MapSingleTestInstance(IDataReader reader, ref int indx)
        {
            BaseTestInstance instance = new BaseTestInstance();

            instance.Id = reader.GetSafeInt32(indx++);
            instance.DateCreated = reader.GetSafeDateTime(indx++);
            instance.Status = _lookUpService.MapSingleLookUp(reader, ref indx);
            instance.Test = _lookUpService.MapSingleLookUp(reader, ref indx);
            instance.TestType = _lookUpService.MapSingleLookUp(reader, ref indx);
            instance.User = _userMapper.MapBaseUser(reader, ref indx);
            return instance;
        }

        private TestInstanceAnswerCount MapTestInstanceRow(IDataReader reader, ref int indx)
        {
            TestInstanceAnswerCount instance = new TestInstanceAnswerCount();

            instance.Id = reader.GetSafeInt32(indx++);
            instance.DateCreated = reader.GetSafeDateTime(indx++);
            instance.Status = _lookUpService.MapSingleLookUp(reader, ref indx);
            instance.Test = _lookUpService.MapSingleLookUp(reader, ref indx);
            instance.TestType = _lookUpService.MapSingleLookUp(reader, ref indx);
            instance.User = _userMapper.MapBaseUser(reader, ref indx);
            instance.QuestionCount = reader.GetSafeInt32(indx++);
            instance.CorrectAnswers = reader.GetSafeInt32(indx++);
            return instance;
        }

        private TestInstanceDetailed MapSingleTestInstanceDetailed(IDataReader reader, ref int indx)
        {
            TestInstanceDetailed instance = new TestInstanceDetailed();

            instance.Id = reader.GetSafeInt32(indx++);
            instance.DateCreated = reader.GetSafeDateTime(indx++);
            instance.DateModified = reader.GetSafeDateTime(indx++);
            instance.Status = _lookUpService.MapSingleLookUp(reader, ref indx);

            instance.User = _userMapper.MapBaseUser(reader, ref indx);

            instance.Test = _testService.MapTestV1(reader, ref indx);

            instance.Questions = reader.DeserializeObject<List<TestQuestionDetailed>>(indx++);
        
            return instance;
        }

        private static DataTable MapAnswersToTable(List<BaseTestAnswerAddRequest> answers)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("@QuestionId", typeof(int));
            dt.Columns.Add("@AnswerOptionId", typeof(int));
            dt.Columns.Add("@Answer", typeof(string));

            foreach (var singleAnswer in answers)
            {
                DataRow dr = dt.NewRow();

                int startingIndex = 0;
                dr.SetField(startingIndex++, singleAnswer.QuestionId);
                dr.SetField(startingIndex++, singleAnswer.AnswerOptionId);
                dr.SetField(startingIndex++, singleAnswer.Answer);

                dt.Rows.Add(dr);
            }

            return dt;
        }

    }
}

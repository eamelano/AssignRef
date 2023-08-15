    [Route("api/tests/results")]
    [ApiController]
    public class TestInstancesApiController : BaseApiController
    {
        private ITestInstancesService _service = null;
        private IAuthenticationService<int> _authService = null;
        public TestInstancesApiController(ITestInstancesService service, IAuthenticationService<int> authService, ILogger<TestInstancesApiController> logger) : base(logger)
        {
            _service = service;
            _authService = authService;
        }

        [HttpPost]
        public ActionResult<ItemResponse<int>> Add(TestInstanceAddRequest model)
        {
            ObjectResult result = null;
            int userId = _authService.GetCurrentUserId();

            try
            {
                int id = _service.Add(model, userId);
                ItemResponse<int> response = new ItemResponse<int>() { Item = id };

                result = Created201(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                ErrorResponse response = new ErrorResponse(ex.Message);

                result = StatusCode(500, response);
            }
            return result;
        }

        [HttpGet("search")]
        public ActionResult<ItemResponse<Paged<TestInstanceAnswerCount>>> Search(int pageIndex, int pageSize, string query, DateTime startDate, DateTime endDate)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                Paged<TestInstanceAnswerCount> pagedList = _service.Search(pageIndex, pageSize, query, startDate, endDate);
                if (pagedList == null)
                {
                    code = 404;
                    response = new ErrorResponse("Records Not Found");
                }
                else
                {
                    response = new ItemResponse<Paged<TestInstanceAnswerCount>>() { Item = pagedList };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                Logger.LogError(ex.ToString());
                response = new ErrorResponse(ex.Message.ToString());
            }
            return StatusCode(code, response);
        }

        [HttpGet]
        public ActionResult<ItemResponse<Paged<BaseTestInstance>>> SelectAll(int pageIndex, int pageSize)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                Paged<BaseTestInstance> pagedList = _service.SelectAllInstances(pageIndex, pageSize);
                if (pagedList == null)
                {
                    code = 404;
                    response = new ErrorResponse("Records Not Found");
                }
                else
                {
                    response = new ItemResponse<Paged<BaseTestInstance>>() { Item = pagedList };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                Logger.LogError(ex.ToString());
                response = new ErrorResponse(ex.Message.ToString());
            }
            return StatusCode(code, response);
        }

        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<Paged<BaseTestInstance>>> SelectById(int id,int pageIndex, int pageSize)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                Paged<BaseTestInstance> pagedList = _service.SelectByTestId(id, pageIndex, pageSize);
                if (pagedList == null)
                {
                    code = 404;
                    response = new ErrorResponse("Records Not Found");
                }
                else
                {
                    response = new ItemResponse<Paged<BaseTestInstance>>() { Item = pagedList };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                Logger.LogError(ex.ToString());
                response = new ErrorResponse(ex.Message.ToString());
            }
            return StatusCode(code, response);
        }

        [HttpGet("user")]
        public ActionResult<ItemResponse<Paged<BaseTestInstance>>> SelectByUserId(int pageIndex, int pageSize)
        {
            int code = 200;
            BaseResponse response = null;
            int userId = _authService.GetCurrentUserId();

            try
            {
                Paged<BaseTestInstance> pagedList = _service.SelectByUserId(userId, pageIndex, pageSize);
                if (pagedList == null)
                {
                    code = 404;
                    response = new ErrorResponse("Records Not Found");
                }
                else
                {
                    response = new ItemResponse<Paged<BaseTestInstance>>() { Item = pagedList };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                Logger.LogError(ex.ToString());
                response = new ErrorResponse(ex.Message.ToString());
            }
            return StatusCode(code, response);
        }

        [HttpGet("detailed/{id:int}")]
        public ActionResult<ItemResponse<TestInstanceDetailed>> SelectByIdDetailed(int id)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                TestInstanceDetailed record = _service.SelectByInstanceIdDetailed(id);
                response = new ItemResponse<TestInstanceDetailed>() { Item = record };
            }
            catch (Exception ex)
            {
                code = 500;
                Logger.LogError(ex.ToString());
                response = new ErrorResponse(ex.Message.ToString());
            }

            return StatusCode(code, response);
        }

        [HttpDelete("{id:int}")]
        public ActionResult<SuccessResponse> SoftDelete(int id, int statusId)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                _service.UpdateStatus(id, statusId);

                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(code, response);
        }

        [HttpPut("{id:int}")]
        public ActionResult<SuccessResponse> Update(TestInstanceUpdateRequest model, int id)
        {
            int code = 200;
            BaseResponse response = null;
            int userId = _authService.GetCurrentUserId();

            try
            {
                _service.Update(model, id, userId);

                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(code, response);
        }

    }

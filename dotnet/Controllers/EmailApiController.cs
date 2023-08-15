    [Route("api/emails")]
    [ApiController]
    public class EmailApiController : BaseApiController
    {
        private IEmailService _service = null;
        public EmailApiController(IEmailService service,
            ILogger<EmailApiController> logger) : base(logger)
        {
            _service = service;
        }

        [HttpPost("sendemail")]
        public ActionResult<SuccessResponse> SendTransacEmail(EmailInformation model)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                _service.ReceiveEmailRequest(model);

                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(code, response);
        }

        [HttpPost("contact")]
        public ActionResult<SuccessResponse> ContactAdmin(ContactUsRequest userInfo)
        {
            int code = 200;
            BaseResponse response = null;

            try
            {
                _service.ContactUsRequest(userInfo);

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

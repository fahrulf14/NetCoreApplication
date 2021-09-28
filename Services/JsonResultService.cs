namespace NUNA.Services
{
    public class JsonResultService
    {
        public JsonResultDto Success(string url)
        {
            JsonResultDto model = new()
            {
                success = true,
                url = url,
                type = "success"
            };
            return model;
        }

        public JsonResultDto Warning(string message)
        {
            JsonResultDto model = new()
            {
                success = false,
                message = message,
                type = "warning"
            };
            return model;
        }

        public JsonResultDto Error(string message)
        {
            JsonResultDto model = new()
            {
                success = false,
                message = message,
                type = "error"
            };
            return model;
        }

        public class JsonResultDto
        {
            public bool success { get; set; }
            public string message { get; set; }
            public string type { get; set; }
            public string url { get; set; }
        }
    }
}

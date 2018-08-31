using System;
using System.Collections.Generic;
using System.Text;

namespace eolinkerToWord
{
    public class eoapi
    {
        public ProjectInfoModel ProjectInfo { get; set; }
        public List<ApiGroupModel> ApiGroupList { get; set; }
    }

    public class ProjectInfoModel
    {
        public int projectId { get; set; }
        public string projectName { get; set; }
    }

    public class ApiGroupModel
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public List<ApiModel> ApiList { get; set; }
    }

    public class ApiModel
    {
        public baseInfoModel BaseInfo { get; set; }
        public List<HeaderInfoModel> HeaderInfo { get; set; }
        public List<RequestInfoModel> RequestInfo { get; set; }
        public List<RequestInfoModel> ResultInfo { get; set; }
    }


    public class baseInfoModel
    {
        public string apiName { get; set; }
        public string apiURI { get; set; }
        private string _apiProtocol { get; set; }
        /// <summary>
        /// 接口协议
        /// </summary>
        public string apiProtocol
        {
            get
            {
                var ret = "XXX";
                switch (_apiProtocol)
                {
                    case "0":
                        ret = "HTTP";
                        break;
                    case "1":
                        ret = "HTTPS";
                        break;
                    default:
                        break;
                }
                return ret;
            }
            set => _apiProtocol = value;
        }

        public string apiSuccessMock { get; set; }

        private string _apiRequestType;
        /// <summary>
        /// 请求类型
        /// </summary>
        public string apiRequestType
        {
            get
            {
                var ret = "XXX";
                switch (_apiRequestType)
                {
                    case "0":
                        ret = "POST";
                        break;
                    case "1":
                        ret = "GET";
                        break;
                    default:
                        break;
                }
                return ret;
            }
            set => _apiRequestType = value;
        }
        private string _apiStatus;
        /// <summary>
        /// 接口状态
        /// </summary>
        public string apiStatus {
            get
            {
                var ret = "XXX";
                switch (_apiStatus)
                {
                    case "0":
                        ret = "启用";
                        break;
                    case "1":
                        ret = "维护";
                        break;
                    case "2":
                        ret = "弃用";
                        break;
                    default:
                        break;
                }
                return ret;
            }
            set => _apiStatus = value;
        }
        /// <summary>
        /// 星标：0-否，1-是
        /// </summary>
        public int starred { get; set; }

    }

    public class HeaderInfoModel
    {
        public string headerName { get; set; }
        public string headerValue { get; set; }
    }


    public class RequestInfoModel
    {
        private string _paramNotNull;
        /// <summary>
        /// 是都必填：0-必填；1-选填
        /// </summary>
        public string paramNotNull {
            get
            {
                var ret = "XXX";
                switch (_paramNotNull)
                {
                    case "0":
                        ret = "Y";
                        break;
                    case "1":
                        ret = "N";
                        break;
                    default:
                        break;
                }
                return ret;
            }
            set => _paramNotNull = value;
        }

        private string _paramType;
        /// <summary>
        /// 参数类型
        /// </summary>
        public string paramType
        {
            get
            {
                var ret = "XXX";
                switch (_paramType)
                {
                    case "0":
                        ret = "text";
                        break;
                    case "1":
                        ret = "file";
                        break;
                    case "2":
                        ret = "json";
                        break;
                    case "3":
                        ret = "int";
                        break;
                    case "4":
                        ret = "float";
                        break;
                    case "5":
                        ret = "double";
                        break;
                    case "6":
                        ret = "date";
                        break;
                    case "7":
                        ret = "datetime";
                        break;
                    case "8":
                        ret = "boolean";
                        break;
                    case "9":
                        ret = "byte";
                        break;
                    case "10":
                        ret = "short";
                        break;
                    case "11":
                        ret = "long";
                        break;
                    default:
                        break;
                }
                return ret;
            }
            set => _paramType = value;
        }
        public string paramName { get; set; }
        public string paramKey { get; set; }
        public string paramValue { get; set; }
        public string paramLimit { get; set; }
        public string paramNote { get; set; }
        public List<paramValue> paramValueList { get; set; }
    }

    public class paramValue
    {
        public string value { get; set; }
        public string valueDescription { get; set; }
    }
}

using Microsoft.AspNetCore.Builder;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Isearch.services
{
    public interface ILogger
    {
        void Logger(object obj);
    }
    public class Logger : ILogger
    {
        string url = "http://ip.lxgreg.cn:19200/ysqlog/doc/";
        void ILogger.Logger(object obj)        {

            req.Post(obj, url);
        }
    }

}
